using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;

namespace FuelMaster.HeadOffice.Services
{
    public class TanentService : ITanentService
    {
        public string TenantId { get; internal set; } = string.Empty;
    }
}