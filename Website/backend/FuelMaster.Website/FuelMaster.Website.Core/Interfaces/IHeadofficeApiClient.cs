namespace FuelMaster.Website.Core.Interfaces;

public interface IHeadofficeApiClient
{
    Task<CreateTenantDatabaseResponse> CreateTenantDatabaseAsync(CreateTenantDatabaseRequest request);
    Task<TenantInfoResponse?> GetTenantInfoAsync(string tenantName);
    Task<BackupResponse> CreateBackupAsync(string tenantName, CreateBackupRequest request);
    Task<RestoreResponse> RestoreBackupAsync(string tenantName, RestoreBackupRequest request);
}

public class CreateTenantDatabaseRequest
{
    public string TenantName { get; set; } = string.Empty;
}

public class CreateTenantDatabaseResponse
{
    public bool Success { get; set; }
    public string? DatabaseName { get; set; }
    public string? ErrorMessage { get; set; }
}

public class TenantInfoResponse
{
    public string TenantName { get; set; } = string.Empty;
    public string? ConnectionString { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateBackupRequest
{
    public string BackupType { get; set; } = "Full";
    public string BackupName { get; set; } = string.Empty;
}

public class BackupResponse
{
    public bool Success { get; set; }
    public Guid? BackupId { get; set; }
    public string? BackupLocation { get; set; }
    public string? ErrorMessage { get; set; }
}

public class RestoreBackupRequest
{
    public Guid? BackupId { get; set; }
    public DateTime? RestorePoint { get; set; }
}

public class RestoreResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

