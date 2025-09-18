using FuelMaster.HeadOffice.Core.Configurations;
using System.Security.Claims;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TenantConfiguration _configuration;
        private const string TenantIdHeader = "X-Tenant-ID";
        private const string TenantIdQueryParam = "tenantId";
        private const string TenantIdClaimType = "TenantId";
        private const string TenantIdItemKey = "TenantId";

        public TenantMiddleware(RequestDelegate next, TenantConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var tenantId = ExtractTenantId(context);
                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    await ReturnErrorResponse(context, StatusCodes.Status400BadRequest, "TenantId is required");
                    return;
                }

                var tenant = ValidateTenant(tenantId);
                if (tenant is null)
                {
                    await ReturnErrorResponse(context, StatusCodes.Status404NotFound, $"Tenant '{tenantId}' not found");
                    return;
                }

                if (!ValidateTenantClaim(context, tenantId))
                {
                    await ReturnErrorResponse(context, StatusCodes.Status403Forbidden, "Tenant access denied");
                    return;
                }

                SetTenantContext(context, tenantId);
                await _next(context);
            }
            catch (Exception ex)
            {
                await ReturnErrorResponse(context, StatusCodes.Status500InternalServerError, "An error occurred while processing tenant information");
            }
        }

        private string? ExtractTenantId(HttpContext context)
        {
            // Try to get tenant ID from header first
            var tenantId = context.Request.Headers[TenantIdHeader].FirstOrDefault();
            
            // If not found in header, try query parameter
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                tenantId = context.Request.Query[TenantIdQueryParam].FirstOrDefault();
            }

            return tenantId;
        }

        private TenantItem? ValidateTenant(string tenantId)
        {
            return _configuration.Tenants.FirstOrDefault(x => 
                string.Equals(x.TenantId, tenantId, StringComparison.OrdinalIgnoreCase));
        }

        private bool ValidateTenantClaim(HttpContext context, string tenantId)
        {
            var tenantClaim = context.User.Claims.FirstOrDefault(x => 
                string.Equals(x.Type, TenantIdClaimType, StringComparison.OrdinalIgnoreCase));

            // If no claim exists, allow the request (for unauthenticated endpoints)
            if (tenantClaim is null)
                return true;

            // If claim exists, it must match the tenant ID
            return string.Equals(tenantClaim.Value, tenantId, StringComparison.OrdinalIgnoreCase);
        }

        private void SetTenantContext(HttpContext context, string tenantId)
        {
            context.Items[TenantIdItemKey] = tenantId;
        }

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
