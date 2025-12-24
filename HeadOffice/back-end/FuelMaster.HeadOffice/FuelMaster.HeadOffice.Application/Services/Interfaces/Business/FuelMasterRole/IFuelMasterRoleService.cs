using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IFuelMasterRoleService :
    IDefaultBusinessQueryService<FuelMasterRoleResult>,
    IBusinessService<FuelMasterRoleDto, FuelMasterRoleResult>,
    IScopedDependency
{
}

