
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces;

public interface ICityService : IScopedDependency
{
    Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage);
    Task<IEnumerable<CityResult>> GetAllAsync();
    Task<ResultDto<CityResult>> CreateAsync(CityDto dto);
    Task<ResultDto<CityResult>> EditAsync(int id, CityDto dto);
    Task<ResultDto> DeleteAsync(int id);
    Task<CityResult?> DetailsAsync(int id);
}