using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Cities;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class CityService : ICityService
    {
        private readonly FuelMasterDbContext _context;
        private readonly ILogger<CityService> _logger;
        private readonly ICacheService _cacheService;

        public CityService(IContextFactory<FuelMasterDbContext> contextFactory, 
            ILogger<CityService> logger, 
            ICacheService cacheService)
        {
            _context = contextFactory.CurrentContext;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<PaginationDto<City>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated cities for page {Page}", currentPage);

            var cacheKey = $"Cities_Page_{currentPage}";
            var cachedPagination = await _cacheService.GetAsync<PaginationDto<City>>(cacheKey);
            
            if (cachedPagination != null)
            {
                _logger.LogInformation("Retrieved paginated cities from cache for page {Page}", currentPage);
                return cachedPagination;
            }

            _logger.LogInformation("Paginated cities not in cache, fetching from database for page {Page}", currentPage);

            var pagination = await _context.Cities.ToPaginationAsync(currentPage);
            
            await _cacheService.SetAsync(cacheKey, pagination);
            
            _logger.LogInformation("Cached paginated cities for page {Page}", currentPage);

            return pagination;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            _logger.LogInformation("Getting all cities");

            var cachedCities = await _cacheService.GetAsync<IEnumerable<City>>("Cities_All");
            if (cachedCities != null)
            {
                _logger.LogInformation("Retrieved {Count} cities from cache", cachedCities.Count());
                return cachedCities;
            }

            _logger.LogInformation("Cities not in cache, fetching from database");

            var cities = await _context.Cities.ToListAsync();
            
            await _cacheService.SetAsync("Cities_All", cities);
            
            _logger.LogInformation("Cached {Count} cities", cities.Count);

            return cities;
        }

        public async Task<ResultDto<City>> CreateAsync(CityDto dto)
        {
            _logger.LogInformation("Creating new city with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);

            try
            {
                var city = new City(dto.ArabicName, dto.EnglishName);
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();

                // Invalidate cache after creating new city
                await InvalidateCitiesCacheAsync();
                
                _logger.LogInformation("Successfully created city with ID: {Id}", city.Id);

                return Results.Success(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating city with Arabic name: {ArabicName}, English name: {EnglishName}", 
                    dto.ArabicName, dto.EnglishName);
                return Results.Failure<City>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<City>> EditAsync(int id, CityDto dto)
        {
            _logger.LogInformation("Editing city with ID: {Id}, Arabic name: {ArabicName}, English name: {EnglishName}", 
                id, dto.ArabicName, dto.EnglishName);

            try
            {
                var city = await _context.Cities.FindAsync(id);

                if (city is null)
                {
                    _logger.LogWarning("City with ID {Id} not found", id);
                    return Results.Failure(Resource.EntityNotFound, city);
                }

                city.ArabicName = dto.ArabicName;
                city.EnglishName = dto.EnglishName;

                _context.Cities.Update(city);
                await _context.SaveChangesAsync();

                // Invalidate cache after editing city
                await InvalidateCitiesCacheAsync();
                
                _logger.LogInformation("Successfully updated city with ID: {Id}", id);

                return Results.Success(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing city with ID: {Id}", id);
                return Results.Failure<City>(Resource.EntityNotFound);
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

            // Invalidate cache after deleting city
            await InvalidateCitiesCacheAsync();
            
            _logger.LogInformation("Successfully deleted city with ID: {Id}", id);

            return Results.Success();
        }

        public async Task<City?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for city with ID: {Id}", id);

            var cacheKey = $"City_Details_{id}";
            var cachedCity = await _cacheService.GetAsync<City>(cacheKey);
            
            if (cachedCity != null)
            {
                _logger.LogInformation("Retrieved city details from cache for ID: {Id}", id);
                return cachedCity;
            }

            _logger.LogInformation("City details not in cache, fetching from database for ID: {Id}", id);

            var city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == id);
            
            if (city != null)
            {
                await _cacheService.SetAsync(cacheKey, city);
                _logger.LogInformation("Cached city details for ID: {Id}", id);
            }

            return city;
        }

        private async Task InvalidateCitiesCacheAsync()
        {
            await _cacheService.RemoveByPatternAsync("Cities");
            _logger.LogInformation("Invalidated all cities cache entries");
        }
    }
}
