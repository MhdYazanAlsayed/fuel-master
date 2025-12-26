using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService;

public class StationService : IStationService
{
    private readonly IStationRepository _stationRepository;
    private readonly IMapper _mapper;
    private readonly ISigninService _signInService;
    private readonly ILogger<StationService> _logger;
    private readonly IEntityCache<Station> _stationCache;
    private readonly INozzleRepository _nozzleRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly IUnitOfWork _unitOfWork;
    public StationService(
        IMapper mapper,
        ISigninService signInService,
        ILogger<StationService> logger,
        // ITankRepository tankRepository,
        INozzleRepository nozzleRepository,
        IZoneRepository zoneRepository,
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
        _nozzleRepository = nozzleRepository;
        _zoneRepository = zoneRepository;
    }

    public async Task<ResultDto<StationResult>> CreateAsync(CreateStationDto dto)
    {
        try
        {
            var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId, dto.AreaId);
            _stationRepository.Create(station);
            await _unitOfWork.SaveChangesAsync();

            // Get the station with relations 
            station = await _stationRepository.DetailsAsync(station.Id, includeCity: true, includeZone: true, includeArea: true);

            // Update caches incrementally - cache entity, not DTO
            await _stationCache.UpdateCacheAfterCreateAsync(station!);

            var stationResponse = _mapper.Map<StationResult>(station);
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

    public async Task<ResultDto<StationResult>> UpdateAsync(int id, EditStationDto dto)
    {
        var station = await _stationRepository.DetailsAsync(id);
        if (station == null)
        {
            return Result.Failure<StationResult>(Resource.EntityNotFound);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Update station information
            station.Update(dto.EnglishName, dto.ArabicName, dto.ZoneId, dto.AreaId);
            _stationRepository.Update(station);
            await _unitOfWork.SaveChangesAsync();

            var zone = await _zoneRepository.DetailsAsync(dto.ZoneId, includePrices: true);
            if (zone == null)
            {
                return Result.Failure<StationResult>(Resource.EntityNotFound);
            }

            // Update all nozzle's prices
            var nozzles = await _nozzleRepository.GetAllByStationIdAsync(id);
            foreach (var nozzle in nozzles)
            {
                var price = zone.Prices.FirstOrDefault(x => x.FuelTypeId == nozzle.FuelTypeId);
                if (price == null)
                {
                    // If the price of this fueltype doesn't exist in new zone, 
                    // set the price to 0
                    nozzle.ChangePrice(0);
                    continue;
                }

                // Update the price of the nozzle
                nozzle.ChangePrice(price.Price);
                _nozzleRepository.Update(nozzle);
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Get station with new relations 
            station = await _stationRepository.DetailsAsync(id, includeCity: true, includeZone: true, includeArea: true);

            // Update caches incrementally
            await _stationCache.UpdateCacheAfterEditAsync(station!);
            var stationResponse = _mapper.Map<StationResult>(station);

            return Result.Success(stationResponse);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
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
            return Filter(cachedStationEntities.ToList());
        }

        _logger.LogInformation("Stations not in cache, fetching from database");

        var stations = await _stationRepository.GetAllAsync(includeCity: true, includeZone: true, includeArea: true);

        // Cache entities
        await _stationCache.SetAllAsync(stations);

        return Filter(stations);
    }

    public async Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage)
    {
        var stations = await GetCachedStationsAsync();
        var paginatedData = stations.ToPagination(currentPage);
        var mappedData = _mapper.Map<List<StationResult>>(paginatedData.Data);

        return new PaginationDto<StationResult>(mappedData, paginatedData.Pages);
    }

    private List<Station> Filter (List<Station> stations)
    {
        var scope = _signInService.GetCurrentScope();
        var cityId = _signInService.GetCurrentCityId(); 
        var areaId = _signInService.GetCurrentAreaId();
        var stationId = _signInService.GetCurrentStationId();

        if (scope == Scope.ALL)
            return stations;

        if (scope == Scope.City)
            return stations.Where(x => x.CityId == cityId).ToList();

        if (scope == Scope.Area)
            return stations.Where(x => x.AreaId == areaId).ToList();

        if (scope == Scope.Station || scope == Scope.Self)
            return stations.Where(x => x.Id == stationId).ToList();

        throw new NotImplementedException();
    }
}
