using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Zones;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService;

public class ZonePriceHistoryService : IZonePriceHistoryService
{
    private readonly IZonePriceHistoryRepository _repository;
    private readonly IMapper _mapper;

    public ZonePriceHistoryService(
        IZonePriceHistoryRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginationDto<ZonePriceHistoryPaginationResult>> GetPaginationAsync(int zonePriceId, int currentPage)
    {
        var (entities, totalCount) = await _repository.GetPaginationAsync(zonePriceId, currentPage, Pagination.Length);
        var results = _mapper.Map<List<ZonePriceHistoryPaginationResult>>(entities);
        
        return new PaginationDto<ZonePriceHistoryPaginationResult>(results, totalCount);
    }
}