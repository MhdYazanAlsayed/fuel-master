using FuelMaster.Website.Attributes;
using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Responses;
using FuelMaster.Website.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/tenants")]
[Authorize]
public class TenantController : ControllerBase
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<TenantController> _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TenantController(
        ITenantService tenantService,
        ILogger<TenantController> logger,
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager)
    {
        _tenantService = tenantService;
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTenant([FromBody] DTOs.Requests.CreateTenantRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenant = await _tenantService.CreateTenantAsync(userId, new FuelMaster.Website.Core.Interfaces.CreateTenantRequest
            {
                Name = request.Name,
            });

            // Refresh token to include updated tenant information
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);
                Response.SetAuthCookie(token, expiresAt);
            }

            var response = new TenantResponse
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                PlanId = tenant.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = tenant.Plan.Id,
                    Name = tenant.Plan.Name,
                    Description = tenant.Plan.Description,
                    Price = tenant.Plan.Price,
                    BillingCycle = tenant.Plan.BillingCycle.ToString(),
                    IsFree = tenant.Plan.IsFree,
                    Features = tenant.Plan.Features,
                    IsActive = tenant.Plan.IsActive
                },
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            _logger.LogInformation("User {UserId} created tenant {TenantName}", userId, tenant.Name);

            return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tenant");
            return StatusCode(500, new { message = "An error occurred while creating the tenant" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTenant(Guid id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            // Check if user owns this tenant or is root admin
            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var response = new TenantResponse
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                PlanId = tenant.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = tenant.Plan.Id,
                    Name = tenant.Plan.Name,
                    Description = tenant.Plan.Description,
                    Price = tenant.Plan.Price,
                    BillingCycle = tenant.Plan.BillingCycle.ToString(),
                    IsFree = tenant.Plan.IsFree,
                    Features = tenant.Plan.Features,
                    IsActive = tenant.Plan.IsActive
                },
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant {TenantId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the tenant" });
        }
    }

    //<summary>
    // Get all tenants in this system to used by Headoffice system.
    //</summary>
    [HttpGet]
    [AllowAnonymous]
    [RequiredApiKey]
    public async Task<IActionResult> GetTenants()
    {
        try
        {
            var tenants = await _tenantService.GetAllTenantsAsync();

            return Ok(tenants.Select(x => new 
            {
                TenantId = x.Id,
                DatabaseName = x.DatabaseName,
                TenantName = x.Name,
                IsActive = x.Status == TenantStatus.Active,
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenants");
            return StatusCode(500, new { message = "An error occurred while retrieving the tenants" });
        }
    }


    [HttpGet("name/{name}")]
    [RequireActiveTenantAndSubscription]
    public async Task<IActionResult> GetTenantByName(string name)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenant = await _tenantService.GetTenantByNameAsync(name);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            // Check if user owns this tenant or is root admin
            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var response = new TenantResponse
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                PlanId = tenant.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = tenant.Plan.Id,
                    Name = tenant.Plan.Name,
                    Description = tenant.Plan.Description,
                    Price = tenant.Plan.Price,
                    BillingCycle = tenant.Plan.BillingCycle.ToString(),
                    IsFree = tenant.Plan.IsFree,
                    Features = tenant.Plan.Features,
                    IsActive = tenant.Plan.IsActive
                },
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant {TenantName}", name);
            return StatusCode(500, new { message = "An error occurred while retrieving the tenant" });
        }
    }

    [HttpGet("my-tenant")]
    [RequireActiveTenantAndSubscription]
    public async Task<IActionResult> GetMyTenant()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenant = await _tenantService.GetTenantByUserIdAsync(userId);
            if (tenant == null)
            {
                return NotFound(new { message = "No tenant found for this user" });
            }

            var response = new TenantResponse
            {
                Id = tenant.Id,
                Name = tenant.Name,
                DatabaseName = tenant.DatabaseName,
                Status = tenant.Status.ToString(),
                PlanId = tenant.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = tenant.Plan.Id,
                    Name = tenant.Plan.Name,
                    Description = tenant.Plan.Description,
                    Price = tenant.Plan.Price,
                    BillingCycle = tenant.Plan.BillingCycle.ToString(),
                    IsFree = tenant.Plan.IsFree,
                    Features = tenant.Plan.Features,
                    IsActive = tenant.Plan.IsActive
                },
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user tenant");
            return StatusCode(500, new { message = "An error occurred while retrieving the tenant" });
        }
    }

    [HttpGet("my-tenants")]
    [RequireActiveTenantAndSubscription]
    public async Task<IActionResult> GetMyTenants()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenants = await _tenantService.GetUserTenantsAsync(userId);

            var response = tenants.Select(t => new TenantResponse
            {
                Id = t.Id,
                Name = t.Name,
                DatabaseName = t.DatabaseName,
                Status = t.Status.ToString(),
                PlanId = t.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = t.Plan.Id,
                    Name = t.Plan.Name,
                    Description = t.Plan.Description,
                    Price = t.Plan.Price,
                    BillingCycle = t.Plan.BillingCycle.ToString(),
                    IsFree = t.Plan.IsFree,
                    Features = t.Plan.Features,
                    IsActive = t.Plan.IsActive
                },
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user tenants");
            return StatusCode(500, new { message = "An error occurred while retrieving tenants" });
        }
    }

    [HttpPut("{id}")]
    [RequireActiveTenantAndSubscription]
    public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] DTOs.Requests.UpdateTenantRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            // Check if user owns this tenant or is root admin
            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var updateRequest = new FuelMaster.Website.Core.Interfaces.UpdateTenantRequest
            {
                Name = request.Name,
                Status = !string.IsNullOrEmpty(request.Status) && Enum.TryParse<TenantStatus>(request.Status, out var status)
                    ? status
                    : null
            };

            var updatedTenant = await _tenantService.UpdateTenantAsync(id, updateRequest);

            var response = new TenantResponse
            {
                Id = updatedTenant.Id,
                Name = updatedTenant.Name,
                DatabaseName = updatedTenant.DatabaseName,
                Status = updatedTenant.Status.ToString(),
                PlanId = updatedTenant.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = updatedTenant.Plan.Id,
                    Name = updatedTenant.Plan.Name,
                    Description = updatedTenant.Plan.Description,
                    Price = updatedTenant.Plan.Price,
                    BillingCycle = updatedTenant.Plan.BillingCycle.ToString(),
                    IsFree = updatedTenant.Plan.IsFree,
                    Features = updatedTenant.Plan.Features,
                    IsActive = updatedTenant.Plan.IsActive
                },
                CreatedAt = updatedTenant.CreatedAt,
                UpdatedAt = updatedTenant.UpdatedAt
            };

            _logger.LogInformation("Tenant {TenantId} updated by user {UserId}", id, userId);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant {TenantId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the tenant" });
        }
    }

    [HttpDelete("{id}")]
    [RequireActiveTenantAndSubscription]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _tenantService.DeleteTenantAsync(id, userId);
            if (!result)
            {
                return NotFound(new { message = "Tenant not found or does not belong to user" });
            }

            _logger.LogInformation("Tenant {TenantId} deleted by user {UserId}", id, userId);

            return Ok(new { message = "Tenant deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tenant {TenantId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the tenant" });
        }
    }

}

