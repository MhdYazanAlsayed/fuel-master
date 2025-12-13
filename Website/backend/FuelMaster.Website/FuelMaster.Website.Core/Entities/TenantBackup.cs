namespace FuelMaster.Website.Core.Entities;

public class TenantBackup
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string BackupName { get; set; } = string.Empty;
    public Enums.BackupType BackupType { get; set; } = Enums.BackupType.Full;
    public Enums.BackupStatus Status { get; set; } = Enums.BackupStatus.InProgress;
    public DateTime BackupDate { get; set; } = DateTime.UtcNow;
    public long? FileSize { get; set; }
    public string? BackupLocation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
}

