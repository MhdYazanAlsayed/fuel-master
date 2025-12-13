namespace FuelMaster.Website.DTOs.Responses;

public class TenantBackupResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string BackupName { get; set; } = string.Empty;
    public string BackupType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime BackupDate { get; set; }
    public long? FileSize { get; set; }
    public string? BackupLocation { get; set; }
    public DateTime CreatedAt { get; set; }
}

