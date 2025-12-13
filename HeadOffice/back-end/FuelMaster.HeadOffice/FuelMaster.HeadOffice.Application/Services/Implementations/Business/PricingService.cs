using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business;

public class PricingService : IPricingService
{
    private readonly ISigninService _signinService;
    private readonly ILogger<PricingService> _logger;
    private readonly IZoneRepository _zoneRepository;       
    private readonly IUnitOfWork _unitOfWork;
    public PricingService(
        ISigninService signinService, 
        IZoneRepository zoneRepository,
        IUnitOfWork unitOfWork,
        ILogger<PricingService> logger)
    {
        _signinService = signinService;
        _zoneRepository = zoneRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ZonePrice>> ChangePricesAsync(int zoneId, List<ChangePricesDto> dto)
    {
        // Extract fuel type IDs to avoid using dto.Any() in queries
        var fuelTypeIds = dto.Select(d => d.FuelTypeId).ToList();
        
        // Load Zone with related entities using Include for proper tracking
        // Note: Filtered ThenInclude may not be supported, so we load all and filter in memory
        var zone = await _zoneRepository.DetailsAsync(
            zoneId, 
            includePrices: true, 
            includeFuelType: true, 
            includeStations: true, 
            includeTanks: true, 
            includeNozzles: true);
        
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

        var userId = _signinService.GetCurrentUserId();
        if (userId is null) 
        {
            _logger.LogError("User is not authorized");
            throw new UnauthorizedAccessException();
        }

        var changedZonePrices = new List<ZonePrice>();
        await _unitOfWork.BeginTransactionAsync();
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
            
                _zoneRepository.Update(result.Zone);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return changedZonePrices;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error changing price for zone {ZoneId} with message : {Message}", zoneId, ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
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
