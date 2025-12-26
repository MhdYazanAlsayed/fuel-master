
using FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface ICityService : 
    IDefaultBusinessQueryService<CityResult>, 
    IBusinessService<CityDto, CityResult>, 
    IScopedDependency
{
}