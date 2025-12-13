using FuelMaster.Website.Attributes;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Requests;
using FuelMaster.Website.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[RequireActiveTenantAndSubscription]
public class BackupController : ControllerBase
{
    private readonly IBackupService _backupService;
    private readonly ITenantService _tenantService;
    private readonly ILogger<BackupController> _logger;

    public BackupController(
        IBackupService backupService,
        ITenantService tenantService,
        ILogger<BackupController> logger)
    {
        _backupService = backupService;
        _tenantService = tenantService;
        _logger = logger;
    }

    [HttpPost("tenant/{tenantId}")]
    public async Task<IActionResult> CreateBackup(Guid tenantId, [FromBody] DTOs.Requests.CreateBackupRequest request)
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

            // Verify user owns the tenant
            var tenant = await _tenantService.GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var backupRequest = new Core.Interfaces.BackupServiceCreateRequest
            {
                BackupType = request.BackupType,
                BackupName = request.BackupName
            };

            var backup = await _backupService.CreateBackupAsync(tenantId, backupRequest);

            var response = new TenantBackupResponse
            {
                Id = backup.Id,
                TenantId = backup.TenantId,
                TenantName = tenant.Name,
                BackupName = backup.BackupName,
                BackupType = backup.BackupType.ToString(),
                Status = backup.Status.ToString(),
                BackupDate = backup.BackupDate,
                FileSize = backup.FileSize,
                BackupLocation = backup.BackupLocation,
                CreatedAt = backup.CreatedAt
            };

            _logger.LogInformation("Backup {BackupId} created for tenant {TenantName}", backup.Id, tenant.Name);

            return CreatedAtAction(nameof(GetBackup), new { id = backup.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backup for tenant {TenantId}", tenantId);
            return StatusCode(500, new { message = "An error occurred while creating the backup" });
        }
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<IActionResult> GetBackupsByTenant(Guid tenantId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // Verify user owns the tenant
            var tenant = await _tenantService.GetTenantByIdAsync(tenantId);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var backups = await _backupService.GetBackupsByTenantIdAsync(tenantId);

            var response = backups.Select(b => new TenantBackupResponse
            {
                Id = b.Id,
                TenantId = b.TenantId,
                TenantName = tenant.Name,
                BackupName = b.BackupName,
                BackupType = b.BackupType.ToString(),
                Status = b.Status.ToString(),
                BackupDate = b.BackupDate,
                FileSize = b.FileSize,
                BackupLocation = b.BackupLocation,
                CreatedAt = b.CreatedAt
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backups for tenant {TenantId}", tenantId);
            return StatusCode(500, new { message = "An error occurred while retrieving backups" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBackup(Guid id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var backup = await _backupService.GetBackupByIdAsync(id);
            if (backup == null)
            {
                return NotFound(new { message = "Backup not found" });
            }

            // Verify user owns the tenant
            var tenant = await _tenantService.GetTenantByIdAsync(backup.TenantId);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var response = new TenantBackupResponse
            {
                Id = backup.Id,
                TenantId = backup.TenantId,
                TenantName = tenant.Name,
                BackupName = backup.BackupName,
                BackupType = backup.BackupType.ToString(),
                Status = backup.Status.ToString(),
                BackupDate = backup.BackupDate,
                FileSize = backup.FileSize,
                BackupLocation = backup.BackupLocation,
                CreatedAt = backup.CreatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backup {BackupId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the backup" });
        }
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreBackup(Guid id, [FromBody] DTOs.Requests.RestoreBackupRequest? request = null)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var backup = await _backupService.GetBackupByIdAsync(id);
            if (backup == null)
            {
                return NotFound(new { message = "Backup not found" });
            }

            // Verify user owns the tenant
            var tenant = await _tenantService.GetTenantByIdAsync(backup.TenantId);
            if (tenant == null)
            {
                return NotFound(new { message = "Tenant not found" });
            }

            var isRootAdmin = User.IsInRole("RootAdmin");
            if (tenant.UserId != userId && !isRootAdmin)
            {
                return Forbid();
            }

            var restoreRequest = request != null
                ? new Core.Interfaces.BackupServiceRestoreRequest { RestorePoint = request.RestorePoint }
                : null;

            var result = await _backupService.RestoreBackupAsync(id, restoreRequest);
            if (!result)
            {
                return BadRequest(new { message = "Failed to restore backup" });
            }

            _logger.LogInformation("Backup {BackupId} restored for tenant {TenantName}", id, tenant.Name);

            return Ok(new { message = "Backup restored successfully" });
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
            _logger.LogError(ex, "Error restoring backup {BackupId}", id);
            return StatusCode(500, new { message = "An error occurred while restoring the backup" });
        }
    }
}

