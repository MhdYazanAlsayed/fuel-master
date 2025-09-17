using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Dtos.Zones;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities.Zones
{
    public interface IZonePriceHistory : IScopedDependency
    {
        Task<ResultDto<ZonePriceHistory>> CreateAsync(CreateHistoryDto dto);
    }
}
