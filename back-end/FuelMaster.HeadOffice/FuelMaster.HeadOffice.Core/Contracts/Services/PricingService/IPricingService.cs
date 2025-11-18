using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Contracts.Services;

public interface IPricingService : IScopedDependency
{
    Task<List<ZonePrice>> ChangePricesAsync(int zoneId, List<ChangePricesDto> dto);
}