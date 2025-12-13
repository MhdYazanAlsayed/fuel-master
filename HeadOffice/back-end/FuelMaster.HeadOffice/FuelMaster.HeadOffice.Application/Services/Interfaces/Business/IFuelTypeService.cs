using System;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface IFuelTypeService : 
    IDefaultBusinessQueryService<FuelTypeResult>, 
    IBusinessService<FuelTypeDto, FuelTypeResult>,
    IScopedDependency
{
    Task<List<FuelType>> GetCachedFuelTypesAsync();
}
