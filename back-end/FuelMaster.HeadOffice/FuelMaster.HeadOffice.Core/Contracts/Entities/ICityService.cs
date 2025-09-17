using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Cities;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface ICityService : IScopedDependency
    {
        Task<PaginationDto<City>> GetPaginationAsync(int currentPage);
        Task<IEnumerable<City>> GetAllAsync();
        Task<ResultDto<City>> CreateAsync(CityDto dto);
        Task<ResultDto<City>> EditAsync(int id , CityDto dto);
        Task<City?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }
}
