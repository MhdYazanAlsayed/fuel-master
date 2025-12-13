namespace FuelMaster.HeadOffice.Core.Helpers;

public class TenantConfig
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public bool IsActive { get; set; }

}