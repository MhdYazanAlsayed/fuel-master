namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.External.Responses;

public class TenantConfigResponse
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public bool IsActive { get; set; }
}