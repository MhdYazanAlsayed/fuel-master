
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface ICityService : IDefaultBusinessQueryService<CityResult>, IBusinessService<CityDto, CityResult>
{
}