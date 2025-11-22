using AutoMapper;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Interfaces.Caching;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FuelMaster.HeadOffice.ApplicationService.Repositories.Nozzles
{
    public class NozzleRepository : INozzleRepository
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISigninService _authorization;
        private readonly ILogger<NozzleRepository> _logger;
        private readonly IEntityCache<Nozzle> _nozzleCache;

        public NozzleRepository(
            IContextFactory<FuelMasterDbContext> contextFactory,
            IMapper mapper,
            ISigninService authorization,
            ILogger<NozzleRepository> logger,
            IEntityCache<Nozzle> nozzleCache)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _authorization = authorization;
            _logger = logger;
            _nozzleCache = nozzleCache;
        }

        public async Task<IEnumerable<Nozzle>> GetCachedNozzlesAsync()
        {
            // Get cached nozzle entities
            var cachedNozzleEntities = await _nozzleCache.GetAllEntitiesAsync();

            if (cachedNozzleEntities != null)
            {
                return cachedNozzleEntities;
            }

            _logger.LogInformation("Nozzles not in cache, fetching from database");

            var nozzles = await _context.Nozzles
                .Include(x => x.Tank)
                .ThenInclude(x => x!.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Tank)
                .ThenInclude(x => x!.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.Pump)
                .Include(x => x.FuelType)
                .AsNoTracking()
                .ToListAsync();

            // Cache entities
            await _nozzleCache.SetAllAsync(nozzles);

            _logger.LogInformation("Cached {Count} nozzles", nozzles.Count);

            return nozzles;
        }

        public async Task<IEnumerable<NozzleResult>> GetAllAsync(GetNozzleRequest dto)
        {
            _logger.LogInformation("Getting all nozzles with StationId: {StationId}, TankId: {TankId}", dto.StationId, dto.TankId);

            // Apply authorization filtering
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            var stationId = authStationId ?? dto.StationId;

            var nozzles = await GetCachedNozzlesAsync();

            // Map entities to DTOs
            var mappedNozzles = _mapper.Map<List<NozzleResult>>(nozzles);

            // Apply filters
            if (stationId.HasValue)
            {
                mappedNozzles = mappedNozzles
                    .Where(x => x.Tank != null && x.Tank.Station != null && x.Tank.Station.Id == stationId.Value)
                    .ToList();
            }

            if (dto.TankId.HasValue)
            {
                mappedNozzles = mappedNozzles
                    .Where(x => x.Tank != null && x.Tank.Id == dto.TankId.Value)
                    .ToList();
            }

            // TODO: Implement CanDelete logic - check if NozzleHistory has any records referencing this nozzle
            // For now, set CanDelete to true for all nozzles
            foreach (var nozzle in mappedNozzles)
            {
                nozzle.CanDelete = true;
            }

            _logger.LogInformation("Retrieved {Count} nozzles", mappedNozzles.Count);

            return mappedNozzles;
        }

        public async Task<PaginationDto<NozzleResult>> GetPaginationAsync(int currentPage, GetNozzleRequest dto)
        {
            _logger.LogInformation("Getting paginated nozzles for page {Page} with StationId: {StationId}, TankId: {TankId}", 
                currentPage, dto.StationId, dto.TankId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allNozzles = await GetAllAsync(dto);

            var pagination = allNozzles.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated nozzles for page {Page}", currentPage);

            return pagination;
        }

        public async Task<ResultDto<NozzleResult>> CreateAsync(CreateNozzleDto dto)
        {
            _logger.LogInformation("Creating new nozzle with TankId: {TankId}, PumpId: {PumpId}, Number: {Number}",
                dto.TankId, dto.PumpId, dto.Number);

            try
            {
                // Get zone price for the tank's fuel type
                var price = await GetZonePriceAsync(dto.TankId);

                // Get tank to retrieve FuelTypeId
                var tank = await _context.Tanks
                    .Include(x => x.FuelType)
                    .SingleOrDefaultAsync(x => x.Id == dto.TankId);

                if (tank == null)
                {
                    _logger.LogWarning("Tank with ID {TankId} not found", dto.TankId);
                    return Results.Failure<NozzleResult>(Resource.EntityNotFound);
                }

                // Create Nozzle using Tank's AddNozzle method which has access to internal constructor
                // First, we need to add it through the Tank entity
                tank.AddNozzle(dto.PumpId, dto.Number, dto.Amount, dto.Volume, dto.Totalizer, price, dto.ReaderNumber);
                var nozzle = tank.Nozzles.Last();
                
                // Remove from tank's collection temporarily to add directly to context
                var nozzlesField = typeof(Tank).GetField("_nozzles", BindingFlags.NonPublic | BindingFlags.Instance);
                if (nozzlesField != null)
                {
                    var nozzlesList = (List<Nozzle>)nozzlesField.GetValue(tank)!;
                    nozzlesList.Remove(nozzle);
                }

                // Set FuelTypeId using reflection since it's private
                var fuelTypeIdProperty = typeof(Nozzle).GetProperty("FuelTypeId", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (fuelTypeIdProperty != null)
                {
                    fuelTypeIdProperty.SetValue(nozzle, tank.FuelTypeId);
                }

                await _context.Nozzles.AddAsync(nozzle);
                await _context.SaveChangesAsync();

                // Load related entities for mapping
                await _context.Entry(nozzle)
                    .Reference(x => x.Tank)
                    .LoadAsync();
                await _context.Entry(nozzle)
                    .Reference(x => x.Pump)
                    .LoadAsync();
                await _context.Entry(nozzle)
                    .Reference(x => x.FuelType)
                    .LoadAsync();

                if (nozzle.Tank != null)
                {
                    await _context.Entry(nozzle.Tank)
                        .Reference(x => x.Station)
                        .LoadAsync();
                    if (nozzle.Tank.Station != null)
                    {
                        await _context.Entry(nozzle.Tank.Station)
                            .Reference(x => x.City)
                            .LoadAsync();
                        await _context.Entry(nozzle.Tank.Station)
                            .Reference(x => x.Zone)
                            .LoadAsync();
                    }
                }

                // Update caches incrementally - cache entity, not DTO
                await _nozzleCache.UpdateCacheAfterCreateAsync(nozzle);

                var nozzleResponse = _mapper.Map<NozzleResult>(nozzle);
                nozzleResponse.CanDelete = true; // TODO: Implement CanDelete logic

                _logger.LogInformation("Successfully created nozzle with ID: {Id}", nozzle.Id);

                return Results.Success(nozzleResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating nozzle with TankId: {TankId}, PumpId: {PumpId}",
                    dto.TankId, dto.PumpId);
                return Results.Failure<NozzleResult>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<NozzleResult>> EditAsync(int id, EditNozzleDto dto)
        {
            _logger.LogInformation("Editing nozzle with ID: {Id}", id);

            var nozzle = await _context.Nozzles
                .Include(x => x.Tank)
                .ThenInclude(x => x!.Station)
                .ThenInclude(x => x!.City)
                .Include(x => x.Tank)
                .ThenInclude(x => x!.Station)
                .ThenInclude(x => x!.Zone)
                .Include(x => x.Tank)
                .ThenInclude(x => x!.FuelType)
                .Include(x => x.Pump)
                .Include(x => x.FuelType)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (nozzle == null)
            {
                _logger.LogWarning("Nozzle with ID {Id} not found", id);
                return Results.Failure<NozzleResult>(Resource.EntityNotFound);
            }

            try
            {
                // Get zone price for the tank's fuel type
                var price = await GetZonePriceAsync(dto.TankId);

                // Update nozzle properties using reflection since they're private
                var tankIdProperty = typeof(Nozzle).GetProperty("TankId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var pumpIdProperty = typeof(Nozzle).GetProperty("PumpId",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var numberProperty = typeof(Nozzle).GetProperty("Number",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var statusProperty = typeof(Nozzle).GetProperty("Status",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var readerNumberProperty = typeof(Nozzle).GetProperty("ReaderNumber",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var amountProperty = typeof(Nozzle).GetProperty("Amount",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var volumeProperty = typeof(Nozzle).GetProperty("Volume",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var totalizerProperty = typeof(Nozzle).GetProperty("Totalizer",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (tankIdProperty != null) tankIdProperty.SetValue(nozzle, dto.TankId);
                if (pumpIdProperty != null) pumpIdProperty.SetValue(nozzle, dto.PumpId);
                if (numberProperty != null) numberProperty.SetValue(nozzle, dto.Number);
                if (statusProperty != null) statusProperty.SetValue(nozzle, dto.Status);
                if (readerNumberProperty != null) readerNumberProperty.SetValue(nozzle, dto.ReaderNumber);
                if (amountProperty != null) amountProperty.SetValue(nozzle, dto.Amount);
                if (volumeProperty != null) volumeProperty.SetValue(nozzle, dto.Volume);
                if (totalizerProperty != null) totalizerProperty.SetValue(nozzle, dto.Totalizer);

                nozzle.ChangePrice(price);

                _context.Nozzles.Update(nozzle);
                await _context.SaveChangesAsync();

                // Update caches incrementally - cache entity, not DTO
                await _nozzleCache.UpdateCacheAfterEditAsync(nozzle);

                var nozzleResponse = _mapper.Map<NozzleResult>(nozzle);
                // TODO: Implement CanDelete logic
                nozzleResponse.CanDelete = true;

                _logger.LogInformation("Successfully updated nozzle with ID: {Id}", id);

                return Results.Success(nozzleResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing nozzle with ID: {Id}", id);
                return Results.Failure<NozzleResult>(Resource.SthWentWrong);
            }
        }

        public async Task<NozzleResult?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for nozzle with ID: {Id}", id);

            // Get all cached nozzles and search for the specific one
            var nozzles = await GetCachedNozzlesAsync();
            var nozzle = nozzles.SingleOrDefault(x => x.Id == id);

            if (nozzle == null)
            {
                _logger.LogWarning("Nozzle with ID {Id} not found", id);
                return null;
            }

            var mappedResult = _mapper.Map<NozzleResult>(nozzle);
            // TODO: Implement CanDelete logic
            mappedResult.CanDelete = true;

            return mappedResult;
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting nozzle with ID: {Id}", id);

            try
            {
                var nozzle = await _context.Nozzles.FindAsync(id);

                if (nozzle == null)
                {
                    _logger.LogWarning("Nozzle with ID {Id} not found for deletion", id);
                    return Results.Failure(Resource.EntityNotFound);
                }

                _context.Nozzles.Remove(nozzle);
                await _context.SaveChangesAsync();

                // Update caches incrementally
                await _nozzleCache.UpdateCacheAfterDeleteAsync(id);

                _logger.LogInformation("Successfully deleted nozzle with ID: {Id}", id);

                return Results.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting nozzle with ID: {Id}", id);
                return Results.Failure(Resource.EntityNotFound);
            }
        }

        private async Task<decimal> GetZonePriceAsync(int tankId)
        {
            _logger.LogDebug("Getting zone price for tank ID: {TankId}", tankId);

            var tank = await _context.Tanks
                .Where(x => x.Id == tankId)
                .Include(x => x.Station)
                .ThenInclude(x => x!.Zone)
                .ThenInclude(x => x!.Prices)
                .Include(x => x.FuelType)
                .SingleOrDefaultAsync();

            if (tank == null || tank.Station?.Zone?.Prices == null)
            {
                _logger.LogError("Tank {TankId} or its zone/prices not found", tankId);
                throw new InvalidOperationException($"Tank {tankId} or its zone/prices not found");
            }

            var zonePrice = tank.Station!.Zone!.Prices!.SingleOrDefault(x => x.FuelTypeId == tank.FuelTypeId);
            if (zonePrice == null)
            {
                _logger.LogError("Zone price for fuel type {FuelTypeId} not found for tank {TankId}", tank.FuelTypeId, tankId);
                throw new InvalidOperationException($"Zone price for fuel type {tank.FuelTypeId} not found");
            }

            _logger.LogDebug("Retrieved zone price {Price} for tank {TankId} with fuel type {FuelTypeId}",
                zonePrice.Price, tankId, tank.FuelTypeId);

            return zonePrice.Price;
        }
    }
}
