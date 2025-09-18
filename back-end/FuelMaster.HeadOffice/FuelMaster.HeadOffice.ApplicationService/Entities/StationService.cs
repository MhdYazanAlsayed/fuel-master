using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Exceptions.Stations;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Stations;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class StationService : IStationService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        private readonly ILogger<StationService> _logger;
        private readonly ICacheService _cacheService;

        public StationService(IContextFactory<FuelMasterDbContext> contextFactory, 
            IMapper mapper, 
            IAuthorization authorization,
            ILogger<StationService> logger,
            ICacheService cacheService)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<StationResponse>> GetAllAsync()
        {
            _logger.LogInformation("Getting all stations");

            int? stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var cacheKey = $"Stations_All_{stationId}";
            var cachedStations = await _cacheService.GetAsync<IEnumerable<StationResponse>>(cacheKey);
            
            if (cachedStations != null)
            {
                _logger.LogInformation("Retrieved {Count} stations from cache", cachedStations.Count());
                return cachedStations;
            }

            _logger.LogInformation("Stations not in cache, fetching from database");
            
            var result = await _context.Stations
            .Where(x => !stationId.HasValue || x.Id == stationId)
            .ToListAsync();
            
            var mappedResult = _mapper.Map<List<StationResponse>>(result);
            
            await _cacheService.SetAsync(cacheKey, mappedResult);
            
            _logger.LogInformation("Cached {Count} stations", mappedResult.Count);

            return mappedResult;
        }

        public async Task<PaginationDto<StationResponse>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated stations for page {Page}", currentPage);

            int? stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var cacheKey = $"Stations_Page_{currentPage}_{stationId}";
            var cachedPagination = await _cacheService.GetAsync<PaginationDto<StationResponse>>(cacheKey);
            
            if (cachedPagination != null)
            {
                _logger.LogInformation("Retrieved paginated stations from cache for page {Page}", currentPage);
                return cachedPagination;
            }

            _logger.LogInformation("Paginated stations not in cache, fetching from database for page {Page}", currentPage);
            
            var data = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .Where(x => !stationId.HasValue || x.Id == stationId)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<StationResponse>>(data.Data);
            var result = new PaginationDto<StationResponse>(mappedData, data.Pages);
            
            await _cacheService.SetAsync(cacheKey, result);
            
            _logger.LogInformation("Cached paginated stations for page {Page}", currentPage);

            return result;
        }

        public async Task<ResultDto<StationResponse>> CreateAsync(StationRequest dto)
        {
            _logger.LogInformation("Creating new station with English name: {EnglishName}, Arabic name: {ArabicName}, city: {CityId}, zone: {ZoneId}", 
                dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);

            try
            {
                var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);
                await _context.Stations.AddAsync(station);
                await _context.SaveChangesAsync();

                // Invalidate cache after creating station
                await InvalidateStationsCacheAsync();
                
                _logger.LogInformation("Successfully created station with ID: {Id}", station.Id);

                var stationResponse = _mapper.Map<StationResponse>(station);
                return Results.Success(stationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating station with English name: {EnglishName}, Arabic name: {ArabicName}", 
                    dto.EnglishName, dto.ArabicName);
                return Results.Failure<StationResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<StationResponse>> EditAsync(int id, StationRequest dto)
        {
            _logger.LogInformation("Editing station with ID: {Id}, English name: {EnglishName}, Arabic name: {ArabicName}, city: {CityId}, zone: {ZoneId}", 
                id, dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);

            var station = await _context.Stations.FindAsync(id);

            if (station == null)
            {
                _logger.LogWarning("Station with ID {Id} not found", id);
                return Results.Failure<StationResponse>(Resource.EntityNotFound);
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _logger.LogInformation("Updating nozzle prices for station {Id} with zone {ZoneId}", id, dto.ZoneId);
                
                // Update nozzles prices
                var zone = await _context.Zones
                .Include(x => x.Prices)
                .SingleAsync(x => x.Id == dto.ZoneId);

                var nozzles = await _context.Nozzles
                .Include(x => x.Tank)
                .Where(x => x.Tank!.StationId == id)
                .ToListAsync();

                _logger.LogInformation("Found {Count} nozzles to update for station {Id}", nozzles.Count, id);

                foreach (var nozzle in nozzles)
                {
                    var zonePrice = zone.Prices!.SingleOrDefault(x => x.FuelType == nozzle.Tank!.FuelType);
                    if (zonePrice == null || zonePrice.Price == 0)
                    {
                        _logger.LogError("Zone price not found for fuel type {FuelType} in zone {ZoneId}", 
                            nozzle.Tank!.FuelType, dto.ZoneId);
                        throw new AssignStationToInitialZoneException(Resource.AssignStationToInitialZoneException);
                    }

                    nozzle.Price = zonePrice.Price;
                    _context.Nozzles.Update(nozzle);
                }

                station.EnglishName = dto.EnglishName;
                station.ArabicName = dto.ArabicName;
                station.CityId = dto.CityId;
                station.ZoneId = dto.ZoneId;

                _context.Stations.Update(station);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Invalidate cache after editing station
                await InvalidateStationsCacheAsync();
                
                _logger.LogInformation("Successfully updated station with ID: {Id}", id);

                var stationResponse = _mapper.Map<StationResponse>(station);
                return Results.Success(stationResponse);
            }
            catch(AssignStationToInitialZoneException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "AssignStationToInitialZoneException for station {Id} with zone {ZoneId}", id, dto.ZoneId);
                return Results.Failure<StationResponse>(Resource.AssignStationToInitialZoneException);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error editing station with ID: {Id}", id);
                return Results.Failure<StationResponse>(Resource.SthWentWrong);
            }
        }

        public async Task<StationResponse?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for station with ID: {Id}", id);

            var cacheKey = $"Station_Details_{id}";
            var cachedStation = await _cacheService.GetAsync<StationResponse>(cacheKey);
            
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

            var mappedResult = _mapper.Map<StationResponse>(result);
            
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

                // Invalidate cache after deleting station
                await InvalidateStationsCacheAsync();
                
                _logger.LogInformation("Successfully deleted station with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting station with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }

        private async Task InvalidateStationsCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync("Stations");
            _logger.LogInformation("Invalidated all stations cache entries");
        }
    }
}
