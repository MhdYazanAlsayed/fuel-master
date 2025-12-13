using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ITenantBackupRepository : IRepository<TenantBackup>
{
    Task<IEnumerable<TenantBackup>> GetBackupsByTenantIdAsync(Guid tenantId);
    Task<TenantBackup?> GetLatestBackupByTenantIdAsync(Guid tenantId);
}

