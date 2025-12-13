using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService;

public class ZoneService : IZoneService
{
    private readonly IPricingService _pricingService;
    private readonly IZoneRepository _zoneRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ZoneService> _logger;
    private readonly IEntityCache<Zone> _zoneCache;
    private readonly IFuelTypeService _fuelTypeService;
    private readonly IStationRepository _stationRepository;
    public ZoneService(
        IZoneRepository zoneRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEntityCache<Zone> zoneCache,
        ILogger<ZoneService> logger,
        IFuelTypeService fuelTypeService,
        IPricingService pricingService,
        IStationRepository stationRepository)
    {
        _zoneRepository = zoneRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _zoneCache = zoneCache;
        _fuelTypeService = fuelTypeService;
        _pricingService = pricingService;
        _stationRepository = stationRepository;
    }

    public async Task<ResultDto<ZoneResult>> CreateAsync(ZoneDto dto)
    {   
        try
        {
            var zone = new Zone(dto.ArabicName, dto.EnglishName);
            _zoneRepository.Create(zone);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally - cache entity, not DTO
            await _zoneCache.UpdateCacheAfterCreateAsync(zone);

            var zoneResult = _mapper.Map<ZoneResult>(zone);
            return Helpers.Result.Success(zoneResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating zone with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);
            return Helpers.Result.Failure<ZoneResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try 
        {
            var zones = await GetCachedZonesAsync();
            var zone = zones.FirstOrDefault(z => z.Id == id);
            if (zone is null)
                return Helpers.Result.Failure(Resource.EntityNotFound);

            _zoneRepository.Delete(zone);
            await _unitOfWork.SaveChangesAsync();
            await _zoneCache.UpdateCacheAfterDeleteAsync(id);

            return Helpers.Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting zone with ID: {Id}", id);
            return Result.Failure(Resource.SthWentWrong);
        }
    }

    public async Task<ZoneResult?> DetailsAsync(int id)
    {
        var zones = await GetCachedZonesAsync();

        return _mapper.Map<ZoneResult>(zones.FirstOrDefault(x => x.Id == id));
    }

    public async Task<ResultDto<ZoneResult>> UpdateAsync(int id, ZoneDto dto)
    {
        try 
        {
            var cachedZones = await GetCachedZonesAsync();
            var zone = cachedZones.SingleOrDefault(x => x.Id == id);
            if (zone is null) 
            {
                return Result.Failure<ZoneResult>(Resource.EntityNotFound);
            }

            zone.Update(dto.ArabicName, dto.EnglishName);
            _zoneRepository.Update(zone);
            await _unitOfWork.SaveChangesAsync();
            await _zoneCache.UpdateCacheAfterEditAsync(zone);

            return Result.Success(_mapper.Map<ZoneResult>(zone));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing zone with ID: {Id}", id);
            return Result.Failure<ZoneResult>(Resource.SthWentWrong);
        }
    }

    public async Task<IEnumerable<ZoneResult>> GetAllAsync()
    {
        var cachedZones = await GetCachedZonesAsync();
        var stations = await _stationRepository.GetAllAsync();

        return stations.Select(x => 
        {
            return new ZoneResult() 
            {
                Id = x.Id,
                ArabicName = x.ArabicName,
                EnglishName = x.EnglishName,
                CanDelete = stations == null || !stations.Any(s => s.Zone?.Id == x.Id)
            };
        }).ToList();
    }

    public async Task<PaginationDto<ZoneResult>> GetPaginationAsync(int currentPage)
    {
        var cachedZones = await GetCachedZonesAsync();
        
        var paginatedData = cachedZones.ToPagination(currentPage);
        var stations = await _stationRepository.GetAllAsync();

        var mappedData = paginatedData.Data.Select(x => 
        {
            return new ZoneResult() 
            {
                Id = x.Id,
                ArabicName = x.ArabicName,
                EnglishName = x.EnglishName,
                // TODO : Add station service to check if the zone can be deleted
                CanDelete = stations == null || !stations.Any(s => s.Zone?.Id == x.Id)
            };
        });
        
        return new PaginationDto<ZoneResult>(mappedData.ToList(), paginatedData.Pages);
    }

    public async Task<List<Zone>> GetCachedZonesAsync()
    {
        var cachedZones = await _zoneCache.GetAllEntitiesAsync();
        if (cachedZones != null)
        {
            return cachedZones.ToList();
        }

        var zones = await _zoneRepository.GetAllAsync(includePrices: true, includeFuelType: true);
        await _zoneCache.SetAllAsync(zones);
        return zones.ToList();
    }

    public async Task<IEnumerable<ZonePriceResult>> GetPricesAsync(int zoneId)
    {
        var fuelTypes = await _fuelTypeService.GetCachedFuelTypesAsync();
        var cachedZones = await GetCachedZonesAsync();
        var zone = cachedZones.SingleOrDefault(x => x.Id == zoneId);
        if (zone is null)
            throw new ZoneNotFoundException($"Zone with id {zoneId} not found");

        var isThereAnyPriceMissing = fuelTypes.Any(x => !zone.Prices.Any(p => p.FuelTypeId == x.Id));
        if (isThereAnyPriceMissing)
        {
            zone.InitializePrices(fuelTypes.Select(x => x.Id).ToList());

            _zoneRepository.Update(zone);
            await _unitOfWork.SaveChangesAsync();
            await _zoneCache.UpdateCacheAfterEditAsync(zone);
        }

        return _mapper.Map<IEnumerable<ZonePriceResult>>(zone.Prices);
    }

    public async Task<ResultDto> ChangePriceAsync(int zoneId, ChangePriceDto dto)
    {
        try
        {
            var mappedPrices = _mapper.Map<List<ChangePricesDto>>(dto.Prices);
            var changedZonePrices = await _pricingService.ChangePricesAsync(zoneId, mappedPrices);

            foreach (var zonePrice in changedZonePrices)
            {
                if (zonePrice.Zone is null)
                    throw new ZoneNotFoundException($"Zone with id {zoneId} not found");

                await _zoneCache.UpdateCacheAfterEditAsync(zonePrice.Zone);
            }

            return Result.Success();
        }
        catch (ZoneNotFoundException)
        {
            return Result.Failure(Resource.EntityNotFound);
        }
        catch (UnauthorizedAccessException)
        {
            return Result.Failure(Resource.Unauthorized);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing prices for zone {ZoneId}", zoneId);
            return Result.Failure(Resource.SthWentWrong);
        }    
    }
}
