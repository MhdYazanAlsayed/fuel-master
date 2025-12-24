using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IPumpService : 
    IBusinessService<PumpDto, UpdatePumpDto, PumpResult>,
    IScopedDependency
{
    Task<List<Pump>> GetCachedPumpsAsync();
    Task<IEnumerable<PumpResult>> GetAllAsync(GetPumpDto dto);
    Task<PaginationDto<PumpResult>> GetPaginationAsync(int currentPage, GetPumpDto dto);
}

