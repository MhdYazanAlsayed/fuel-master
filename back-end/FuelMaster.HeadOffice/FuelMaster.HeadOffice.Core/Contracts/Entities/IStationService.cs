using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Stations;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface IStationService : IScopedDependency
    {
        Task<IEnumerable<StationResponse>> GetAllAsync();
        Task<PaginationDto<StationResponse>> GetPaginationAsync(int currentPage);
        Task<ResultDto<StationResponse>> CreateAsync(StationRequest dto);
        Task<ResultDto<StationResponse>> EditAsync(int id, StationRequest dto);
        Task<StationResponse?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }
}
