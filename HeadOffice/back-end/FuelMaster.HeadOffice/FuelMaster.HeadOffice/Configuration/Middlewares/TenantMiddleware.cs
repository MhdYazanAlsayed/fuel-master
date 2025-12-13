using FuelMaster.HeadOffice.Application.Constants;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Infrastructure.Configurations;
using FuelMaster.HeadOffice.Services;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    /// <summary>
    /// This middleware is used to set the current tenant context for the request.
    /// It will set the tenant id and connection string in the current tenant service.
    /// </summary>
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FuelMasterWebsiteConfiguration _fuelMasterWebsiteConfiguration;
        public TenantMiddleware(RequestDelegate next, FuelMasterWebsiteConfiguration fuelMasterWebsiteConfiguration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _fuelMasterWebsiteConfiguration = fuelMasterWebsiteConfiguration ?? throw new ArgumentNullException(nameof(fuelMasterWebsiteConfiguration));
        }

        public async Task InvokeAsync(HttpContext context, ITenants tenants, ICurrentTenant _tanentService)
        {
            try
            {
                if (IsRequestDoesNotAuthenticatedYetAndAllowedToLogin(context))
                {
                    await _next(context);
                    return;
                }

                // Check if the request is from the FuelMasterWebsite
                if (CheckApiKeyInHeader(context))
                {
                    if (!ValidateRequestFromFuelMasterWebsite(context))
                    {
                        await ReturnErrorResponse(context, StatusCodes.Status401Unauthorized, "Invalid API key");
                        return;
                    }

                    await _next(context);
                    return;
                }
              
                var tenantId = GetTenantFromToken(context);
                if (tenantId is null)
                {
                    await ReturnErrorResponse(context, StatusCodes.Status400BadRequest, "TenantId is required");
                    return;
                }

                var tenant =  await GetTenantInfoAsync(tenants, tenantId.Value);
                if (tenant is null)
                {
                    await ReturnErrorResponse(context, StatusCodes.Status404NotFound, $"Tenant '{tenantId}' not found");
                    return;
                }

                if (!tenant.IsActive)
                {
                    await ReturnErrorResponse(context, StatusCodes.Status403Forbidden, $"Tenant '{tenantId}' is not active");
                    return;
                }

                SetTenantContext(context, tenant, _tanentService);
                await _next(context);
            }
            catch (Exception)
            {
                await ReturnErrorResponse(context, StatusCodes.Status500InternalServerError, "An error occurred while processing tenant information");
            }
        }


        /// <summary>
        /// This method is used to check if the request is not authenticated yet.
        /// It will check if the user is authenticated.
        /// If user is not authenticated, the request will be allowed to proceed only to login endpoint.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>True if the request is not authenticated yet, false otherwise.</returns>
        public bool IsRequestDoesNotAuthenticatedYetAndAllowedToLogin(HttpContext context)
        {
            return !context.User.Identity?.IsAuthenticated ?? true && context.Request.Path.StartsWithSegments("/api/auth/login");
        }

        /// <summary>
        /// This method is used to validate the request from the FuelMasterWebsite.
        /// It will check if the API key is valid.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>True if the request is from the FuelMasterWebsite, false otherwise.</returns>

        private bool ValidateRequestFromFuelMasterWebsite(HttpContext context)
        {
            var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
            return string.Equals(apiKey, _fuelMasterWebsiteConfiguration.ApiKey, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// This method is used to check if the API key is in the header.
        /// It will check if the API key is valid.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>True if the API key is in the header, false otherwise.</returns>
        private bool CheckApiKeyInHeader(HttpContext context)
        {
            var apiKey = context.Request.Headers["X-Api-Key"].FirstOrDefault();
            return !string.IsNullOrWhiteSpace(apiKey);
        }

        /// <summary>
        /// This method is used to get the tenant information from the memory.
        /// It will return the tenant information from the memory.
        /// If tenant was not found, the service will call the FuelMasterAPI to get the latest updated tenants.
        /// </summary>
        /// <param name="tenants">The tenants service.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <returns>The tenant information.</returns>
        private async Task<TenantConfig?> GetTenantInfoAsync(ITenants tenants, Guid tenantId)
        {
            return await tenants.GetTenantAsync(tenantId);
        }

        /// <summary>
        /// This method is used to get the tenant id from the token.
        /// It will return the tenant id from the token.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The tenant id.</returns>
        private Guid? GetTenantFromToken(HttpContext context)
        {
            var tenantClaim = context.User.Claims.FirstOrDefault(x => 
                string.Equals(x.Type, ConfigKeys.TanentId, StringComparison.OrdinalIgnoreCase));

            // If no claim exists, allow the request (for unauthenticated endpoints)
            if (tenantClaim is null)
                return null;

            // If claim exists, it must match the tenant ID
            return Guid.Parse(tenantClaim.Value!);
        }


        /// <summary>
        /// This method is used to set the tenant context in the current tenant service (Memory).
        /// It will set the tenant id and connection string in the current tenant service.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="tenant">The tenant information.</param>
        /// <param name="tanentService">The current tenant service.</param>
        private void SetTenantContext(HttpContext context, TenantConfig tenant, ICurrentTenant _tanentService)
        {
            // context.Items[ConfigKeys.TanentId] = tenantId;
            var tenantService = _tanentService as CurrentTenant;
            if (tenantService is null)
            {
                throw new InvalidOperationException("TanentService is not registered");
            }
            
            tenantService.TenantId = tenant.TenantId;
            tenantService.ConnectionString = tenant.ConnectionString;
        }

        /// <summary>
        /// This method is used to return the error response.
        /// It will return the error response in the JSON format.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>The error response.</returns>
        private async Task ReturnErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            
            var errorResponse = new
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
