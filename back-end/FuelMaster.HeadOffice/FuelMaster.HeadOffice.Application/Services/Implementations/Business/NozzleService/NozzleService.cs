using System;
using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService;

public class NozzleService : INozzleService
{
    private readonly INozzleRepository _nozzleRepository;
    private readonly ITankRepository _tankRepository;
    private readonly IZoneRepository _zoneRepository;
    private readonly IMapper _mapper;
    private readonly ISigninService _authorization;
    private readonly ILogger<NozzleService> _logger;
    private readonly IEntityCache<Nozzle> _nozzleCache;
    private readonly IEntityCache<Tank> _tankCache;
    private readonly IUnitOfWork _unitOfWork;

    public NozzleService(
        INozzleRepository nozzleRepository,
        ITankRepository tankRepository,
        IZoneRepository zoneRepository,
        IMapper mapper,
        ISigninService authorization,
        ILogger<NozzleService> logger,
        IEntityCache<Nozzle> nozzleCache,
        IEntityCache<Tank> tankCache,
        IUnitOfWork unitOfWork)
    {
        _nozzleRepository = nozzleRepository;
        _tankRepository = tankRepository;
        _zoneRepository = zoneRepository;
        _mapper = mapper;
        _authorization = authorization;
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
            var stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            if (stationId.HasValue)
            {
                var tanks = await _tankCache.GetAllEntitiesAsync();
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
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
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
                var tanks = await _tankCache.GetAllEntitiesAsync();
                var tankIds = tanks?.Where(t => t.StationId == stationId.Value).Select(t => t.Id).ToList() ?? new List<int>();
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

        var tanks = await _tankCache.GetAllEntitiesAsync();
        var tank = tanks?.FirstOrDefault(t => t.Id == tankId);
        
        if (tank == null)
        {
            // If not in cache, fetch from repository
            var tankList = await _tankRepository.GetAllAsync(includeStation: true, includeFuelType: true, includeNozzles: false);
            tank = tankList.FirstOrDefault(t => t.Id == tankId);
        }

        if (tank == null || tank.Station == null)
        {
            _logger.LogError("Tank {TankId} or its station not found", tankId);
            throw new InvalidOperationException($"Tank {tankId} or its station not found");
        }

        // Get zone with prices
        var zones = await _zoneRepository.GetAllAsync(includePrices: true, includeFuelType: false, includeStations: false, includeTanks: false, includeNozzles: false);
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

            // Get tank from cache or repository
            var tanks = await _tankCache.GetAllEntitiesAsync();
            var tank = tanks?.FirstOrDefault(t => t.Id == dto.TankId);
            
            if (tank == null)
            {
                var tankList = await _tankRepository.GetAllAsync(includeStation: true, includeFuelType: true, includeNozzles: false);
                tank = tankList.FirstOrDefault(t => t.Id == dto.TankId);
            }

            if (tank == null)
            {
                _logger.LogWarning("Tank with ID {TankId} not found", dto.TankId);
                return Result.Failure<NozzleResult>(Resource.EntityNotFound);
            }

            // Create Nozzle using Tank's AddNozzle method which has access to internal constructor
            tank.AddNozzle(dto.PumpId, dto.Number, dto.Amount, dto.Volume, dto.Totalizer, price, dto.ReaderNumber);
            var nozzle = tank.Nozzles.Last();

            // Update tank to save the nozzle (EF Core will track the nozzle in tank's collection)
            _tankRepository.Update(tank);
            await _unitOfWork.SaveChangesAsync();

            // // Get the created nozzle with includes for mapping
            // var context = _contextFactory.CurrentContext;
            // await context.Entry(nozzle)
            //     .Reference(x => x.Tank)
            //     .LoadAsync();
            // await context.Entry(nozzle)
            //     .Reference(x => x.Pump)
            //     .LoadAsync();
            // await context.Entry(nozzle)
            //     .Reference(x => x.FuelType)
            //     .LoadAsync();

            // Update caches incrementally
            await _nozzleCache.UpdateCacheAfterCreateAsync(nozzle);
            await _tankCache.UpdateCacheAfterEditAsync(tank);

            var nozzleResult = _mapper.Map<NozzleResult>(nozzle);
            nozzleResult.CanDelete = true; // TODO: Implement CanDelete logic

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

            // Note: Nozzle entity doesn't have a public Update method
            // Most properties are private setters, so we can only change the price via ChangePrice
            // For now, we'll update what we can - if the DTO has different values, 
            // we might need to handle this differently or throw an error
            
            // If price needs to be updated, get new zone price
            var newPrice = await GetZonePriceAsync(dto.TankId);
            if (nozzle.Price != newPrice)
            {
                nozzle.ChangePrice(newPrice);
            }

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

            // TODO: Check if NozzleHistory has any records referencing this nozzle
            // For now, allow deletion
            // var nozzleHistories = await _nozzleHistoryCache.GetAllEntitiesAsync();
            // if (nozzleHistories != null && nozzleHistories.Any(nh => nh.NozzleId == id))
            // {
            //     _logger.LogWarning("Cannot delete nozzle with ID: {Id} because it has associated history records", id);
            //     return Result.Failure(Resource.CantDelete);
            // }

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

