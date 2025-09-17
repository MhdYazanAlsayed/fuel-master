using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Zones;
using FuelMaster.HeadOffice.Core.Models.Responses.ZonePriceHistories;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities.Zones
{
    public interface IZoneService : IScopedDependency
    {
        Task<IEnumerable<Zone>> GetAllAsync();
        Task<PaginationDto<Zone>> GetPaginationAsync(int currentPage);
        Task<ResultDto<Zone>> CreateAsync(ZoneDto dto);
        Task<ResultDto<Zone>> EditAsync(int id, ZoneDto dto);
        Task<Zone?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
        Task<ResultDto> ChangePriceAsync(int zonePriceId, ChangePriceDto dto);
        Task<IEnumerable<ZonePrice>> GetPricesAsync(int zoneId);
        Task<PaginationDto<ZonePriceHistoryPaginationResponse>> GetHistoriesAsync(int currentPage, int zonePrice);
    }
}
