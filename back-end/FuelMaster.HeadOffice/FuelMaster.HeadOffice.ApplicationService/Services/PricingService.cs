using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Services;

public class PricingService : IPricingService
{
    private readonly ISigninService _signinService;
    private readonly ILogger<PricingService> _logger;
    private readonly FuelMasterDbContext _context;
    public PricingService(ISigninService signinService, ILogger<PricingService> logger, IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _signinService = signinService;
        _logger = logger;
        _context = contextFactory.CurrentContext;
    }

    public async Task<List<ZonePrice>> ChangePricesAsync(int zoneId, List<ChangePricesDto> dto)
    {
        // Extract fuel type IDs to avoid using dto.Any() in queries
        var fuelTypeIds = dto.Select(d => d.FuelTypeId).ToList();
        
        // Load Zone with related entities using Include for proper tracking
        // Note: Filtered ThenInclude may not be supported, so we load all and filter in memory
        var zone = await _context.Zones
            .Where(z => z.Id == zoneId)
            .Include(z => z.Prices.Where(p => fuelTypeIds.Contains(p.FuelTypeId)))
            .ThenInclude(p => p.FuelType)
            .Include(z => z.Stations!)
                .ThenInclude(s => s.Tanks!)
                    .ThenInclude(t => t.Nozzles!)
            .SingleOrDefaultAsync();
        
        if (zone is null)
        {
            _logger.LogError("Zone with id {ZoneId} not found", zoneId);
            throw new ZoneNotFoundException($"Zone with id {zoneId} not found");
        }
        
        // Project the data we need for processing
        var result = new 
        {
            Zone = zone,
            Prices = zone.Prices.Where(p => fuelTypeIds.Contains(p.FuelTypeId)),
            Stations = zone.Stations!.Select(s => new 
            {
                Tanks = s.Tanks!.Where(t => fuelTypeIds.Contains(t.FuelTypeId)).Select(t => new 
                {
                    FuelTypeId = t.FuelTypeId,
                    Nozzles = t.Nozzles!.Select(n => new 
                    {
                        Nozzle = n
                    })
                })
            })
        };

        var userId = _signinService.GetLoggedUserId();
        if (userId is null) 
        {
            _logger.LogError("User is not authorized");
            throw new UnauthorizedAccessException();
        }

        var changedZonePrices = new List<ZonePrice>();
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var item in dto)
            {
                var changed = result.Zone.ChangePrice(userId, item.FuelTypeId, item.Price);
                if (!changed) 
                {
                    _logger.LogInformation("Price for zone {ZoneId} with fuel type {FuelTypeId} is already {NewPrice}", zoneId, item.FuelTypeId, item.Price);
                    continue;
                }

                var nozzles = result.Stations!
                .SelectMany(station => station.Tanks!.Where(tank => tank.FuelTypeId == item.FuelTypeId).SelectMany(tank => tank.Nozzles!.Select(nozzle => nozzle.Nozzle)))
                .ToList();

                UpdateNozzlesPrices(nozzles, item.Price);

                _logger.LogInformation("Price changed for zone {ZoneId} with fuel type {FuelTypeId} to {NewPrice}", zoneId, item.FuelTypeId, item.Price);
            
                changedZonePrices.Add(result.Zone.Prices.Single(x => x.FuelTypeId == item.FuelTypeId));
            
                _context.Update(result.Zone);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return changedZonePrices;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error changing price for zone {ZoneId} with message : {Message}", zoneId, ex.Message);
            await transaction.RollbackAsync();
            throw;
        }
    }

    private void UpdateNozzlesPrices (List<Nozzle> nozzles, decimal newPrice)
    {
        foreach (var data in nozzles)
        {
            data.ChangePrice(newPrice);
        }
    }

}