using AutoMapper;
using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class CityRepository : ICityRepository
    {
        private readonly FuelMasterDbContext _context;
        private readonly ILogger<CityRepository> _logger;
        private readonly ICacheService _cacheService;
        private readonly PaginationConfiguration _paginationConfig;

        private readonly IMapper _mapper;

        public CityRepository(
            IContextFactory<FuelMasterDbContext> contextFactory, 
            ILogger<CityRepository> logger, 
            ICacheService cacheService, 
            IMapper mapper,
            PaginationConfiguration paginationConfig)
        {
            _context = contextFactory.CurrentContext;
            _logger = logger;
            _cacheService = cacheService;
            _paginationConfig = paginationConfig;
            _mapper = mapper;
        }

        public async Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated cities for page {Page}", currentPage);

            // Use GetAllAsync to leverage existing caching and mapping, then paginate in-memory
            var allCities = await GetAllAsync();

            var totalCount = allCities.Count();
            var pages = (int)Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(_paginationConfig.Length));

            var pageData = allCities
                .Skip(_paginationConfig.Length * (currentPage - 1))
                .Take(_paginationConfig.Length)
                .ToList();

            var mappedPagination = new PaginationDto<CityResult>(pageData, pages);

            _logger.LogInformation("Cached paginated cities for page {Page}", currentPage);

            return mappedPagination;
        }

        public async Task<IEnumerable<CityResult>> GetAllAsync()
        {
            _logger.LogInformation("Getting all cities");

            var cachedCities = await _cacheService.GetAsync<IEnumerable<CityResult>>("Cities_All");
            if (cachedCities != null)
            {
                _logger.LogInformation("Retrieved {Count} cities from cache", cachedCities.Count());
                return cachedCities;
            }

            _logger.LogInformation("Cities not in cache, fetching from database");

            var cities = await _context.Cities.ToListAsync();
            var mappedCities = cities.Select(_mapper.Map<CityResult>).ToList();
            
            await _cacheService.SetAsync("Cities_All", mappedCities);
            
            _logger.LogInformation("Cached {Count} cities", mappedCities.Count);

            return mappedCities;
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

                // Update caches incrementally
                await UpdateCitiesCacheAfterCreateAsync(cityResult);

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

                // Update caches incrementally
                await UpdateCitiesCacheAfterEditAsync(updated);
                await _cacheService.SetAsync($"City_Details_{city.Id}", updated);

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
            await UpdateCitiesCacheAfterDeleteAsync(id);
            await _cacheService.RemoveAsync($"City_Details_{id}");
            
            _logger.LogInformation("Successfully deleted city with ID: {Id}", id);

            return Results.Success();
        }

        public async Task<CityResult?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for city with ID: {Id}", id);

            var cacheKey = $"City_Details_{id}";
            var cachedCity = await _cacheService.GetAsync<CityResult>(cacheKey);
            
            if (cachedCity != null)
            {
                _logger.LogInformation("Retrieved city details from cache for ID: {Id}", id);
                return cachedCity;
            }

            _logger.LogInformation("City details not in cache, fetching from database for ID: {Id}", id);

            var city = await _context.Cities.SingleOrDefaultAsync(x => x.Id == id);
            
            if (city != null)
            {
                var cityResult = _mapper.Map<CityResult>(city);
                await _cacheService.SetAsync(cacheKey, cityResult);
                _logger.LogInformation("Cached city details for ID: {Id}", id);
                return cityResult;
            }

            return null;
        }

        private async Task UpdateCitiesCacheAfterCreateAsync(CityResult newCity)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<CityResult>>("Cities_All");
            if (cached is null) return;

            var updatedList = cached.ToList();
            updatedList.Add(newCity);
            await _cacheService.SetAsync("Cities_All", updatedList);
        }

        private async Task UpdateCitiesCacheAfterEditAsync(CityResult updatedCity)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<CityResult>>("Cities_All");
            if (cached is null) return;

            var updatedList = cached.ToList();
            var index = updatedList.FindIndex(c => c.Id == updatedCity.Id);
            if (index >= 0)
            {
                updatedList[index] = updatedCity;
                await _cacheService.SetAsync("Cities_All", updatedList);
            }
        }

        private async Task UpdateCitiesCacheAfterDeleteAsync(int id)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<CityResult>>("Cities_All");
            if (cached is null) return;

            var updatedList = cached.Where(c => c.Id != id).ToList();
            await _cacheService.SetAsync("Cities_All", updatedList);
        }
    }
}
