using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using AreaEntity = FuelMaster.HeadOffice.Core.Entities.Configs.Area.Area;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Area;

public interface IAreaService : 
    IDefaultBusinessQueryService<AreaResult>, 
    IBusinessService<AreaDto, AreaResult>, 
    IScopedDependency
{
    Task<List<AreaEntity>> GetCachedAreasAsync();
}

