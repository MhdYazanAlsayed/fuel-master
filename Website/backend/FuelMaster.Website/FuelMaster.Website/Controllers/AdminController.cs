using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Requests;
using FuelMaster.Website.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "RootAdmin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ITenantService _tenantService;
    private readonly UserManager<Core.Entities.ApplicationUser> _userManager;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IAdminService adminService,
        ITenantService tenantService,
        UserManager<Core.Entities.ApplicationUser> userManager,
        ILogger<AdminController> logger)
    {
        _adminService = adminService;
        _tenantService = tenantService;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _adminService.GetAllUsersAsync();

            var response = new List<UserResponse>();
            foreach (var user in users)
            {
                var tenantCount = await _tenantService.GetUserTenantsAsync(user.Id);
                var subscriptions = await _userManager.Users
                    .Include(u => u.Subscriptions)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                response.Add(new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt,
                    TenantCount = tenantCount.Count(),
                    SubscriptionCount = subscriptions?.Subscriptions.Count ?? 0
                });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var tenantCount = await _tenantService.GetUserTenantsAsync(userId);
            var subscriptions = await _userManager.Users
                .Include(u => u.Subscriptions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var response = new UserResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                TenantCount = tenantCount.Count(),
                SubscriptionCount = subscriptions?.Subscriptions.Count ?? 0
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    [HttpPut("users/{userId}/root-admin")]
    public async Task<IActionResult> SetRootAdmin(string userId, [FromBody] SetRootAdminRequest request)
    {
        try
        {
            var result = await _adminService.SetRootAdminAsync(userId, request.IsRootAdmin);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            _logger.LogInformation("Root admin status for user {UserId} set to {IsRootAdmin}", userId, request.IsRootAdmin);

            return Ok(new { message = $"User root admin status updated to {request.IsRootAdmin}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting root admin status for user {UserId}", userId);
            return StatusCode(500, new { message = "An error occurred while updating root admin status" });
        }
    }

    [HttpGet("tenants")]
    public async Task<IActionResult> GetAllTenants()
    {
        try
        {
            var tenants = await _adminService.GetAllTenantsAsync();

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
            _logger.LogError(ex, "Error retrieving all tenants");
            return StatusCode(500, new { message = "An error occurred while retrieving tenants" });
        }
    }

    [HttpGet("health")]
    public async Task<IActionResult> GetSystemHealth()
    {
        try
        {
            var health = await _adminService.GetSystemHealthAsync();

            var response = new SystemHealthResponse
            {
                TotalUsers = health.TotalUsers,
                ActiveUsers = health.ActiveUsers,
                TotalTenants = health.TotalTenants,
                ActiveTenants = health.ActiveTenants,
                TotalSubscriptions = health.TotalSubscriptions,
                ActiveSubscriptions = health.ActiveSubscriptions,
                LastChecked = health.LastChecked
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system health");
            return StatusCode(500, new { message = "An error occurred while retrieving system health" });
        }
    }
}

