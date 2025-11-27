using System;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface IFuelTypeService : IDefaultBusinessQueryService<FuelTypeResult>, IBusinessService<FuelTypeDto, FuelTypeResult>
{
    Task<List<FuelType>> GetCachedFuelTypesAsync();
}
