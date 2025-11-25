using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IStationService : IBusinessService<CreateStationDto, EditStationDto, StationResult>, IScopedDependency
{
    Task<List<Station>> GetCachedStationsAsync();
}
