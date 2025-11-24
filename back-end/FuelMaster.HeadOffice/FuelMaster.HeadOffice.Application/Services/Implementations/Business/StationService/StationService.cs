using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService;

public class StationService : IStationService
{
    public Task<ResultDto<StationResult>> CreateAsync(CreateStationDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<StationResult?> DetailsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDto<StationResult>> EditAsync(int id, EditStationDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StationResult>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage)
    {
        throw new NotImplementedException();
    }
}
