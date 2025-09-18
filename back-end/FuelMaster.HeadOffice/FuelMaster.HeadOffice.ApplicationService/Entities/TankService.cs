using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Tanks;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class TankService : ITankService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorization _authorization;
        private readonly ILogger<TankService> _logger;
        private readonly ICacheService _cacheService;

        public TankService(IContextFactory<FuelMasterDbContext> contextFactory, 
            IMapper mapper, 
            IAuthorization authorization,
            ILogger<TankService> logger,
            ICacheService cacheService)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<TankResponse>> GetAllAsync(GetTankRequest dto)
        {
            _logger.LogInformation("Getting all tanks for station: {StationId}", dto.StationId);

            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var cacheKey = $"Tanks_Station_{dto.StationId}";
            var cachedTanks = await _cacheService.GetAsync<IEnumerable<TankResponse>>(cacheKey);
            
            if (cachedTanks != null)
            {
                _logger.LogInformation("Retrieved {Count} tanks from cache for station: {StationId}", 
                    cachedTanks.Count(), dto.StationId);
                return cachedTanks;
            }

            _logger.LogInformation("Tanks not in cache, fetching from database for station: {StationId}", dto.StationId);
            
            var result = await _context.Tanks
                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
                .ToListAsync();

            var mappedResult = _mapper.Map<IEnumerable<TankResponse>>(result);
            
            await _cacheService.SetAsync(cacheKey, mappedResult);
            
            _logger.LogInformation("Cached {Count} tanks for station: {StationId}", mappedResult.Count(), dto.StationId);

            return mappedResult;
        }

        public async Task<ResultDto<TankResponse>> CreateAsync(TankRequest dto)
        {
            _logger.LogInformation("Creating new tank with number: {Number}, fuel type: {FuelType}, station: {StationId}, capacity: {Capacity}", 
                dto.Number, dto.FuelType, dto.StationId, dto.Capacity);

            try
            {
                // Validate tank parameters
                if (dto.Capacity <= 0 || dto.MaxLimit <= 0 || dto.MinLimit <= 0 || dto.CurrentLevel <= 0 || dto.CurrentVolume <= 0)
                {
                    _logger.LogWarning("Invalid tank parameters: capacity={Capacity}, maxLimit={MaxLimit}, minLimit={MinLimit}, currentLevel={CurrentLevel}, currentVolume={CurrentVolume}", 
                        dto.Capacity, dto.MaxLimit, dto.MinLimit, dto.CurrentLevel, dto.CurrentVolume);
                    throw new Exception("Invalid tank parameters");
                }

                if (dto.MaxLimit >= dto.Capacity)
                {
                    _logger.LogWarning("Max limit {MaxLimit} must be less than capacity {Capacity}", dto.MaxLimit, dto.Capacity);
                    throw new Exception("Max limit must be less than capacity");
                }

                if (dto.MinLimit >= dto.MaxLimit)
                {
                    _logger.LogWarning("Min limit {MinLimit} must be less than max limit {MaxLimit}", dto.MinLimit, dto.MaxLimit);
                    throw new Exception("Min limit must be less than max limit");
                }

                if (dto.CurrentVolume > dto.MaxLimit)
                {
                    _logger.LogWarning("Current volume {CurrentVolume} exceeds max limit {MaxLimit}", dto.CurrentVolume, dto.MaxLimit);
                    throw new Exception("Current volume exceeds max limit");
                }
                
                if (dto.CurrentVolume < dto.MinLimit)
                {
                    _logger.LogWarning("Current volume {CurrentVolume} is below min limit {MinLimit}", dto.CurrentVolume, dto.MinLimit);
                    throw new Exception("Current volume is below min limit");
                }

                var tank = new Tank(
                    dto.StationId,
                    dto.FuelType,
                    dto.Number,
                    dto.Capacity,
                    dto.MaxLimit,
                    dto.MinLimit,
                    dto.CurrentLevel,
                    dto.CurrentVolume,
                    dto.HasSensor
                );
                await _context.Tanks.AddAsync(tank);
                await _context.SaveChangesAsync();

                // Invalidate cache after creating tank
                await InvalidateTanksCacheAsync();
                
                _logger.LogInformation("Successfully created tank with ID: {Id} for station: {StationId}", 
                    tank.Id, dto.StationId);

                var tankResponse = _mapper.Map<TankResponse>(tank);
                return Results.Success(tankResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tank with number: {Number}, station: {StationId}", 
                    dto.Number, dto.StationId);
                return Results.Failure<TankResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<TankResponse>> EditAsync(int id, TankRequest dto)
        {
            _logger.LogInformation("Editing tank with ID: {Id}, number: {Number}, fuel type: {FuelType}, station: {StationId}", 
                id, dto.Number, dto.FuelType, dto.StationId);

            try
            {
                // Validate tank parameters
                if (dto.Capacity <= 0 || dto.MaxLimit <= 0 || dto.MinLimit <= 0 || dto.CurrentLevel <= 0 || dto.CurrentVolume <= 0)
                {
                    _logger.LogWarning("Invalid tank parameters for ID {Id}: capacity={Capacity}, maxLimit={MaxLimit}, minLimit={MinLimit}, currentLevel={CurrentLevel}, currentVolume={CurrentVolume}", 
                        id, dto.Capacity, dto.MaxLimit, dto.MinLimit, dto.CurrentLevel, dto.CurrentVolume);
                    throw new Exception("Invalid tank parameters");
                }

                if (dto.MaxLimit >= dto.Capacity)
                {
                    _logger.LogWarning("Max limit {MaxLimit} must be less than capacity {Capacity} for tank {Id}", dto.MaxLimit, dto.Capacity, id);
                    throw new Exception("Max limit must be less than capacity");
                }

                if (dto.MinLimit >= dto.MaxLimit)
                {
                    _logger.LogWarning("Min limit {MinLimit} must be less than max limit {MaxLimit} for tank {Id}", dto.MinLimit, dto.MaxLimit, id);
                    throw new Exception("Min limit must be less than max limit");
                }

                if (dto.CurrentVolume > dto.MaxLimit)
                {
                    _logger.LogWarning("Current volume {CurrentVolume} exceeds max limit {MaxLimit} for tank {Id}", dto.CurrentVolume, dto.MaxLimit, id);
                    throw new Exception("Current volume exceeds max limit");
                }
                
                if (dto.CurrentVolume < dto.MinLimit)
                {
                    _logger.LogWarning("Current volume {CurrentVolume} is below min limit {MinLimit} for tank {Id}", dto.CurrentVolume, dto.MinLimit, id);
                    throw new Exception("Current volume is below min limit");
                }

                var tank = await _context.Tanks.FindAsync(id);

                if (tank == null)
                {
                    _logger.LogWarning("Tank with ID {Id} not found", id);
                    return Results.Failure<TankResponse>(Resource.EntityNotFound);
                }

                tank.StationId = dto.StationId;
                tank.FuelType = dto.FuelType;
                tank.Number = dto.Number;
                tank.Capacity = dto.Capacity;
                tank.MaxLimit = dto.MaxLimit;
                tank.MinLimit = dto.MinLimit;
                tank.CurrentLevel = dto.CurrentLevel;
                tank.CurrentVolume = dto.CurrentVolume;
                tank.HasSensor = dto.HasSensor;

                _context.Tanks.Update(tank);
                await _context.SaveChangesAsync();

                // Invalidate cache after editing tank
                await InvalidateTanksCacheAsync();
                
                _logger.LogInformation("Successfully updated tank with ID: {Id}", id);

                var tankResponse = _mapper.Map<TankResponse>(tank);
                return Results.Success(tankResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing tank with ID: {Id}", id);
                return Results.Failure<TankResponse>(Resource.EntityNotFound);
            }
        }

        public async Task<TankResponse?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for tank with ID: {Id}", id);

            var cacheKey = $"Tank_Details_{id}";
            var cachedTank = await _cacheService.GetAsync<TankResponse>(cacheKey);
            
            if (cachedTank != null)
            {
                _logger.LogInformation("Retrieved tank details from cache for ID: {Id}", id);
                return cachedTank;
            }

            _logger.LogInformation("Tank details not in cache, fetching from database for ID: {Id}", id);

            var result = await _context.Tanks.Include(x => x.Station).SingleOrDefaultAsync(x => x.Id == id);
            
            var mappedResult = _mapper.Map<TankResponse>(result);
            
            if (mappedResult != null)
            {
                await _cacheService.SetAsync(cacheKey, mappedResult);
                _logger.LogInformation("Cached tank details for ID: {Id}", id);
            }

            return mappedResult;
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting tank with ID: {Id}", id);

            try
            {
                var tank = await _context.Tanks.FindAsync(id);

                if (tank == null)
                {
                    _logger.LogWarning("Tank with ID {Id} not found for deletion", id);
                    return Results.Failure(Resource.EntityNotFound);
                }

                _context.Tanks.Remove(tank);
                await _context.SaveChangesAsync();

                // Invalidate cache after deleting tank
                await InvalidateTanksCacheAsync();
                
                _logger.LogInformation("Successfully deleted tank with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tank with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }

        public async Task<PaginationDto<TankResponse>> GetPaginationAsync(int currentPage, GetTankRequest dto)
        {
            _logger.LogInformation("Getting paginated tanks for page {Page}, station: {StationId}", 
                currentPage, dto.StationId);

            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var cacheKey = $"Tanks_Page_{currentPage}_Station_{dto.StationId}";
            var cachedPagination = await _cacheService.GetAsync<PaginationDto<TankResponse>>(cacheKey);
            
            if (cachedPagination != null)
            {
                _logger.LogInformation("Retrieved paginated tanks from cache for page {Page}", currentPage);
                return cachedPagination;
            }

            _logger.LogInformation("Paginated tanks not in cache, fetching from database for page {Page}", currentPage);
            
            var result = await _context.Tanks
                .Include(x => x.Station)
                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<TankResponse>>(result.Data);
            var paginationResult = new PaginationDto<TankResponse>(mappedData, result.Pages);
            
            await _cacheService.SetAsync(cacheKey, paginationResult);
            
            _logger.LogInformation("Cached paginated tanks for page {Page}", currentPage);

            return paginationResult;
        }

        private async Task InvalidateTanksCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync("Tanks");
            _logger.LogInformation("Invalidated all tanks cache entries");
        }
    }
}
