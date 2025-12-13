using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(string userId, CreateTenantRequest request);
    Task<Tenant?> GetTenantByIdAsync(Guid tenantId);
    Task<Tenant?> GetTenantByNameAsync(string tenantName);
    Task<Tenant?> GetTenantByUserIdAsync(string userId);
    Task<IEnumerable<Tenant>> GetUserTenantsAsync(string userId);
    Task<Tenant> UpdateTenantAsync(Guid tenantId, UpdateTenantRequest request);
    Task<bool> DeleteTenantAsync(Guid tenantId, string userId);
    Task<string> GetTenantConnectionStringAsync(string tenantName);
    Task<IEnumerable<Tenant>> GetAllTenantsAsync(bool isActive = true);
}

public class CreateTenantRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateTenantRequest
{
    public string? Name { get; set; }
    public Enums.TenantStatus? Status { get; set; }
}

