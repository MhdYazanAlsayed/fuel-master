using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities.Zones;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Extenssions;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Entities.Zones
{
   public class ZoneRepository : IZoneRepository
   {
       private readonly FuelMasterDbContext _context;
       private readonly IMapper _mapper;
       private readonly IPricingService _pricingService;
       private readonly ILogger<ZoneRepository> _logger;
       private readonly ICacheService _cacheService;

       private readonly IFuelTypeRepository _fuelTypeRepository;
       public ZoneRepository(
           IContextFactory<FuelMasterDbContext> contextFactory, 
           IPricingService pricingService,
           IFuelTypeRepository fuelTypeRepository,
           IMapper mapper,
           ILogger<ZoneRepository> logger,
           ICacheService cacheService)
       {
           _context = contextFactory.CurrentContext;
           _mapper = mapper;
           _pricingService = pricingService;
           _logger = logger;
           _cacheService = cacheService;
           _fuelTypeRepository = fuelTypeRepository;
       }

       public async Task<IEnumerable<Zone>> GetAllAsync()
       {
           _logger.LogInformation("Getting all zones");

           var cachedZones = await _cacheService.GetAsync<IEnumerable<Zone>>("Zones_All");
           if (cachedZones != null)
           {
               _logger.LogInformation("Retrieved {Count} zones from cache", cachedZones.Count());
               return cachedZones;
           }

           _logger.LogInformation("Zones not in cache, fetching from database");

           var zones = await _context.Zones
           .Include(x => x.Prices)
           .ThenInclude(x => x.FuelType)
           .ToListAsync();
           
           await _cacheService.SetAsync("Zones_All", zones);
           
           _logger.LogInformation("Cached {Count} zones", zones.Count);

           return zones;
       }

       public async Task<PaginationDto<Zone>> GetPaginationAsync(int currentPage)
       {
           _logger.LogInformation("Getting paginated zones for page {Page}", currentPage);

           // Use GetAllAsync to leverage existing caching, then paginate in-memory
           var allZones = await GetAllAsync();

           var pagination = allZones.ToPagination(currentPage);

           _logger.LogInformation("Retrieved paginated zones for page {Page}", currentPage);

           return pagination;
       }

       public async Task<ResultDto<Zone>> CreateAsync(ZoneDto dto)
       {
           _logger.LogInformation("Creating new zone with Arabic name: {ArabicName}, English name: {EnglishName}", 
               dto.ArabicName, dto.EnglishName);

           try
           {
               var zone = new Zone(dto.ArabicName, dto.EnglishName);
               await _context.Zones.AddAsync(zone);
               await _context.SaveChangesAsync();

               // Update caches incrementally
               await UpdateZonesCacheAfterCreateAsync(zone);

               _logger.LogInformation("Successfully created zone with ID: {Id}", zone.Id);

               return Results.Success(zone);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error creating zone with Arabic name: {ArabicName}, English name: {EnglishName}", 
                   dto.ArabicName, dto.EnglishName);
               return Results.Failure<Zone>(Resource.EntityNotFound);
           }
       }

       public async Task<ResultDto<Zone>> EditAsync(int id, ZoneDto dto)
       {
           _logger.LogInformation("Editing zone with ID: {Id}, Arabic name: {ArabicName}, English name: {EnglishName}", 
               id, dto.ArabicName, dto.EnglishName);

           try
           {
               var zone = await _context.Zones.FindAsync(id);
               if (zone == null)
               {
                   _logger.LogWarning("Zone with ID {Id} not found", id);
                   return Results.Failure<Zone>(Resource.EntityNotFound);
               }

               zone.Update(dto.ArabicName, dto.EnglishName);

               _context.Zones.Update(zone);
               await _context.SaveChangesAsync();

               // Update caches incrementally
               await UpdateZonesCacheAfterEditAsync(zone);
               await _cacheService.SetAsync($"Zone_Details_{zone.Id}", zone);

               _logger.LogInformation("Successfully updated zone with ID: {Id}", id);

               return Results.Success(zone);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error editing zone with ID: {Id}", id);
               return Results.Failure<Zone>(Resource.EntityNotFound);
           }
       }

       public async Task<Zone?> DetailsAsync(int id)
       {
           _logger.LogInformation("Getting details for zone with ID: {Id}", id);

           var cacheKey = $"Zone_Details_{id}";
           var cachedZone = await _cacheService.GetAsync<Zone>(cacheKey);
           
           if (cachedZone != null)
           {
               _logger.LogInformation("Retrieved zone details from cache for ID: {Id}", id);
               return cachedZone;
           }

           _logger.LogInformation("Zone details not in cache, fetching from database for ID: {Id}", id);

           var zone = await _context.Zones.SingleOrDefaultAsync(x => x.Id == id);
           
           if (zone != null)
           {
               await _cacheService.SetAsync(cacheKey, zone);
               _logger.LogInformation("Cached zone details for ID: {Id}", id);
               return zone;
           }

           return null;
       }

       public async Task<ResultDto> DeleteAsync(int id)
       {
           _logger.LogInformation("Deleting zone with ID: {Id}", id);

           var zone = await _context.Zones.FindAsync(id);
           if (zone == null)
           {
               _logger.LogWarning("Zone with ID {Id} not found for deletion", id);
               return Results.Failure(Resource.EntityNotFound);
           }

           _context.Zones.Remove(zone);
           await _context.SaveChangesAsync();

           // Update caches incrementally
           await UpdateZonesCacheAfterDeleteAsync(id);
           await _cacheService.RemoveAsync($"Zone_Details_{id}");
           
           _logger.LogInformation("Successfully deleted zone with ID: {Id}", id);

           return Results.Success();
       }

       public async Task<PaginationDto<ZonePriceHistoryPaginationResult>> GetHistoriesAsync(int currentPage, int zonePrice)
       {
            var histories = await _context.ZonePriceHistory
                .Include(x => x.User)
                .Include(x => x.User!.Employee)
                .Include(x => x.ZonePrice)
                .ThenInclude(x => x!.FuelType)
                .Where(x => x.ZonePriceId == zonePrice)
                .OrderByDescending(x => x.CreatedAt)
                .ToPaginationAsync(currentPage);

            var data = _mapper.Map<List<ZonePriceHistoryPaginationResult>>(histories.Data);

           return new PaginationDto<ZonePriceHistoryPaginationResult>(data, histories.Pages);
       }

       public async Task<ResultDto> ChangePriceAsync(int zoneId, ChangePriceDto dto)
       {
           _logger.LogInformation("Changing prices for zone {ZoneId}", zoneId);

           try
           {
                var mappedPrices = _mapper.Map<List<ChangePricesDto>>(dto.Prices);
                var changedZonePrices = await _pricingService.ChangePricesAsync(zoneId, mappedPrices);

                foreach (var zonePrice in changedZonePrices)
                {
                    if (zonePrice.Zone is null)
                        throw new ZoneNotFoundException($"Zone with id {zoneId} not found");

                    await UpdateZonesCacheAfterEditAsync(zonePrice.Zone);
                }

                return Results.Success();
           }
           catch (ZoneNotFoundException)
           {
               return Results.Failure(Resource.EntityNotFound);
           }
           catch (UnauthorizedAccessException)
           {
               return Results.Failure(Resource.Unauthorized);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error changing prices for zone {ZoneId}", zoneId);
               return Results.Failure(Resource.SthWentWrong);
           }
       }

       public async Task<IEnumerable<ZonePrice>> GetPricesAsync(int zoneId)
       {
            var fuelTypes = await _fuelTypeRepository.GetAllAsync();

            var zone = (await GetAllAsync()).SingleOrDefault(x => x.Id == zoneId);

            if (zone is null)
                throw new ZoneNotFoundException($"Zone with id {zoneId} not found");

            var isThereAnyPriceMissing = fuelTypes.Any(x => !zone.Prices.Any(p => p.FuelTypeId == x.Id));
            if (isThereAnyPriceMissing)
            {
                foreach (var fuelType in fuelTypes)
                {
                    zone.InitializePrice(fuelType.Id);
                }

                _context.Update(zone);
                await _context.SaveChangesAsync();

                await UpdateZonesCacheAfterEditAsync(zone);

                await _context.Entry(zone)
                    .Collection(z => z.Prices)
                    .Query()
                    .Include(p => p.FuelType)
                    .LoadAsync();
            }

            return zone.Prices;
       }

       private async Task UpdateZonesCacheAfterCreateAsync(Zone newZone)
       {
           var cached = await _cacheService.GetAsync<IEnumerable<Zone>>("Zones_All");
           if (cached is null) return;

           var updatedList = cached.ToList();
           updatedList.Add(newZone);
           await _cacheService.SetAsync("Zones_All", updatedList);
       }

       private async Task UpdateZonesCacheAfterEditAsync(Zone updatedZone)
       {
           var cached = await _cacheService.GetAsync<IEnumerable<Zone>>("Zones_All");
           if (cached is null) return;

           var updatedList = cached.ToList();
           var index = updatedList.FindIndex(z => z.Id == updatedZone.Id);
           if (index >= 0)
           {
               updatedList[index] = updatedZone;
               await _cacheService.SetAsync("Zones_All", updatedList);
           }
       }

       private async Task UpdateZonesCacheAfterDeleteAsync(int id)
       {
           var cached = await _cacheService.GetAsync<IEnumerable<Zone>>("Zones_All");
           if (cached is null) return;

           var updatedList = cached.Where(z => z.Id != id).ToList();
           await _cacheService.SetAsync("Zones_All", updatedList);
       }
   }
}
