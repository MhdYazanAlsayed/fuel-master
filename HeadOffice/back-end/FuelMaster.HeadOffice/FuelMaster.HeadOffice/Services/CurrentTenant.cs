using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;

namespace FuelMaster.HeadOffice.Services
{
    public class CurrentTenant : ICurrentTenant
    {
        public Guid TenantId { get; internal set; }  
        public string ConnectionString { get; internal set; } = string.Empty;
    }
}