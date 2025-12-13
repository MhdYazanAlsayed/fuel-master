using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.Website.Core.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly ILogger<AdminService> _logger;

    public AdminService(
        UserManager<ApplicationUser> userManager,
        ITenantRepository tenantRepository,
        IUserSubscriptionRepository subscriptionRepository,
        ILogger<AdminService> logger)
    {
        _userManager = userManager;
        _tenantRepository = tenantRepository;
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<bool> SetRootAdminAsync(string userId, bool isRootAdmin)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Add or remove from RootAdmin role
        if (isRootAdmin)
        {
            if (!await _userManager.IsInRoleAsync(user, "RootAdmin"))
            {
                var result = await _userManager.AddToRoleAsync(user, "RootAdmin");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} added to RootAdmin role", userId);
                }
                return result.Succeeded;
            }
        }
        else
        {
            if (await _userManager.IsInRoleAsync(user, "RootAdmin"))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, "RootAdmin");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} removed from RootAdmin role", userId);
                }
                return result.Succeeded;
            }
        }

        return true;
    }

    public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
    {
        return await _tenantRepository.GetAllAsync();
    }

    public async Task<SystemHealthInfo> GetSystemHealthAsync()
    {
        var allUsers = await _userManager.Users.ToListAsync();
        var allTenants = await _tenantRepository.GetAllAsync();
        var allSubscriptions = await _subscriptionRepository.GetAllAsync();

        var activeSubscriptions = allSubscriptions
            .Where(s => s.Status == SubscriptionStatus.Active)
            .ToList();

        var activeTenants = allTenants
            .Where(t => t.Status == TenantStatus.Active)
            .ToList();

        // Active users are those who have logged in within the last 30 days
        // For now, we'll consider all users as active (can be enhanced later)
        var activeUsers = allUsers.Count;

        return new SystemHealthInfo
        {
            TotalUsers = allUsers.Count,
            ActiveUsers = activeUsers,
            TotalTenants = allTenants.Count(),
            ActiveTenants = activeTenants.Count,
            TotalSubscriptions = allSubscriptions.Count(),
            ActiveSubscriptions = activeSubscriptions.Count,
            LastChecked = DateTime.UtcNow
        };
    }
}

