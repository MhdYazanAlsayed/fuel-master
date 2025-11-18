using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.Stations.Exceptions;
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

        public StationRepository(IContextFactory<FuelMasterDbContext> contextFactory, 
            IMapper mapper, 
            ISigninService authorization,
            ILogger<StationRepository> logger,
            ICacheService cacheService)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<StationResult>> GetAllAsync()
        {
            _logger.LogInformation("Getting all stations");

            var stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var result = await _context.Stations
            .Where(x => !stationId.HasValue || x.Id == stationId)
            .ToListAsync();
            
            var mappedResult = _mapper.Map<List<StationResult>>(result);
            
            _logger.LogInformation("Cached {Count} stations", mappedResult.Count);

            return mappedResult;
        }

        public async Task<PaginationDto<StationResult>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated stations for page {Page}", currentPage);

            int? stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            
            var data = await _context.Stations
                .Include(x => x.City)
                .Include(x => x.Zone)
                .Where(x => !stationId.HasValue || x.Id == stationId)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<StationResult>>(data.Data);
            var result = new PaginationDto<StationResult>(mappedData, data.Pages);
            
            return result;
        }

        public async Task<ResultDto<StationResult>> CreateAsync(StationDto dto)
        {
            _logger.LogInformation("Creating new station with English name: {EnglishName}, Arabic name: {ArabicName}, city: {CityId}, zone: {ZoneId}", 
                dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);

            try
            {
                var station = new Station(dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);
                await _context.Stations.AddAsync(station);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created station with ID: {Id}", station.Id);

                var stationResponse = _mapper.Map<StationResult>(station);
                return Results.Success(stationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating station with English name: {EnglishName}, Arabic name: {ArabicName}", 
                    dto.EnglishName, dto.ArabicName);
                return Results.Failure<StationResult>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<StationResult>> EditAsync(int id, StationDto dto)
        {
            _logger.LogInformation("Editing station with ID: {Id}, English name: {EnglishName}, Arabic name: {ArabicName}, city: {CityId}, zone: {ZoneId}", 
                id, dto.EnglishName, dto.ArabicName, dto.CityId, dto.ZoneId);

            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                _logger.LogWarning("Station with ID {Id} not found", id);
                return Results.Failure<StationResult>(Resource.EntityNotFound);
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _logger.LogInformation("Updating nozzle prices for station {Id} with zone {ZoneId}", id, dto.ZoneId);
                

                var zone = await _context.Zones
                .Where(x => x.Id == dto.ZoneId)
                .Select(x => new 
                {
                    Prices = x.Prices.Count()
                })
                .SingleOrDefaultAsync();
                if (zone is null || zone.Prices == 0)
                {
                    throw new InvalidAssignStationException(Resource.AssignStationToInitialZoneException);
                }

                station.Update(dto.EnglishName, dto.ArabicName, dto.ZoneId);
                _context.Stations.Update(station);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully updated station with ID: {Id}", id);

                var stationResponse = _mapper.Map<StationResult>(station);
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

                _logger.LogInformation("Successfully deleted station with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting station with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }
    }
}
