using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IStationService : 
    IDefaultBusinessQueryService<StationResult>, 
    IBusinessService<CreateStationDto, EditStationDto, StationResult>, 
    IScopedDependency
{
    Task<List<Station>> GetCachedStationsAsync();
}
