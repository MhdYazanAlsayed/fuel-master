using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface ITankService : 
    IBusinessService<TankDto, EditTankDto, TankResult>,
    IScopedDependency
{
    Task<List<Tank>> GetCachedTanksAsync();
    Task<IEnumerable<TankResult>> GetAllAsync(GetTankDto dto);
    Task<PaginationDto<TankResult>> GetPaginationAsync(int currentPage, GetTankDto dto);
}
