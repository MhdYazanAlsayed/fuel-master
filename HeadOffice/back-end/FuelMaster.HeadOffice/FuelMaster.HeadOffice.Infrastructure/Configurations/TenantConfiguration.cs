namespace FuelMaster.HeadOffice.Infrastructure.Configurations
{
    public class TenantConfiguration
    {
        public List<TenantItem> Tenants { get; set; } = null!;
    }

    public class TenantItem
    {
        public string TenantId { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
