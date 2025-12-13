using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IBackupService
{
    Task<TenantBackup> CreateBackupAsync(Guid tenantId, BackupServiceCreateRequest request);
    Task<IEnumerable<TenantBackup>> GetBackupsByTenantIdAsync(Guid tenantId);
    Task<TenantBackup?> GetBackupByIdAsync(Guid backupId);
    Task<bool> RestoreBackupAsync(Guid backupId, BackupServiceRestoreRequest? request = null);
}

public class BackupServiceCreateRequest
{
    public string BackupType { get; set; } = "Full";
    public string? BackupName { get; set; }
}

public class BackupServiceRestoreRequest
{
    public DateTime? RestorePoint { get; set; }
}

