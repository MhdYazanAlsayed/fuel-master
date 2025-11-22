using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Caching;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Tanks.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Tanks.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class TankRepository : ITankRepository
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISigninService _authorization;
        private readonly ILogger<TankRepository> _logger;
        private readonly ICacheService _cacheService; // Keep for details caching
        private readonly IEntityCache<Tank> _tankCache;
        private readonly INozzleRepository _nozzleRepository;

        public TankRepository(IContextFactory<FuelMasterDbContext> contextFactory, 
            IMapper mapper, 
            ISigninService authorization,
            ILogger<TankRepository> logger,
            ICacheService cacheService,
            INozzleRepository nozzleRepository,
            IEntityCache<Tank> tankCache)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _cacheService = cacheService; // Keep for details caching
            _tankCache = tankCache;
            _nozzleRepository = nozzleRepository;
        }

        public async Task<IEnumerable<TankResult>> GetAllAsync(GetTankRequest dto)
        {
            _logger.LogInformation("Getting all tanks with StationId: {StationId}", dto.StationId);

            // Apply authorization filtering
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            var stationId = authStationId ?? dto.StationId;

            var tanks = await GetCachedTanksAsync();
            var nozzles = await _nozzleRepository.GetCachedNozzlesAsync();
            
            // Map to DTOs
            var mappedTanksFromDb = _mapper.Map<List<TankResult>>(tanks);
            mappedTanksFromDb = mappedTanksFromDb.Select(x => 
            {
                x.CanDelete = nozzles == null || !nozzles.Any(n => n.TankId == x.Id);
                return x;
            })
            .ToList();
            
            // Apply station filter
            if (stationId.HasValue)
            {
                var filtered = mappedTanksFromDb.Where(x => x.Station != null && x.Station.Id == stationId.Value).ToList();
                _logger.LogInformation("Filtered to {Count} tanks based on StationId: {StationId}", filtered.Count, stationId.Value);
                return filtered;
            }

            return mappedTanksFromDb;
        }

        public async Task<PaginationDto<TankResult>> GetPaginationAsync(int currentPage, GetTankRequest dto)
        {
            _logger.LogInformation("Getting paginated tanks for page {Page} with StationId: {StationId}", currentPage, dto.StationId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allTanks = await GetAllAsync(dto);

            var pagination = allTanks.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated tanks for page {Page}", currentPage);

            return pagination;
        }

        public async Task<ResultDto<TankResult>> CreateAsync(CreateTankDto dto)
        {
            _logger.LogInformation("Creating new tank with StationId: {StationId}, FuelTypeId: {FuelTypeId}, Number: {Number}", 
                dto.StationId, dto.FuelTypeId, dto.Number);

            try
            {
                var tank = new Tank(
                    dto.StationId, 
                    dto.FuelTypeId, 
                    dto.Number, 
                    dto.Capacity, 
                    dto.MaxLimit, 
                    dto.MinLimit, 
                    dto.CurrentLevel, 
                    dto.CurrentVolume, 
                    dto.HasSensor);
                
                await _context.Tanks.AddAsync(tank);
                await _context.SaveChangesAsync();

                // Load related entities for mapping
                await _context.Entry(tank)
                    .Reference(x => x.Station)
                    .LoadAsync();
                await _context.Entry(tank)
                    .Reference(x => x.FuelType)
                    .LoadAsync();

                if (tank.Station != null)
                {
                    await _context.Entry(tank.Station)
                        .Reference(x => x.City)
                        .LoadAsync();
                    await _context.Entry(tank.Station)
                        .Reference(x => x.Zone)
                        .LoadAsync();
                }

                // Update caches incrementally - cache entity, not DTO
                await _tankCache.UpdateCacheAfterCreateAsync(tank);

                var tankResponse = _mapper.Map<TankResult>(tank);

                _logger.LogInformation("Successfully created tank with ID: {Id}", tank.Id);

                return Results.Success(tankResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tank with StationId: {StationId}, FuelTypeId: {FuelTypeId}", 
                    dto.StationId, dto.FuelTypeId);
                return Results.Failure<TankResult>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<TankResult>> EditAsync(int id, EditTankDto dto)
        {
            _logger.LogInformation("Editing tank with ID: {Id}", id);
            
            var tank = await _context.Tanks
                .Include(x => x.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.FuelType)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (tank == null)
            {
                _logger.LogWarning("Tank with ID {Id} not found", id);
                return Results.Failure<TankResult>(Resource.EntityNotFound);
            }

            try
            {
                tank.Update(
                    dto.Capacity, 
                    dto.MaxLimit, 
                    dto.MinLimit, 
                    dto.CurrentLevel, 
                    dto.CurrentVolume, 
                    dto.HasSensor);
                
                _context.Tanks.Update(tank);
                await _context.SaveChangesAsync();

                // Update caches incrementally - cache entity, not DTO
                await _tankCache.UpdateCacheAfterEditAsync(tank);
                await _cacheService.RemoveAsync($"Tank_Details_{tank.Id}");

                var tankResponse = _mapper.Map<TankResult>(tank);

                _logger.LogInformation("Successfully updated tank with ID: {Id}", id);

                return Results.Success(tankResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing tank with ID: {Id}", id);
                return Results.Failure<TankResult>(Resource.SthWentWrong);
            }
        }

        public async Task<TankResult?> DetailsAsync(int id)
        {
            var result = await GetCachedTanksAsync();
            var tank = result.SingleOrDefault(x => x.Id == id);

            return _mapper.Map<TankResult?>(tank);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting tank with ID: {Id}", id);

            try
            {
                var tank = await _context.Tanks
                    .Include(x => x.Station)
                    .SingleOrDefaultAsync(x => x.Id == id);
                
                if (tank == null)
                {
                    _logger.LogWarning("Tank with ID {Id} not found for deletion", id);
                    return Results.Failure(Resource.EntityNotFound);
                }

                _context.Tanks.Remove(tank);
                await _context.SaveChangesAsync();

                // Update caches incrementally
                await _tankCache.UpdateCacheAfterDeleteAsync(id);
                await _cacheService.RemoveAsync($"Tank_Details_{id}");

                _logger.LogInformation("Successfully deleted tank with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tank with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }
    
        public async Task<IEnumerable<Tank>> GetCachedTanksAsync()
        {
             // Get cached tank entities
            var cachedTankEntities = await _tankCache.GetAllEntitiesAsync();
            
            if (cachedTankEntities != null)
            {
                return cachedTankEntities;
            }

            var tanks = await _context.Tanks
                .Include(x => x.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.FuelType)
                .AsNoTracking()
                .ToListAsync();
            
            // Cache entities
            await _tankCache.SetAllAsync(tanks);

            return tanks;
        }
    }
}

