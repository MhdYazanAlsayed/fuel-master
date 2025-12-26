using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface INozzleService : 
    IBusinessService<NozzleDto, UpdateNozzleDto , NozzleResult>,
    IScopedDependency
{
    Task<List<Nozzle>> GetCachedNozzlesAsync();
    Task<IEnumerable<NozzleResult>> GetAllAsync(GetNozzleDto dto);
    Task<PaginationDto<NozzleResult>> GetPaginationAsync(int currentPage, GetNozzleDto dto);
}

