using System;
using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService;

public class NozzleService : INozzleService
{
    private readonly INozzleRepository _nozzleRepository;
    private readonly ITankService _tankService;
    private readonly IZoneService _zoneService;
    private readonly ITankRepository _tankRepository; // Needed for updating tank after adding nozzle
    private readonly IMapper _mapper;
    private readonly ISigninService _signInService;
    private readonly ILogger<NozzleService> _logger;
    private readonly IEntityCache<Nozzle> _nozzleCache;
    private readonly IEntityCache<Tank> _tankCache; // Needed for cache updates
    private readonly IUnitOfWork _unitOfWork;

    public NozzleService(
        INozzleRepository nozzleRepository,
        ITankService tankService,
        IZoneService zoneService,
        ITankRepository tankRepository,
        IMapper mapper,
        ISigninService signInService,
        ILogger<NozzleService> logger,
        IEntityCache<Nozzle> nozzleCache,
        IEntityCache<Tank> tankCache,
        IUnitOfWork unitOfWork)
    {
        _nozzleRepository = nozzleRepository;
        _tankService = tankService;
        _zoneService = zoneService;
        _tankRepository = tankRepository;
        _mapper = mapper;
        _signInService = signInService;
        _logger = logger;
        _nozzleCache = nozzleCache;
        _tankCache = tankCache;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Nozzle>> GetCachedNozzlesAsync()
    {
        var cachedNozzleEntities = await _nozzleCache.GetAllEntitiesAsync();
        if (cachedNozzleEntities != null)
        {
            // Apply authorization filtering after getting from cache
            int? stationId = _signInService.GetCurrentStationIds().FirstOrDefault();
            if (stationId.HasValue)
            {
                var tanks = await _tankService.GetCachedTanksAsync();
                var tankIds = tanks?.Where(t => t.StationId == stationId.Value).Select(t => t.Id).ToList() ?? new List<int>();
                var filtered = cachedNozzleEntities.Where(x => tankIds.Contains(x.TankId)).ToList();
                _logger.LogInformation("Filtered to {Count} nozzles based on authorization", filtered.Count);
                return filtered;
            }
            
            return cachedNozzleEntities.ToList();
        }

        _logger.LogInformation("Nozzles not in cache, fetching from database");

        var nozzles = await _nozzleRepository.GetAllAsync(includeTank: true, includePump: true, includeFuelType: false);
        
        // Cache entities
        await _nozzleCache.SetAllAsync(nozzles);
        
        return nozzles;
    }

    public async Task<IEnumerable<NozzleResult>> GetAllAsync(GetNozzleDto dto)
    {
        try
        {
            _logger.LogInformation("Getting all nozzles with StationId: {StationId}, TankId: {TankId}", dto.StationId, dto.TankId);

            // Apply authorization filtering
            int? authStationId = _signInService.GetCurrentStationIds().FirstOrDefault();
            var stationId = authStationId ?? dto.StationId;

            var nozzles = await GetCachedNozzlesAsync();
            
            // Map to DTOs
            var mappedNozzles = nozzles.Select(_mapper.Map<NozzleResult>).ToList();
            
            // Set CanDelete (for now, always true - can be enhanced later to check NozzleHistory)
            mappedNozzles = mappedNozzles.Select(x => 
            {
                x.CanDelete = true; // TODO: Implement CanDelete logic - check if NozzleHistory has any records
                return x;
            }).ToList();
            
            // Apply filters
            if (stationId.HasValue)
            {
                var tanks = await _tankService.GetCachedTanksAsync();
                var tankIds = tanks?
                .Where(t => t.StationId == stationId.Value)
                .Select(t => t.Id)
                .ToList() ?? new List<int>();
                mappedNozzles = mappedNozzles.Where(x => x.Tank != null && tankIds.Contains(x.Tank.Id)).ToList();
            }

            if (dto.TankId.HasValue)
            {
                mappedNozzles = mappedNozzles.Where(x => x.Tank != null && x.Tank.Id == dto.TankId.Value).ToList();
            }

            _logger.LogInformation("Retrieved {Count} nozzles", mappedNozzles.Count);

            return mappedNozzles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all nozzles");
            return Enumerable.Empty<NozzleResult>();
        }
    }

    public async Task<PaginationDto<NozzleResult>> GetPaginationAsync(int currentPage, GetNozzleDto dto)
    {
        try
        {
            _logger.LogInformation("Getting paginated nozzles for page {Page} with StationId: {StationId}, TankId: {TankId}", 
                currentPage, dto.StationId, dto.TankId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allNozzles = await GetAllAsync(dto);

            var pagination = allNozzles.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated nozzles for page {Page}", currentPage);

            return pagination;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated nozzles for page {Page}", currentPage);
            return new PaginationDto<NozzleResult>(new List<NozzleResult>(), 0);
        }
    }

    private async Task<decimal> GetZonePriceAsync(int tankId)
    {
        _logger.LogDebug("Getting zone price for tank ID: {TankId}", tankId);

        var tanks = await _tankService.GetCachedTanksAsync();
        var tank = tanks?.FirstOrDefault(t => t.Id == tankId);

        if (tank == null || tank.Station == null)
        {
            _logger.LogError("Tank {TankId} or its station not found", tankId);
            throw new InvalidOperationException($"Tank {tankId} or its station not found");
        }

        // Get zone with prices using zone service
        var zones = await _zoneService.GetCachedZonesAsync();
        var zone = zones?.FirstOrDefault(z => z.Id == tank.Station.ZoneId);

        if (zone == null)
        {
            _logger.LogError("Zone not found for tank {TankId}", tankId);
            throw new InvalidOperationException($"Zone not found for tank {tankId}");
        }

        var zonePrice = zone.Prices?.FirstOrDefault(x => x.FuelTypeId == tank.FuelTypeId);
        if (zonePrice == null)
        {
            _logger.LogError("Zone price for fuel type {FuelTypeId} not found for tank {TankId}", tank.FuelTypeId, tankId);
            throw new InvalidOperationException($"Zone price for fuel type {tank.FuelTypeId} not found");
        }

        _logger.LogDebug("Retrieved zone price {Price} for tank {TankId} with fuel type {FuelTypeId}",
            zonePrice.Price, tankId, tank.FuelTypeId);

        return zonePrice.Price;
    }

    public async Task<ResultDto<NozzleResult>> CreateAsync(NozzleDto dto)
    {
        try
        {
            _logger.LogInformation("Creating new nozzle with TankId: {TankId}, PumpId: {PumpId}, Number: {Number}",
                dto.TankId, dto.PumpId, dto.Number);

            // Get zone price for the tank's fuel type
            var price = await GetZonePriceAsync(dto.TankId);

            // Get tank from cache using tank service
            var tanks = await _tankService.GetCachedTanksAsync();
            var tank = tanks?.FirstOrDefault(t => t.Id == dto.TankId);
            
            if (tank == null)
            {
                _logger.LogWarning("Tank with ID {TankId} not found", dto.TankId);
                return Result.Failure<NozzleResult>(Resource.EntityNotFound);
            }

            // Create Nozzle using Tank's AddNozzle method which has access to internal constructor
            var nozzle = tank.AddNozzle(dto.PumpId, dto.Number, dto.Amount, dto.Volume, dto.Totalizer, price, dto.ReaderNumber);

            // Update tank to save the nozzle (EF Core will track the nozzle in tank's collection)
            // Note: We use repository here since we're modifying the entity directly and need to persist it.
            // The service layer works with DTOs, so this is a necessary exception for domain entity operations.
            _tankRepository.Update(tank);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _nozzleCache.UpdateCacheAfterCreateAsync(nozzle);

            var nozzleResult = _mapper.Map<NozzleResult>(nozzle);
            nozzleResult.CanDelete = true; 

            _logger.LogInformation("Successfully created nozzle with ID: {Id}", nozzle.Id);

            return Result.Success(nozzleResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating nozzle with TankId: {TankId}, PumpId: {PumpId}",
                dto.TankId, dto.PumpId);
            return Result.Failure<NozzleResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<NozzleResult>> UpdateAsync(int id, NozzleDto dto)
    {
        try
        {
            _logger.LogInformation("Editing nozzle with ID: {Id}", id);

            var nozzles = await GetCachedNozzlesAsync();
            var nozzle = nozzles.FirstOrDefault(x => x.Id == id);
            if (nozzle == null)
            {
                _logger.LogWarning("Nozzle with ID {Id} not found", id);
                return Result.Failure<NozzleResult>(Resource.EntityNotFound);
            }

            nozzle.Update(dto.Amount, dto.Volume, dto.Totalizer, dto.ReaderNumber);

            // Update nozzle in repository
            _nozzleRepository.Update(nozzle);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _nozzleCache.UpdateCacheAfterEditAsync(nozzle);

            var updated = _mapper.Map<NozzleResult>(nozzle);
            updated.CanDelete = true; // TODO: Implement CanDelete logic

            _logger.LogInformation("Successfully updated nozzle with ID: {Id}", id);

            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing nozzle with ID: {Id}", id);
            return Result.Failure<NozzleResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var nozzles = await GetCachedNozzlesAsync();
            var nozzle = nozzles.FirstOrDefault(x => x.Id == id);
            if (nozzle == null)
            {
                _logger.LogWarning("Nozzle with ID {Id} not found for deletion", id);
                return Result.Failure(Resource.EntityNotFound);
            }

            _nozzleRepository.Delete(nozzle);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _nozzleCache.UpdateCacheAfterDeleteAsync(id);

            _logger.LogInformation("Successfully deleted nozzle with ID: {Id}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting nozzle with ID: {Id}", id);
            return Result.Failure(Resource.EntityNotFound);
        }
    }

    public async Task<NozzleResult?> DetailsAsync(int id)
    {
        try
        {
            var nozzles = await GetCachedNozzlesAsync();
            var nozzle = nozzles.FirstOrDefault(x => x.Id == id);
            if (nozzle == null)
            {
                return null;
            }

            var result = _mapper.Map<NozzleResult>(nozzle);
            result.CanDelete = true; // TODO: Implement CanDelete logic - check if NozzleHistory has any records

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting nozzle details for ID: {Id}", id);
            return null;
        }
    }
}

