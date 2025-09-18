using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.FuelMasterGroups;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class FuelMasterGroupService : IFuelMasterGroupService
    {
        private readonly FuelMasterDbContext _context;
        private readonly ILogger<FuelMasterGroupService> _logger;
        private readonly ICacheService _cacheService;

        public FuelMasterGroupService(IContextFactory<FuelMasterDbContext> contextFactory, 
            ILogger<FuelMasterGroupService> logger, 
            ICacheService cacheService)
        {
            _context = contextFactory.CurrentContext;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<FuelMasterGroup>> GetAllAsync()
        {
            _logger.LogInformation("Getting all FuelMaster groups");

            var cachedGroups = await _cacheService.GetAsync<IEnumerable<FuelMasterGroup>>("FuelMasterGroups_All");
            if (cachedGroups != null)
            {
                _logger.LogInformation("Retrieved {Count} groups from cache", cachedGroups.Count());
                return cachedGroups;
            }

            _logger.LogInformation("Groups not in cache, fetching from database");

            var groups = await _context.FuelMasterGroup.ToListAsync();
            
            await _cacheService.SetAsync("FuelMasterGroups_All", groups);
            
            _logger.LogInformation("Cached {Count} groups", groups.Count);

            return groups;
        }

        public async Task<PaginationDto<FuelMasterGroup>> GetPaginationAsync(int currentPage)
        {
            _logger.LogInformation("Getting paginated groups for page {Page}", currentPage);

            var cacheKey = $"FuelMasterGroups_Page_{currentPage}";
            var cachedPagination = await _cacheService.GetAsync<PaginationDto<FuelMasterGroup>>(cacheKey);
            
            if (cachedPagination != null)
            {
                _logger.LogInformation("Retrieved paginated groups from cache for page {Page}", currentPage);
                return cachedPagination;
            }

            _logger.LogInformation("Paginated groups not in cache, fetching from database for page {Page}", currentPage);

            var pagination = await _context.FuelMasterGroup.ToPaginationAsync(currentPage);
            
            await _cacheService.SetAsync(cacheKey, pagination);
            
            _logger.LogInformation("Cached paginated groups for page {Page}", currentPage);

            return pagination;
        }

        public async Task<ResultDto<FuelMasterGroup>> CreateAsync(FuelMasterGroupDto dto)
        {
            _logger.LogInformation("Creating new group with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);

            try
            {
                var fuelMasterGroup = new FuelMasterGroup(dto.ArabicName, dto.EnglishName);
                await _context.FuelMasterGroup.AddAsync(fuelMasterGroup);
                await _context.SaveChangesAsync();

                // Invalidate cache after creating new group
                await _cacheService.RemoveAllFuelMasterGroupsAsync();
                
                _logger.LogInformation("Successfully created group with ID: {Id}", fuelMasterGroup.Id);

                return Results.Success(fuelMasterGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating group with Arabic name: {ArabicName}, English name: {EnglishName}", 
                    dto.ArabicName, dto.EnglishName);
                return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<FuelMasterGroup>> EditAsync(int id, FuelMasterGroupDto dto)
        {
            _logger.LogInformation("Editing group with ID: {Id}, Arabic name: {ArabicName}, English name: {EnglishName}", 
                id, dto.ArabicName, dto.EnglishName);

            try
            {
                var fuelMasterGroup = await _context.FuelMasterGroup.FindAsync(id);

                if (fuelMasterGroup == null)
                {
                    _logger.LogWarning("Group with ID {Id} not found", id);
                    return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
                }

                fuelMasterGroup.ArabicName = dto.ArabicName;
                fuelMasterGroup.EnglishName = dto.EnglishName;

                _context.FuelMasterGroup.Update(fuelMasterGroup);
                await _context.SaveChangesAsync();

                // Invalidate cache after editing group
                await _cacheService.RemoveAllFuelMasterGroupsAsync();
                
                _logger.LogInformation("Successfully updated group with ID: {Id}", id);

                return Results.Success(fuelMasterGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing group with ID: {Id}", id);
                return Results.Failure<FuelMasterGroup>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting group with ID: {Id}", id);

            var fuelMasterGroup = await _context.FuelMasterGroup.FindAsync(id);

            if (fuelMasterGroup == null)
            {
                _logger.LogWarning("Group with ID {Id} not found for deletion", id);
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.FuelMasterGroup.Remove(fuelMasterGroup);
            await _context.SaveChangesAsync();

            // Invalidate cache after deleting group
            await _cacheService.RemoveAllFuelMasterGroupsAsync();
            
            _logger.LogInformation("Successfully deleted group with ID: {Id}", id);

            return Results.Success();
        }

        public async Task<FuelMasterGroup?> DetailsAsync(int id)
        {
            _logger.LogInformation("Getting details for group with ID: {Id}", id);

            var cacheKey = $"FuelMasterGroup_Details_{id}";
            var cachedGroup = await _cacheService.GetAsync<FuelMasterGroup>(cacheKey);
            
            if (cachedGroup != null)
            {
                _logger.LogInformation("Retrieved group details from cache for ID: {Id}", id);
                return cachedGroup;
            }

            _logger.LogInformation("Group details not in cache, fetching from database for ID: {Id}", id);

            var group = await _context.FuelMasterGroup.SingleOrDefaultAsync(x => x.Id == id);
            
            if (group != null)
            {
                await _cacheService.SetAsync(cacheKey, group);
                _logger.LogInformation("Cached group details for ID: {Id}", id);
            }

            return group;
        }

    }
}
