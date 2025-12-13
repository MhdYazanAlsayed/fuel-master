using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService;

public class StationService : IStationService
{
    private readonly IStationRepository _stationRepository;
    private readonly IMapper _mapper;
    private readonly ISigninService _signInService;
    private readonly ILogger<StationService> _logger;
    private readonly IEntityCache<Station> _stationCache;
    // private readonly ITankRepository _tankRepository;
    private readonly IUnitOfWork _unitOfWork;
    public StationService( 
        IMapper mapper, 
        ISigninService signInService,
        ILogger<StationService> logger,
        // ITankRepository tankRepository,
        IEntityCache<Station> stationCache,
        IStationRepository stationRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _signInService = signInService;
        _logger = logger;
        _stationCache = stationCache;
        // _tankRepository = tankRepository;
        _stationRepository = stationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultDto<StationResult>> CreateAsync(CreateStationDto dto)
    {
        try
        {
            var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);
            _stationRepository.Create(station);
            await _unitOfWork.SaveChangesAsync();

            var stationResponse = _mapper.Map<StationResult>(station);

            // Update caches incrementally - cache entity, not DTO
            await _stationCache.UpdateCacheAfterCreateAsync(station);

            return Result.Success(stationResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating station with English name: {EnglishName}, Arabic name: {ArabicName}", 
                dto.EnglishName, dto.ArabicName);
            return Result.Failure<StationResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var stations = await GetCachedStationsAsync();
            var station = stations.FirstOrDefault(x => x.Id == id);
            if (station == null)
            {
                return Result.Failure(Resource.EntityNotFound);
            }

            _stationRepository.Delete(station);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _stationCache.UpdateCacheAfterDeleteAsync(id);

            _logger.LogInformation("Successfully deleted station with ID: {Id}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting station with ID: {Id}", id);
            return Result.Failure(Resource.EntityNotFound);
        }    
    }

    public async Task<StationResult?> DetailsAsync(int id)
    {
        var stations = await GetCachedStationsAsync();
        var station = stations.FirstOrDefault(x => x.Id == id);
        if (station == null)
        {
            return null;
        }
        return _mapper.Map<StationResult?>(station);
    }

    public async Task<ResultDto<StationResult>> EditAsync(int id, EditStationDto dto)
    {
        try
        {
            var stations = await GetCachedStationsAsync();
            var station = stations.FirstOrDefault(x => x.Id == id);
            if (station == null)
            {
                return Result.Failure<StationResult>(Resource.EntityNotFound);
            }

            station.Update(dto.ArabicName, dto.EnglishName, dto.ZoneId);
            _stationRepository.Update(station);
            await _unitOfWork.SaveChangesAsync();

            var stationResponse = _mapper.Map<StationResult>(station);

            // Update caches incrementally
            await _stationCache.UpdateCacheAfterEditAsync(station);

            return Result.Success(stationResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing station with ID: {Id}", id);
            return Result.Failure<StationResult>(Resource.EntityNotFound);
        }
    }

    public async Task<IEnumerable<StationResult>> GetAllAsync()
    {
        var stations = await GetCachedStationsAsync();
        return _mapper.Map<List<StationResult>>(stations);
    }

    public async Task<List<Station>> GetCachedStationsAsync()
    {
        var cachedStationEntities = await _stationCache.GetAllEntitiesAsync();
        if (cachedStationEntities != null)
        {
            // Apply authorization filtering after getting from cache
            int? stationId = _signInService.GetCurrentStationIds().FirstOrDefault();
            if (stationId.HasValue)
            {
                var filtered = cachedStationEntities.Where(x => x.Id == stationId).ToList();
                _logger.LogInformation("Filtered to {Count} stations based on authorization", filtered.Count);
                return filtered;
            }
            
            return cachedStationEntities.ToList();
        }

        _logger.LogInformation("Stations not in cache, fetching from database");

        var stations = await _stationRepository.GetAllAsync(includeCity: true, includeZone: true);
        
        // Cache entities
        await _stationCache.SetAllAsync(stations);
        
        return stations;
    }

    public async Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage)
    {
        var stations = await GetCachedStationsAsync();
        var paginatedData = stations.ToPagination(currentPage);
        return _mapper.Map<PaginationDto<StationResult>>(paginatedData);
    }
}
