using AutoMapper;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Interfaces.Caching;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Repositories.Pumps
{
    public class PumpRepository : IPumpRepository
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISigninService _authorization;
        private readonly ILogger<PumpRepository> _logger;
        private readonly IEntityCache<Pump> _pumpCache;
        private readonly INozzleRepository _nozzleRepository;   

        public PumpRepository(
            IContextFactory<FuelMasterDbContext> contextFactory,
            IMapper mapper,
            ISigninService authorization,
            INozzleRepository nozzleRepository,
            ILogger<PumpRepository> logger,
            IEntityCache<Pump> pumpCache)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _pumpCache = pumpCache;
            _nozzleRepository = nozzleRepository;
        }

        public async Task<IEnumerable<Pump>> GetCachedPumpsAsync()
        {
            // Get cached pump entities
            var cachedPumpEntities = await _pumpCache.GetAllEntitiesAsync();

            if (cachedPumpEntities != null)
            {
                return cachedPumpEntities;
            }

            _logger.LogInformation("Pumps not in cache, fetching from database");

            var pumps = await _context.Pumps
                .Include(x => x.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.Nozzles)
                .AsNoTracking()
                .ToListAsync();

            // Cache entities
            await _pumpCache.SetAllAsync(pumps);

            _logger.LogInformation("Cached {Count} pumps", pumps.Count);

            return pumps;
        }

        public async Task<IEnumerable<PumpResult>> GetAllAsync(GetPumpRequest dto)
        {
            _logger.LogInformation("Getting all pumps with StationId: {StationId}", dto.StationId);

            // Apply authorization filtering
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            var stationId = authStationId ?? dto.StationId;

            var pumps = await GetCachedPumpsAsync();
            var nozzles = await _nozzleRepository.GetCachedNozzlesAsync();

            // Map entities to DTOs
            var mappedPumps = _mapper.Map<List<PumpResult>>(pumps);
            mappedPumps = mappedPumps.Select(x => 
            {
                x.NozzleCount = nozzles.Count(n => n.PumpId == x.Id);
                return x;
            })
            .ToList();

            // Apply station filter
            if (stationId.HasValue)
            {
                mappedPumps = mappedPumps
                    .Where(x => x.StationId == stationId.Value)
                    .ToList();
            }

            _logger.LogInformation("Retrieved {Count} pumps", mappedPumps.Count);

            return mappedPumps;
        }

        public async Task<PaginationDto<PumpResult>> GetPaginationAsync(int currentPage, GetPumpRequest dto)
        {
            _logger.LogInformation("Getting paginated pumps for page {Page} with StationId: {StationId}", 
                currentPage, dto.StationId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allPumps = await GetAllAsync(dto);
            var nozzles = await _nozzleRepository.GetCachedNozzlesAsync();
            
            allPumps = allPumps.Select(x => 
            {
                x.NozzleCount = nozzles.Count(n => n.PumpId == x.Id);
                return x;
            })
            .ToList();

            var pagination = allPumps.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated pumps for page {Page}", currentPage);

            return pagination;
        }

        public async Task<ResultDto<PumpResult>> CreateAsync(CreatePumpDto dto)
        {
            _logger.LogInformation("Creating new pump with Number: {Number}, StationId: {StationId}, Manufacturer: {Manufacturer}",
                dto.Number, dto.StationId, dto.Manufacturer);

            try
            {
                var pump = new Pump(dto.Number, dto.StationId, dto.Manufacturer);

                await _context.Pumps.AddAsync(pump);
                await _context.SaveChangesAsync();

                // Load related entities for mapping
                await _context.Entry(pump)
                    .Reference(x => x.Station)
                    .LoadAsync();

                if (pump.Station != null)
                {
                    await _context.Entry(pump.Station)
                        .Reference(x => x.City)
                        .LoadAsync();
                    await _context.Entry(pump.Station)
                        .Reference(x => x.Zone)
                        .LoadAsync();
                }

                // Update caches incrementally - cache entity, not DTO
                await _pumpCache.UpdateCacheAfterCreateAsync(pump);

                var pumpResponse = _mapper.Map<PumpResult>(pump);
                pumpResponse.NozzleCount = 0;
                _logger.LogInformation("Successfully created pump with ID: {Id}", pump.Id);

                return Results.Success(pumpResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pump with Number: {Number}, StationId: {StationId}",
                    dto.Number, dto.StationId);
                return Results.Failure<PumpResult>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<PumpResult>> EditAsync(int id, EditPumpDto dto)
        {
            _logger.LogInformation("Editing pump with ID: {Id}", id);

            var pump = await _context.Pumps
                .Include(x => x.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.Nozzles)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (pump == null)
            {
                _logger.LogWarning("Pump with ID {Id} not found", id);
                return Results.Failure<PumpResult>(Resource.EntityNotFound);
            }

            try
            {
                pump.Update(dto.Number, dto.StationId, dto.Manufacturer);

                _context.Pumps.Update(pump);
                await _context.SaveChangesAsync();

                // Update caches incrementally - cache entity, not DTO
                await _pumpCache.UpdateCacheAfterEditAsync(pump);

                var pumpResponse = _mapper.Map<PumpResult>(pump);
              
                _logger.LogInformation("Successfully updated pump with ID: {Id}", id);

                return Results.Success(pumpResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing pump with ID: {Id}", id);
                return Results.Failure<PumpResult>(Resource.SthWentWrong);
            }
        }

        public async Task<PumpResult?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for pump with ID: {Id}", id);

            // Get all cached pumps and search for the specific one
            var pumps = await GetCachedPumpsAsync();
            var pump = pumps.SingleOrDefault(x => x.Id == id);

            if (pump == null)
            {
                _logger.LogWarning("Pump with ID {Id} not found", id);
                return null;
            }

            var mappedResult = _mapper.Map<PumpResult>(pump);

            return mappedResult;
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting pump with ID: {Id}", id);

            try
            {
                var pump = await _context.Pumps.FindAsync(id);

                if (pump == null)
                {
                    _logger.LogWarning("Pump with ID {Id} not found for deletion", id);
                    return Results.Failure(Resource.EntityNotFound);
                }

                _context.Pumps.Remove(pump);
                await _context.SaveChangesAsync();

                // Update caches incrementally
                await _pumpCache.UpdateCacheAfterDeleteAsync(id);

                _logger.LogInformation("Successfully deleted pump with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pump with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }
    }
}
