using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Caching;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.Stations.Exceptions;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class StationRepository : IStationRepository
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISigninService _authorization;
        private readonly ILogger<StationRepository> _logger;
        private readonly ICacheService _cacheService;
        private readonly IEntityCache<Station> _stationCache;
        private readonly ITankRepository _tankRepository;

        public StationRepository(IContextFactory<FuelMasterDbContext> contextFactory, 
            IMapper mapper, 
            ISigninService authorization,
            ILogger<StationRepository> logger,
            ICacheService cacheService,
            ITankRepository tankRepository,
            IEntityCache<Station> stationCache)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _cacheService = cacheService;
            _stationCache = stationCache;
            _tankRepository = tankRepository;
        }

        public async Task<IEnumerable<StationResult>> GetAllAsync()
        {
            _logger.LogInformation("Getting all stations");

            var cachedStationEntities = await _stationCache.GetAllEntitiesAsync();
            if (cachedStationEntities != null)
            {
                _logger.LogInformation("Retrieved {Count} stations from cache", cachedStationEntities.Count());
                
                // Map entities to DTOs
                var mappedStations = _mapper.Map<List<StationResult>>(cachedStationEntities);
                
                // Apply authorization filtering after getting from cache
                var stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
                if (stationId.HasValue)
                {
                    var filtered = mappedStations.Where(x => x.Id == stationId).ToList();
                    _logger.LogInformation("Filtered to {Count} stations based on authorization", filtered.Count);
                    return filtered;
                }
                
                return mappedStations;
            }

            _logger.LogInformation("Stations not in cache, fetching from database");

            var stations = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .AsNoTracking()
                .ToListAsync();
            
            // Cache entities
            await _stationCache.SetAllAsync(stations);
            
            _logger.LogInformation("Cached {Count} stations", stations.Count);

            // Map entities to DTOs
            var mappedStationsFromDb = _mapper.Map<List<StationResult>>(stations);

            // Apply authorization filtering
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            if (authStationId.HasValue)
            {
                var filtered = mappedStationsFromDb.Where(x => x.Id == authStationId).ToList();
                _logger.LogInformation("Filtered to {Count} stations based on authorization", filtered.Count);
                return filtered;
            }

            return mappedStationsFromDb;
        }

        public async Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated stations for page {Page}", currentPage);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allStations = await GetAllAsync();
            var tanks = await _tankRepository.GetCachedTanksAsync();

            allStations = allStations.Select(x => 
            {
                x.CanDelete = tanks == null || !tanks.Any(t => t.StationId == x.Id);

                return x;
            });

            var pagination = allStations.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated stations for page {Page}", currentPage);

            return pagination;
        }

        public async Task<ResultDto<StationResult>> CreateAsync(CreateStationDto dto)
        {
            _logger.LogInformation("Creating new station with English name: {EnglishName}, Arabic name: {ArabicName}, city: {CityId}, zone: {ZoneId}", 
                dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);

            try
            {
                var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);
                await _context.Stations.AddAsync(station);
                await _context.SaveChangesAsync();

                // Load related entities for mapping and caching
                await _context.Entry(station)
                    .Reference(x => x.City)
                    .LoadAsync();
                await _context.Entry(station)
                    .Reference(x => x.Zone)
                    .LoadAsync();

                var stationResponse = _mapper.Map<StationResult>(station);

                // Update caches incrementally - cache entity, not DTO
                await _stationCache.UpdateCacheAfterCreateAsync(station);

                _logger.LogInformation("Successfully created station with ID: {Id}", station.Id);

                return Results.Success(stationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating station with English name: {EnglishName}, Arabic name: {ArabicName}", 
                    dto.EnglishName, dto.ArabicName);
                return Results.Failure<StationResult>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<StationResult>> EditAsync(int id, EditStationDto dto)
        {
            _logger.LogInformation("Editing station with ID: {Id}, English name: {EnglishName}, Arabic name: {ArabicName}, zone: {ZoneId}", 
                id, dto.EnglishName, dto.ArabicName, dto.ZoneId);
            var station =
                await _context.Stations
                .Include(x => x.Tanks)
                .ThenInclude(x => x.Nozzles)
                .SingleOrDefaultAsync(x => x.Id == id);


            if (station == null)
            {
                _logger.LogWarning("Station with ID {Id} not found", id);
                return Results.Failure<StationResult>(Resource.EntityNotFound);
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _logger.LogInformation("Updating nozzle prices for station {Id} with zone {ZoneId}", id, dto.ZoneId);

                var isZoneChanged = dto.ZoneId != station.ZoneId;
                if (isZoneChanged)
                {
                    var zonePrices = await GetZonePricesAsync(dto.ZoneId);
                    if (zonePrices is null || zonePrices.Count == 0)
                    {
                        return Results.Failure<StationResult>(Resource.AssignStationToInitialZoneException);
                    }

                    // Update a nozzles prices in the station
                    UpdateNozzlesPrices(station, zonePrices);
                }

                station.Update(dto.EnglishName, dto.ArabicName, dto.ZoneId);
                _context.Stations.Update(station);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Load related entities for mapping and caching
                await UpdateStationIncludesAsync(station);

                var stationResponse = _mapper.Map<StationResult>(station);

                // Update caches incrementally - cache entity, not DTO
                await _stationCache.UpdateCacheAfterEditAsync(station);
                await _cacheService.SetAsync($"Station_Details_{station.Id}", stationResponse);

                _logger.LogInformation("Successfully updated station with ID: {Id}", id);

                return Results.Success(stationResponse);
            }
            catch(InvalidAssignStationException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "AssignStationToInitialZoneException for station {Id} with zone {ZoneId}", id, dto.ZoneId);
                return Results.Failure<StationResult>(Resource.AssignStationToInitialZoneException);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error editing station with ID: {Id}", id);
                return Results.Failure<StationResult>(Resource.SthWentWrong);
            }
        }

        public async Task<StationResult?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for station with ID: {Id}", id);

            var cacheKey = $"Station_Details_{id}";
            var cachedStation = await _cacheService.GetAsync<StationResult>(cacheKey);
            
            if (cachedStation != null)
            {
                _logger.LogInformation("Retrieved station details from cache for ID: {Id}", id);
                return cachedStation;
            }

            _logger.LogInformation("Station details not in cache, fetching from database for ID: {Id}", id);

            var result = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .SingleOrDefaultAsync(x => x.Id == id);

            var mappedResult = _mapper.Map<StationResult>(result);
            
            if (mappedResult != null)
            {
                await _cacheService.SetAsync(cacheKey, mappedResult);
                _logger.LogInformation("Cached station details for ID: {Id}", id);
            }

            return mappedResult;
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting station with ID: {Id}", id);

            try
            {
                var station = await _context.Stations.FindAsync(id);
                if (station == null)
                {
                    _logger.LogWarning("Station with ID {Id} not found for deletion", id);
                    return Results.Failure(Resource.EntityNotFound);
                }

                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();

                // Update caches incrementally
                await _stationCache.UpdateCacheAfterDeleteAsync(id);
                await _cacheService.RemoveAsync($"Station_Details_{id}");

                _logger.LogInformation("Successfully deleted station with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting station with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }

    
        private async Task<IReadOnlyList<ZonePrice>?> GetZonePricesAsync(int zoneId)
        {
            var zone = await _context.Zones
            .Where(x => x.Id == zoneId)
            .Include(x => x.Prices)
            .SingleOrDefaultAsync();

            if (zone is null || zone.Prices.Count == 0)
            {
                return null;
            }

            return zone.Prices;
        }
    
        private void UpdateNozzlesPrices(Station station, IReadOnlyList<ZonePrice> zonePrices)
        {
            foreach (var tank in station.Tanks)
            {
                foreach (var nozzle in tank.Nozzles)
                {
                    var zonePrice = zonePrices.SingleOrDefault(x => x.FuelTypeId == nozzle.FuelTypeId);
                    if (zonePrice is null)
                    {
                        throw new ZonePriceNotFoundException($"Zone price for fuel type {nozzle.FuelTypeId} not found");
                    }

                    nozzle.ChangePrice(zonePrice.Price);
                    _context.Update(nozzle);
                }
            }
        }
    
        private async Task UpdateStationIncludesAsync (Station station)
        {
            await _context.Entry(station)
                        .Reference(x => x.Zone)
                        .LoadAsync();

            await _context.Entry(station)
                    .Reference(x => x.City)
                    .LoadAsync();
        }
    }
}
