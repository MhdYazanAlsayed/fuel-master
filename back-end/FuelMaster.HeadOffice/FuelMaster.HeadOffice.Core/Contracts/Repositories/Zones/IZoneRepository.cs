using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities.Zones
{
    public interface IZoneRepository : IScopedDependency
    {
        Task<IEnumerable<Zone>> GetCachedZonesAsync ();
        Task<IEnumerable<ZoneResult>> GetAllAsync();
        Task<PaginationDto<ZoneResult>> GetPaginationAsync(int currentPage);
        Task<ResultDto<ZoneResult>> CreateAsync(ZoneDto dto);
        Task<ResultDto<ZoneResult>> EditAsync(int id, ZoneDto dto);
        Task<ZoneResult?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
        Task<ResultDto> ChangePriceAsync(int zonePriceId, ChangePriceDto dto);
        Task<IEnumerable<ZonePrice>> GetPricesAsync(int zoneId);
        Task<PaginationDto<ZonePriceHistoryPaginationResult>> GetHistoriesAsync(int currentPage, int zonePrice);
    }
}
