using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Application.Services.Implementations;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage)
    {
        var allCities = await GetAllAsync();
        var stations = await _stationRepository.GetAllAsync();
        
        var totalCount = allCities.Count();
        var pages = (int)Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(_paginationConfig.Length));

        var pageData = allCities
            .Skip(_paginationConfig.Length * (currentPage - 1))
            .Take(_paginationConfig.Length)
            .ToList();

        pageData = pageData.Select(c => 
        {
            c.CanDelete = stations == null || !stations.Any(s => s.City?.Id == c.Id);
            return c;
        }).ToList();

        var mappedPagination = new PaginationDto<CityResult>(pageData, pages);

        return mappedPagination;
    }

    public async Task<IEnumerable<CityResult>> GetAllAsync()
    {
        _logger.LogInformation("Getting all cities");

        var cachedCityEntities = await _cityCache.GetAllEntitiesAsync();
        if (cachedCityEntities != null)
        {
            _logger.LogInformation("Retrieved {Count} cities from cache", cachedCityEntities.Count());
            
            // Map entities to DTOs
            var mappedCities = cachedCityEntities.Select(_mapper.Map<CityResult>).ToList();
            return mappedCities;
        }

        _logger.LogInformation("Cities not in cache, fetching from database");

        var cities = await _context.Cities
            .AsNoTracking()
            .ToListAsync();
        
        // Cache entities
        await _cityCache.SetAllAsync(cities);
        
        _logger.LogInformation("Cached {Count} cities", cities.Count);

        // Map entities to DTOs
        var mappedCitiesFromDb = cities.Select(_mapper.Map<CityResult>).ToList();

        return mappedCitiesFromDb;
    }

    public async Task<ResultDto<CityResult>> CreateAsync(CityDto dto)
    {
        _logger.LogInformation("Creating new city with Arabic name: {ArabicName}, English name: {EnglishName}", 
            dto.ArabicName, dto.EnglishName);

        try
        {
            var city = new City(dto.ArabicName, dto.EnglishName);
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            var cityResult = _mapper.Map<CityResult>(city);

            // Update caches incrementally - cache entity, not DTO
            await _cityCache.UpdateCacheAfterCreateAsync(city);

            _logger.LogInformation("Successfully created city with ID: {Id}", city.Id);

            return Results.Success(cityResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating city with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);
            return Results.Failure<CityResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<CityResult>> EditAsync(int id, CityDto dto)
    {
        _logger.LogInformation("Editing city with ID: {Id}, Arabic name: {ArabicName}, English name: {EnglishName}", 
            id, dto.ArabicName, dto.EnglishName);

        try
        {
            var city = await _context.Cities.FindAsync(id);

            if (city is null)
            {
                _logger.LogWarning("City with ID {Id} not found", id);
                return Results.Failure<CityResult>(Resource.EntityNotFound);
            }

            city.Update(dto.ArabicName, dto.EnglishName);

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            var updated = _mapper.Map<CityResult>(city);

            // Update caches incrementally - cache entity, not DTO
            await _cityCache.UpdateCacheAfterEditAsync(city);

            _logger.LogInformation("Successfully updated city with ID: {Id}", id);

            return Results.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing city with ID: {Id}", id);
            return Results.Failure<CityResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting city with ID: {Id}", id);

        var city = await _context.Cities.FindAsync(id);

        if (city is null)
        {
            _logger.LogWarning("City with ID {Id} not found for deletion", id);
            return Results.Failure(Resource.EntityNotFound);
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        // Update caches incrementally
        await _cityCache.UpdateCacheAfterDeleteAsync(id);
        
        _logger.LogInformation("Successfully deleted city with ID: {Id}", id);

        return Results.Success();
    }

    public async Task<CityResult?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for city with ID: {Id}", id);

            var cachedCity = await _cityCache.GetAllEntitiesAsync();
            if (cachedCity != null)
            {
                _logger.LogInformation("Retrieved city details from cache for ID: {Id}", id);
                return _mapper.Map<CityResult?>(cachedCity.FirstOrDefault(c => c.Id == id));
            }

            return null;
        }
}