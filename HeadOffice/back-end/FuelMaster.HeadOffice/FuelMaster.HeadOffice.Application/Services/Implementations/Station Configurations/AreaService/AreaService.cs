using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Area;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.Results;
using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.AreaService;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ISigninService _signInService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityCache<Area> _areaCache;
    private readonly ILogger<AreaService> _logger;
    private readonly IMapper _mapper;
    private readonly IStationRepository _stationRepository;

    public AreaService(
        IAreaRepository areaRepository,
        IUnitOfWork unitOfWork,
        IEntityCache<Area> areaCache,
        IMapper mapper,
        ILogger<AreaService> logger,
        IStationRepository stationRepository,
        ISigninService signInService)
    {
        _areaRepository = areaRepository;
        _unitOfWork = unitOfWork;
        _areaCache = areaCache;
        _mapper = mapper;
        _logger = logger;
        _stationRepository = stationRepository;
        _signInService = signInService;
    }

    public async Task<ResultDto<AreaResult>> CreateAsync(AreaDto dto)
    {
        try
        {
            var area = new Area(dto.ArabicName, dto.EnglishName);
            _areaRepository.Create(area);
            await _unitOfWork.SaveChangesAsync();

            var areaResult = _mapper.Map<AreaResult>(area);

            // Update caches incrementally - cache entity, not DTO
            await _areaCache.UpdateCacheAfterCreateAsync(area);

            _logger.LogInformation("Successfully created area with ID: {Id}", area.Id);

            return Result.Success(areaResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating area with Arabic name: {ArabicName}, English name: {EnglishName}",
                dto.ArabicName, dto.EnglishName);
            return Result.Failure<AreaResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<AreaResult>> UpdateAsync(int id, AreaDto dto)
    {
        try
        {
            var areas = await GetCachedAreasAsync();
            var area = areas.FirstOrDefault(a => a.Id == id);
            if (area is null)
                return Result.Failure<AreaResult>(Resource.EntityNotFound);

            area.Update(dto.ArabicName, dto.EnglishName);

            _areaRepository.Update(area);
            await _unitOfWork.SaveChangesAsync();

            var updated = _mapper.Map<AreaResult>(area);

            // Update caches incrementally - cache entity, not DTO
            await _areaCache.UpdateCacheAfterEditAsync(area);

            _logger.LogInformation("Successfully updated area with ID: {Id}", id);

            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing area with ID: {Id}", id);
            return Result.Failure<AreaResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var areas = await GetCachedAreasAsync();
            var area = areas.FirstOrDefault(a => a.Id == id);
            if (area is null)
                return Result.Failure(Resource.EntityNotFound);

            _areaRepository.Delete(area);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _areaCache.UpdateCacheAfterDeleteAsync(id);

            _logger.LogInformation("Successfully deleted area with ID: {Id}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting area with ID: {Id}", id);
            return Result.Failure(Resource.EntityNotFound);
        }
    }

    public async Task<AreaResult?> DetailsAsync(int id)
    {
        try
        {
            var areas = await GetCachedAreasAsync();
            return _mapper.Map<AreaResult?>(areas.FirstOrDefault(x => x.Id == id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting area details with ID: {Id}", id);
            return null;
        }
    }

    public async Task<IEnumerable<AreaResult>> GetAllAsync()
    {
        try
        {
            var areas = await GetCachedAreasAsync();
            return _mapper.Map<IEnumerable<AreaResult>>(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all areas");
            return Enumerable.Empty<AreaResult>();
        }
    }

    public async Task<List<Area>> GetCachedAreasAsync()
    {
        var cachedAreaEntities = await _areaCache.GetAllEntitiesAsync();
        if (cachedAreaEntities != null)
        {
            return Filter(cachedAreaEntities.ToList());
        }

        _logger.LogInformation("Areas not in cache, fetching from database");

        var areas = await _areaRepository.GetAllAsync(includeStations: true);
        await _areaCache.SetAllAsync(areas);
        return Filter(areas.ToList());
    }

    public async Task<PaginationDto<AreaResult>> GetPaginationAsync(int currentPage)
    {
        try
        {
            var allAreas = await GetAllAsync();
            var stations = await _stationRepository.GetAllAsync();

            var result = allAreas.ToPagination(currentPage);

            result.Data = result.Data.Select(a =>
            {
                a.CanDelete = stations == null || !stations.Any(s => s.Area?.Id == a.Id);
                return a;
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated areas for page: {CurrentPage}", currentPage);
            return new PaginationDto<AreaResult>(new List<AreaResult>(), 0);
        }
    }

    private List<Area> Filter (List<Area> areas)
    {
        var scope = _signInService.GetCurrentScope();
        var cityId = _signInService.GetCurrentCityId();
        var areaId = _signInService.GetCurrentAreaId();
        var stationId = _signInService.GetCurrentStationId();

        if (scope == Scope.ALL || scope == Scope.City)
            return areas;

        if (scope == Scope.Area)
            return areas.Where(x => x.Id == areaId).ToList();

        if (scope == Scope.Station || scope == Scope.Self)
            return Enumerable.Empty<Area>().ToList();

        throw new NotImplementedException();
    }
}

