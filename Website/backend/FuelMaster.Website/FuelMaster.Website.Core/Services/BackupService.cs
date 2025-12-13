using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FuelMaster.Website.Core.Services;

public class BackupService : IBackupService
{
    private readonly ITenantBackupRepository _backupRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IHeadofficeApiClient _headofficeApiClient;
    private readonly ILogger<BackupService> _logger;

    public BackupService(
        ITenantBackupRepository backupRepository,
        ITenantRepository tenantRepository,
        IHeadofficeApiClient headofficeApiClient,
        ILogger<BackupService> logger)
    {
        _backupRepository = backupRepository;
        _tenantRepository = tenantRepository;
        _headofficeApiClient = headofficeApiClient;
        _logger = logger;
    }

    public async Task<TenantBackup> CreateBackupAsync(Guid tenantId, BackupServiceCreateRequest request)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant == null)
        {
            throw new ArgumentException("Tenant not found", nameof(tenantId));
        }

        // Create backup record
        var backupName = request.BackupName ?? $"backup_{tenant.Name}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
        var backup = new TenantBackup
        {
            TenantId = tenantId,
            BackupName = backupName,
            BackupType = Enum.TryParse<BackupType>(request.BackupType, out var type) ? type : BackupType.Full,
            Status = BackupStatus.InProgress,
            BackupDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        backup = await _backupRepository.AddAsync(backup);

        try
        {
            // Call Headoffice API to create backup
            var headofficeRequest = new Core.Interfaces.CreateBackupRequest
            {
                BackupType = request.BackupType,
                BackupName = backupName
            };

            var headofficeResponse = await _headofficeApiClient.CreateBackupAsync(tenant.Name, headofficeRequest);

            if (headofficeResponse.Success)
            {
                backup.Status = BackupStatus.Completed;
                backup.BackupLocation = headofficeResponse.BackupLocation;
                if (headofficeResponse.BackupId.HasValue)
                {
                    // Store backup ID if provided by Headoffice
                }
            }
            else
            {
                backup.Status = BackupStatus.Failed;
                _logger.LogError("Failed to create backup for tenant {TenantName}: {Error}",
                    tenant.Name, headofficeResponse.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            backup.Status = BackupStatus.Failed;
            _logger.LogError(ex, "Error creating backup for tenant {TenantName}", tenant.Name);
        }

        await _backupRepository.UpdateAsync(backup);

        return backup;
    }

    public async Task<IEnumerable<TenantBackup>> GetBackupsByTenantIdAsync(Guid tenantId)
    {
        return await _backupRepository.GetBackupsByTenantIdAsync(tenantId);
    }

    public async Task<TenantBackup?> GetBackupByIdAsync(Guid backupId)
    {
        return await _backupRepository.GetByIdAsync(backupId);
    }

    public async Task<bool> RestoreBackupAsync(Guid backupId, BackupServiceRestoreRequest? request = null)
    {
        var backup = await _backupRepository.GetByIdAsync(backupId);
        if (backup == null)
        {
            throw new ArgumentException("Backup not found", nameof(backupId));
        }

        if (backup.Status != BackupStatus.Completed)
        {
            throw new InvalidOperationException("Cannot restore a backup that is not completed");
        }

        var tenant = await _tenantRepository.GetByIdAsync(backup.TenantId);
        if (tenant == null)
        {
            throw new ArgumentException("Tenant not found");
        }

        try
        {
            var restoreRequest = new Core.Interfaces.RestoreBackupRequest
            {
                BackupId = backup.Id,
                RestorePoint = request?.RestorePoint
            };

            var restoreResponse = await _headofficeApiClient.RestoreBackupAsync(tenant.Name, restoreRequest);

            if (restoreResponse.Success)
            {
                _logger.LogInformation("Successfully restored backup {BackupId} for tenant {TenantName}",
                    backupId, tenant.Name);
                return true;
            }
            else
            {
                _logger.LogError("Failed to restore backup {BackupId} for tenant {TenantName}: {Error}",
                    backupId, tenant.Name, restoreResponse.ErrorMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring backup {BackupId} for tenant {TenantName}",
                backupId, tenant.Name);
            return false;
        }
    }
}

