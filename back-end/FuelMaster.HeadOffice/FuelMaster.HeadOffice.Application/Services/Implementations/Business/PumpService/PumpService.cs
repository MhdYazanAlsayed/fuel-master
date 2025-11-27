using System;
using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService;

public class PumpService : IPumpService
{
    private readonly IPumpRepository _pumpRepository;
    private readonly IMapper _mapper;
    private readonly ISigninService _authorization;
    private readonly ILogger<PumpService> _logger;
    private readonly IEntityCache<Pump> _pumpCache;
    private readonly IEntityCache<Nozzle> _nozzleCache;
    private readonly IUnitOfWork _unitOfWork;

    public PumpService(
        IPumpRepository pumpRepository,
        IMapper mapper,
        ISigninService authorization,
        ILogger<PumpService> logger,
        IEntityCache<Pump> pumpCache,
        IEntityCache<Nozzle> nozzleCache,
        IUnitOfWork unitOfWork)
    {
        _pumpRepository = pumpRepository;
        _mapper = mapper;
        _authorization = authorization;
        _logger = logger;
        _pumpCache = pumpCache;
        _nozzleCache = nozzleCache;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Pump>> GetCachedPumpsAsync()
    {
        var cachedPumpEntities = await _pumpCache.GetAllEntitiesAsync();
        if (cachedPumpEntities != null)
        {
            // Apply authorization filtering after getting from cache
            var stationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            if (stationId.HasValue)
            {
                var filtered = cachedPumpEntities.Where(x => x.StationId == stationId.Value).ToList();
                _logger.LogInformation("Filtered to {Count} pumps based on authorization", filtered.Count);
                return filtered;
            }
            
            return cachedPumpEntities.ToList();
        }

        _logger.LogInformation("Pumps not in cache, fetching from database");

        var pumps = await _pumpRepository.GetAllAsync(includeStation: true, includeNozzles: false);
        
        // Cache entities
        await _pumpCache.SetAllAsync(pumps);
        
        return pumps;
    }

    public async Task<IEnumerable<PumpResult>> GetAllAsync(GetPumpDto dto)
    {
        try
        {
            _logger.LogInformation("Getting all pumps with StationId: {StationId}", dto.StationId);

            // Apply authorization filtering
            var authStationId = (await _authorization.TryToGetStationIdAsync()) ?? null;
            var stationId = authStationId ?? dto.StationId;

            var pumps = await GetCachedPumpsAsync();
            var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            
            // Map to DTOs
            var mappedPumps = pumps.Select(_mapper.Map<PumpResult>).ToList();
            
            // Calculate NozzleCount and CanDelete using cached nozzles
            mappedPumps = mappedPumps.Select(x => 
            {
                if (nozzles != null)
                {
                    x.NozzleCount = nozzles.Count(n => n.PumpId == x.Id);
                    x.CanDelete = x.NozzleCount == 0;
                }
                else
                {
                    x.NozzleCount = 0;
                    x.CanDelete = true;
                }
                return x;
            }).ToList();
            
            // Apply station filter
            if (stationId.HasValue)
            {
                var filtered = mappedPumps.Where(x => x.Station != null && x.Station.Id == stationId.Value).ToList();
                _logger.LogInformation("Filtered to {Count} pumps based on StationId: {StationId}", filtered.Count, stationId.Value);
                return filtered;
            }

            return mappedPumps;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all pumps");
            return Enumerable.Empty<PumpResult>();
        }
    }

    public async Task<PaginationDto<PumpResult>> GetPaginationAsync(int currentPage, GetPumpDto dto)
    {
        try
        {
            _logger.LogInformation("Getting paginated pumps for page {Page} with StationId: {StationId}", 
                currentPage, dto.StationId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allPumps = await GetAllAsync(dto);

            var pagination = allPumps.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated pumps for page {Page}", currentPage);

            return pagination;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated pumps for page {Page}", currentPage);
            return new PaginationDto<PumpResult>(new List<PumpResult>(), 0);
        }
    }

    public async Task<ResultDto<PumpResult>> CreateAsync(PumpDto dto)
    {
        try
        {
            var pump = new Pump(
                dto.Number,
                dto.StationId,
                dto.Manufacturer);

            _pumpRepository.Create(pump);
            await _unitOfWork.SaveChangesAsync();

            var pumpResult = _mapper.Map<PumpResult>(pump);

            // Update caches incrementally - cache entity, not DTO
            await _pumpCache.UpdateCacheAfterCreateAsync(pump);

            _logger.LogInformation("Successfully created pump with ID: {Id}", pump.Id);

            return Result.Success(pumpResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pump with StationId: {StationId}, Number: {Number}", 
                dto.StationId, dto.Number);
            return Result.Failure<PumpResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<PumpResult>> UpdateAsync(int id, PumpDto dto)
    {
        try
        {
            var pumps = await GetCachedPumpsAsync();
            var pump = pumps.FirstOrDefault(x => x.Id == id);
            if (pump == null)
            {
                return Result.Failure<PumpResult>(Resource.EntityNotFound);
            }

            pump.Update(
                dto.Number,
                dto.StationId,
                dto.Manufacturer);

            _pumpRepository.Update(pump);
            await _unitOfWork.SaveChangesAsync();

            var updated = _mapper.Map<PumpResult>(pump);

            // Update caches incrementally
            await _pumpCache.UpdateCacheAfterEditAsync(pump);

            _logger.LogInformation("Successfully updated pump with ID: {Id}", id);

            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing pump with ID: {Id}", id);
            return Result.Failure<PumpResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var pumps = await GetCachedPumpsAsync();
            var pump = pumps.FirstOrDefault(x => x.Id == id);
            if (pump == null)
            {
                return Result.Failure(Resource.EntityNotFound);
            }

            // Check if pump can be deleted (has nozzles)
            var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            if (nozzles != null && nozzles.Any(n => n.PumpId == id))
            {
                _logger.LogWarning("Cannot delete pump with ID: {Id} because it has associated nozzles", id);
                return Result.Failure(Resource.CantDelete);
            }

            _pumpRepository.Delete(pump);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _pumpCache.UpdateCacheAfterDeleteAsync(id);

            _logger.LogInformation("Successfully deleted pump with ID: {Id}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting pump with ID: {Id}", id);
            return Result.Failure(Resource.EntityNotFound);
        }
    }

    public async Task<PumpResult?> DetailsAsync(int id)
    {
        try
        {
            var pumps = await GetCachedPumpsAsync();
            var pump = pumps.FirstOrDefault(x => x.Id == id);
            if (pump == null)
            {
                return null;
            }

            var result = _mapper.Map<PumpResult>(pump);

            // Calculate NozzleCount and CanDelete using cached nozzles
            var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            if (nozzles != null)
            {
                result.NozzleCount = nozzles.Count(n => n.PumpId == id);
                result.CanDelete = result.NozzleCount == 0;
            }
            else
            {
                result.NozzleCount = 0;
                result.CanDelete = true;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pump details for ID: {Id}", id);
            return null;
        }
    }
}

