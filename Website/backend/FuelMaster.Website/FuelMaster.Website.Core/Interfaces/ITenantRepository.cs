using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant?> GetByNameAsync(string name);
    Task<Tenant?> GetByUserIdAsync(string userId);
    Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(string userId);
    Task<bool> TenantNameExistsAsync(string name);
}

