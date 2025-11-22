using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface IStationRepository : IScopedDependency
    {
        Task<IEnumerable<StationResult>> GetAllAsync();
        Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage);
        Task<ResultDto<StationResult>> CreateAsync(CreateStationDto dto);
        Task<ResultDto<StationResult>> EditAsync(int id, EditStationDto dto);
        Task<StationResult?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }
}
