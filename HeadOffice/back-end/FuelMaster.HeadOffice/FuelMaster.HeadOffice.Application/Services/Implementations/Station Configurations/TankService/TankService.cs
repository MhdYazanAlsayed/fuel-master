using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService;

public class TankService : ITankService
{
    private readonly ITankRepository _tankRepository;
    private readonly IMapper _mapper;
    private readonly ISigninService _signInService;
    private readonly ILogger<TankService> _logger;
    private readonly IEntityCache<Tank> _tankCache;
    private readonly IEntityCache<Nozzle> _nozzleCache;
    private readonly IUnitOfWork _unitOfWork;

    public TankService(
        ITankRepository tankRepository,
        IMapper mapper,
        ISigninService signInService,
        ILogger<TankService> logger,
        IEntityCache<Tank> tankCache,
        IEntityCache<Nozzle> nozzleCache,
        IUnitOfWork unitOfWork)
    {
        _tankRepository = tankRepository;
        _mapper = mapper;
        _signInService = signInService;
        _logger = logger;
        _tankCache = tankCache;
        _nozzleCache = nozzleCache;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Tank>> GetCachedTanksAsync()
    {
        var cachedTankEntities = await _tankCache.GetAllEntitiesAsync();
        if (cachedTankEntities != null)
        {
            // Apply authorization filtering after getting from cache
            return Filter(cachedTankEntities.ToList());
        }

        var tanks = await _tankRepository.GetAllAsync(includeStation: true, includeFuelType: true, includeNozzles: false);
        
        // Cache entities
        await _tankCache.SetAllAsync(tanks);
        
        return Filter(tanks.ToList());
    }

    public async Task<IEnumerable<TankResult>> GetAllAsync(GetTankDto dto)
    {
        try
        {
            _logger.LogInformation("Getting all tanks with StationId: {StationId}", dto.StationId);

            // Apply authorization filtering
            int? authStationId = _signInService.GetCurrentStationId();
            var stationId = authStationId ?? dto.StationId;

            var tanks = await GetCachedTanksAsync();
            // var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            
            // Map to DTOs
            var mappedTanks = tanks.Select(_mapper.Map<TankResult>).ToList();
            
            // Calculate CanDelete using cached nozzles
            mappedTanks = mappedTanks.Select(x => 
            {
                // x.CanDelete = nozzles == null || !nozzles.Any(n => n.TankId == x.Id);
                return x;
            }).ToList();
            
            // Apply station filter
            if (stationId.HasValue)
            {
                var filtered = mappedTanks.Where(x => x.Station != null && x.Station.Id == stationId.Value).ToList();
                return filtered;
            }

            return mappedTanks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all tanks");
            return Enumerable.Empty<TankResult>();
        }
    }

    public async Task<PaginationDto<TankResult>> GetPaginationAsync(int currentPage, GetTankDto dto)
    {
        try
        {
            _logger.LogInformation("Getting paginated tanks for page {Page} with StationId: {StationId}", 
                currentPage, dto.StationId);

            // Use GetAllAsync to leverage existing caching, then paginate in-memory
            var allTanks = await GetAllAsync(dto);

            var pagination = allTanks.ToPagination(currentPage);

            _logger.LogInformation("Retrieved paginated tanks for page {Page}", currentPage);

            return pagination;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated tanks for page {Page}", currentPage);
            return new PaginationDto<TankResult>(new List<TankResult>(), 0);
        }
    }

    public async Task<ResultDto<TankResult>> CreateAsync(TankDto dto)
    {
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

            _tankRepository.Create(tank);
            await _unitOfWork.SaveChangesAsync();

            tank = await _tankRepository.DetailsAsync(tank.Id, includeStation: true, includeFuelType: true, includeNozzles: false);

            // Update caches incrementally - cache entity, not DTO
            var tankResult = _mapper.Map<TankResult>(tank);
            await _tankCache.UpdateCacheAfterCreateAsync(tank!);

            return Result.Success(tankResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tank with StationId: {StationId}, FuelTypeId: {FuelTypeId}, Number: {Number}", 
                dto.StationId, dto.FuelTypeId, dto.Number);
            return Result.Failure<TankResult>(ex.Message);
        }
    }

    public async Task<ResultDto<TankResult>> UpdateAsync(int id, EditTankDto dto)
    {
        try
        {
            var tanks = await GetCachedTanksAsync();
            var tank = tanks.FirstOrDefault(x => x.Id == id);
            if (tank == null)
            {
                return Result.Failure<TankResult>(Resource.EntityNotFound);
            }

            tank.Update(
                dto.Capacity,
                dto.MaxLimit,
                dto.MinLimit,
                dto.CurrentLevel,
                dto.CurrentVolume,
                dto.HasSensor);

            _tankRepository.Update(tank);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _tankCache.UpdateCacheAfterEditAsync(tank);

            _logger.LogInformation("Successfully updated tank with ID: {Id}", id);

            var updated = _mapper.Map<TankResult>(tank);
            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing tank with ID: {Id}", id);
            return Result.Failure<TankResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var tanks = await GetCachedTanksAsync();
            var tank = tanks.FirstOrDefault(x => x.Id == id);
            if (tank == null)
            {
                return Result.Failure(Resource.EntityNotFound);
            }

            // Check if tank can be deleted (has nozzles)
            var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            if (nozzles != null && nozzles.Any(n => n.TankId == id))
            {
                _logger.LogWarning("Cannot delete tank with ID: {Id} because it has associated nozzles", id);
                return Result.Failure(Resource.CantDelete);
            }

            _tankRepository.Delete(tank);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _tankCache.UpdateCacheAfterDeleteAsync(id);

            _logger.LogInformation("Successfully deleted tank with ID: {Id}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tank with ID: {Id}", id);
            return Result.Failure(Resource.EntityNotFound);
        }
    }

    public async Task<TankResult?> DetailsAsync(int id)
    {
        try
        {
            var tanks = await GetCachedTanksAsync();
            var tank = tanks.FirstOrDefault(x => x.Id == id);
            if (tank == null)
            {
                return null;
            }

            var result = _mapper.Map<TankResult>(tank);

            // Calculate CanDelete using cached nozzles
            var nozzles = await _nozzleCache.GetAllEntitiesAsync();
            result.CanDelete = nozzles == null || !nozzles.Any(n => n.TankId == id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tank details for ID: {Id}", id);
            return null;
        }
    }

    private List<Tank> Filter (List<Tank> tanks)
    {
        var scope = _signInService.GetCurrentScope();
        var cityId = _signInService.GetCurrentCityId();
        var areaId = _signInService.GetCurrentAreaId();
        var stationId = _signInService.GetCurrentStationId();
        
        if (scope == Scope.ALL)
            return tanks;

        if (scope == Scope.City)
            return tanks.Where(x => x.Station!.CityId == cityId).ToList();

        if (scope == Scope.Area)
            return tanks.Where(x => x.Station!.AreaId == areaId).ToList();

        if (scope == Scope.Station || scope == Scope.Self)
            return tanks.Where(x => x.Station!.Id == stationId).ToList();

        throw new NotImplementedException();
    }
}
