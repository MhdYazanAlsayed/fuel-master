using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Results;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface ICityRepository : IScopedDependency
    {
        Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage);
        Task<IEnumerable<CityResult>> GetAllAsync();
        Task<ResultDto<CityResult>> CreateAsync(CityDto dto);
        Task<ResultDto<CityResult>> EditAsync(int id , CityDto dto);
        Task<CityResult?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }
}
