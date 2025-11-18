using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Repositories.FuelTypes;

public class FuelTypeRepository : IFuelTypeRepository
{
    private readonly FuelMasterDbContext _context;
    private readonly ILogger<FuelTypeRepository> _logger;
    private readonly ICacheService _cacheService;

    public FuelTypeRepository(
        IContextFactory<FuelMasterDbContext> contextFactory,
        ILogger<FuelTypeRepository> logger,
        ICacheService cacheService)
    {
        _context = contextFactory.CurrentContext;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<FuelType>> GetAllAsync()
    {
        _logger.LogInformation("Getting all fuel types");

        var cachedFuelTypes = await _cacheService.GetAsync<IEnumerable<FuelType>>("FuelTypes_All");
        if (cachedFuelTypes != null)
        {
            _logger.LogInformation("Retrieved {Count} fuel types from cache", cachedFuelTypes.Count());
            return cachedFuelTypes;
        }

        _logger.LogInformation("Fuel types not in cache, fetching from database");

        var fuelTypes = await _context.FuelTypes.ToListAsync();
        
        await _cacheService.SetAsync("FuelTypes_All", fuelTypes);
        
        _logger.LogInformation("Cached {Count} fuel types", fuelTypes.Count);

        return fuelTypes;
    }

    public async Task<PaginationDto<FuelType>> GetPaginationAsync(int currentPage)
    {
        _logger.LogInformation("Getting paginated fuel types for page {Page}", currentPage);

        // Use GetAllAsync to leverage existing caching, then paginate in-memory
        var allFuelTypes = await GetAllAsync();

        var pagination = allFuelTypes.ToPagination(currentPage);

        _logger.LogInformation("Retrieved paginated fuel types for page {Page}", currentPage);

        return pagination;
    }

    public async Task<ResultDto<FuelType>> CreateAsync(FuelTypeDto dto)
    {
        _logger.LogInformation("Creating new fuel type with Arabic name: {ArabicName}, English name: {EnglishName}", 
            dto.ArabicName, dto.EnglishName);

        try
        {
            var fuelType = new FuelType(dto.ArabicName, dto.EnglishName);
            await _context.FuelTypes.AddAsync(fuelType);
            await _context.SaveChangesAsync();

            // Update caches incrementally
            await UpdateFuelTypesCacheAfterCreateAsync(fuelType);

            _logger.LogInformation("Successfully created fuel type with ID: {Id}", fuelType.Id);

            return Results.Success(fuelType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fuel type with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);
            return Results.Failure<FuelType>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<FuelType>> UpdateAsync(int id, FuelTypeDto dto)
    {
        _logger.LogInformation("Updating fuel type with ID: {Id}, Arabic name: {ArabicName}, English name: {EnglishName}", 
            id, dto.ArabicName, dto.EnglishName);

        try
        {
            var fuelType = await _context.FuelTypes.FindAsync(id);
            if (fuelType == null)
            {
                _logger.LogWarning("Fuel type with ID {Id} not found", id);
                return Results.Failure<FuelType>(Resource.EntityNotFound);
            }

            fuelType.Update(dto.ArabicName, dto.EnglishName);

            _context.FuelTypes.Update(fuelType);
            await _context.SaveChangesAsync();

            // Update caches incrementally
            await UpdateFuelTypesCacheAfterUpdateAsync(fuelType);

            _logger.LogInformation("Successfully updated fuel type with ID: {Id}", id);

            return Results.Success(fuelType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating fuel type with ID: {Id}", id);
            return Results.Failure<FuelType>(Resource.EntityNotFound);
        }
    }

    private async Task UpdateFuelTypesCacheAfterCreateAsync(FuelType newFuelType)
    {
        var cached = await _cacheService.GetAsync<IEnumerable<FuelType>>("FuelTypes_All");
        if (cached is null) return;

        var updatedList = cached.ToList();
        updatedList.Add(newFuelType);
        await _cacheService.SetAsync("FuelTypes_All", updatedList);
    }

    private async Task UpdateFuelTypesCacheAfterUpdateAsync(FuelType updatedFuelType)
    {
        var cached = await _cacheService.GetAsync<IEnumerable<FuelType>>("FuelTypes_All");
        if (cached is null) return;

        var updatedList = cached.ToList();
        var index = updatedList.FindIndex(f => f.Id == updatedFuelType.Id);
        if (index >= 0)
        {
            updatedList[index] = updatedFuelType;
            await _cacheService.SetAsync("FuelTypes_All", updatedList);
        }
    }
}
