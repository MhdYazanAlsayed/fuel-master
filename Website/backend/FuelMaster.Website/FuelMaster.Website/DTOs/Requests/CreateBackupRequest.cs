using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class CreateBackupRequest
{
    [Required]
    public string BackupType { get; set; } = "Full"; // "Full" or "Incremental"

    public string? BackupName { get; set; }
}

