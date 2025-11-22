using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Station.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Entities
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
