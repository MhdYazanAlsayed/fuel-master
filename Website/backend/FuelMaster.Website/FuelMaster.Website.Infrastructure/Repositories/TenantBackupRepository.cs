using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class TenantBackupRepository : Repository<TenantBackup>, ITenantBackupRepository
{
    public TenantBackupRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TenantBackup>> GetBackupsByTenantIdAsync(Guid tenantId)
    {
        return await _dbSet
            .Include(b => b.Tenant)
            .Where(b => b.TenantId == tenantId)
            .OrderByDescending(b => b.BackupDate)
            .ToListAsync();
    }

    public async Task<TenantBackup?> GetLatestBackupByTenantIdAsync(Guid tenantId)
    {
        return await _dbSet
            .Include(b => b.Tenant)
            .Where(b => b.TenantId == tenantId)
            .OrderByDescending(b => b.BackupDate)
            .FirstOrDefaultAsync();
    }
}

