using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntityCache<City> _cityCache;
    private readonly ILogger<CityService> _logger;
    private readonly IMapper _mapper;
    private readonly IStationRepository _stationRepository;
    public CityService(
    ICityRepository cityRepository, 
    IUnitOfWork unitOfWork,
    IEntityCache<City> cityCache, 
    IMapper mapper,
    ILogger<CityService> logger,
    IStationRepository stationRepository)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _cityCache = cityCache;
        _mapper = mapper;
        _logger = logger;
        _stationRepository = stationRepository;
    }

    public async Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage)
    {
        var allCities = await GetAllAsync();
        // TODO: Get station repository
        var stations = await _stationRepository.GetAllAsync();
        
        var result = allCities.ToPagination(currentPage);

        result.Data = result.Data.Select(c => 
        {
            c.CanDelete = stations == null || !stations.Any(s => s.City?.Id == c.Id);
            return c;
        }).ToList();

        return result;
    }

    public async Task<IEnumerable<CityResult>> GetAllAsync()
    {
        var cities = await GetWithCachedDataAsync();
        
        // Map entities to DTOs
        var mappedCitiesFromDb = cities.Select(_mapper.Map<CityResult>).ToList();

        return mappedCitiesFromDb;
    }

    public async Task<ResultDto<CityResult>> CreateAsync(CityDto dto)
    {
        try
        {
            var city = new City(dto.ArabicName, dto.EnglishName);
            _cityRepository.Create(city);
            await _unitOfWork.SaveChangesAsync();

            var cityResult = _mapper.Map<CityResult>(city);

            // Update caches incrementally - cache entity, not DTO
            await _cityCache.UpdateCacheAfterCreateAsync(city);

            _logger.LogInformation("Successfully created city with ID: {Id}", city.Id);

            return Result.Success(cityResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating city with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);
            return Result.Failure<CityResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<CityResult>> UpdateAsync(int id, CityDto dto)
    {
        try
        {
            var cities = await GetWithCachedDataAsync();
            var city = cities.FirstOrDefault(c => c.Id == id);
            if (city is null)
                return Result.Failure<CityResult>(Resource.EntityNotFound);

            city.Update(dto.ArabicName, dto.EnglishName);

            _cityRepository.Update(city);
            await _unitOfWork.SaveChangesAsync();

            var updated = _mapper.Map<CityResult>(city);

            // Update caches incrementally - cache entity, not DTO
            await _cityCache.UpdateCacheAfterEditAsync(city);

            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing city with ID: {Id}", id);
            return Result.Failure<CityResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        var cities = await GetWithCachedDataAsync();
        var city = cities.FirstOrDefault(c => c.Id == id);
        if (city is null)
            return Result.Failure(Resource.EntityNotFound);

        _cityRepository.Delete(city);
        await _unitOfWork.SaveChangesAsync();

        // Update caches incrementally
        await _cityCache.UpdateCacheAfterDeleteAsync(id);
        
        return Result.Success();
    }

    public async Task<CityResult?> DetailsAsync(int id)
    {
        var cities = await GetWithCachedDataAsync();

        return _mapper.Map<CityResult?>(cities.FirstOrDefault(x => x.Id == id));
    }

    public async Task<List<City>> GetWithCachedDataAsync()
    {
        var cachedCityEntities = await _cityCache.GetAllEntitiesAsync();
        if (cachedCityEntities != null)
        {
            return cachedCityEntities.ToList();
        }

        var cities = await _cityRepository.GetAllAsync();
        await _cityCache.SetAllAsync(cities);
        return cities.ToList();
    }
}