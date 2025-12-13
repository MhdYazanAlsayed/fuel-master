using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<bool> SetRootAdminAsync(string userId, bool isRootAdmin);
    Task<IEnumerable<Tenant>> GetAllTenantsAsync();
    Task<SystemHealthInfo> GetSystemHealthAsync();
}

public class SystemHealthInfo
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalTenants { get; set; }
    public int ActiveTenants { get; set; }
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public DateTime LastChecked { get; set; } = DateTime.UtcNow;
}

