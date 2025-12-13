
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface ICityService : 
    IDefaultBusinessQueryService<CityResult>, 
    IBusinessService<CityDto, CityResult>, 
    IScopedDependency
{
}