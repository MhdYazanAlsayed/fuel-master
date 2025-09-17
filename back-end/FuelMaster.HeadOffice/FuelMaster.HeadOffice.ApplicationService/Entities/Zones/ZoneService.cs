using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Zones;
using FuelMaster.HeadOffice.Core.Models.Responses.ZonePriceHistories;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities.Zones
{
    public class ZoneService : IZoneService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IMapper _mapper;
        private readonly IZonePriceHistory _zonePriceHistory;

        public ZoneService(IContextFactory<FuelMasterDbContext> contextFactory, IMapper mapper,
            IZonePriceHistory zonePriceHistory)
        {
            _context = contextFactory.CurrentContext;
            _mapper = mapper;
            _zonePriceHistory = zonePriceHistory;
        }

        public async Task<IEnumerable<Zone>> GetAllAsync()
        {
            return await _context.Zones.ToListAsync();
        }

        public async Task<PaginationDto<Zone>> GetPaginationAsync(int currentPage)
        {
            return await _context.Zones.ToPaginationAsync(currentPage);
        }

        public async Task<ResultDto<Zone>> CreateAsync(ZoneDto dto)
        {
            try
            {
                var zone = new Zone(dto.ArabicName, dto.EnglishName);
                await _context.Zones.AddAsync(zone);
                await _context.SaveChangesAsync();

                return Results.Success(zone);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<Zone>(Resource.EntityNotFound);
            }
        }

        public async Task<ResultDto<Zone>> EditAsync(int id, ZoneDto dto)
        {
            try
            {
                var zone = await _context.Zones.FindAsync(id);

                if (zone == null)
                {
                    return Results.Failure<Zone>(Resource.EntityNotFound);
                }

                zone.ArabicName = dto.ArabicName;
                zone.EnglishName = dto.EnglishName;

                _context.Zones.Update(zone);
                await _context.SaveChangesAsync();

                return Results.Success(zone);
            }
            catch (NullReferenceException)
            {
                return Results.Failure<Zone>(Resource.EntityNotFound);
            }
        }

        public async Task<Zone?> DetailsAsync(int id)
        {
            return await _context.Zones.FindAsync(id);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var zone = await _context.Zones.FindAsync(id);

            if (zone == null)
            {
                return Results.Failure(Resource.EntityNotFound);
            }

            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<PaginationDto<ZonePriceHistoryPaginationResponse>> GetHistoriesAsync(int currentPage, int zonePrice)
        {
            var paginatedData = await _context.ZonePriceHistory
                .Include(x => x.User)
                .Include(x => x.User!.Employee)
                .Include(x => x.ZonePrice)
                .Where(x => x.ZonePriceId == zonePrice)
                .ToPaginationAsync(currentPage);

            var mappedData = _mapper.Map<List<ZonePriceHistoryPaginationResponse>>(paginatedData.Data);

            return new PaginationDto<ZonePriceHistoryPaginationResponse>(mappedData, paginatedData.Pages);
        }

        public async Task<ResultDto> ChangePriceAsync(int zoneId, ChangePriceDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                foreach (var item in dto.Prices)
                {
                    var zonePrice = await GetZonePriceAsync(zoneId, item.FuelType);
                    if (zonePrice is null)
                    {
                        await _context.ZonePrices
                            .AddAsync(new(zoneId, item.FuelType, item.Price));

                        continue;
                    }

                    if (zonePrice.Price == item.Price) continue;

                    var priceBeforeChange = zonePrice.Price;
                    UpdateZonePrice(zonePrice, item.Price);

                    await _zonePriceHistory.CreateAsync(new(zonePrice.Id, priceBeforeChange));

                    await UpdateNozzlesPrices(zonePrice);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch(ArgumentException)
            {
                await transaction.RollbackAsync();
                return Results.Failure(Resource.PriceShouldBeGreaterThanZero);
            }
            catch
            {
                await transaction.RollbackAsync();
                return Results.Failure();
            }

            return Results.Success();
        }

        public async Task<IEnumerable<ZonePrice>> GetPricesAsync(int zoneId)
        {
            return await _context.ZonePrices
                .Where(x => x.ZoneId == zoneId)
                .ToListAsync();
        }

        // Local Methods 
        private void UpdateZonePrice(ZonePrice zonePrice, decimal newPrice)
        {
            zonePrice.ChangePrice(newPrice);
            _context.ZonePrices.Update(zonePrice);
        }
        private async Task<ZonePrice?> GetZonePriceAsync(int zoneId, FuelType fuelType)
        {
            return await _context.ZonePrices
                  .SingleOrDefaultAsync(x => x.ZoneId == zoneId && x.FuelType == fuelType);
        }
        private async Task UpdateNozzlesPrices(ZonePrice zonePrice)
        {
            var stations = await _context.Stations
                .Include(x => x.Tanks)!
                .ThenInclude(x => x.Nozzles)
                .Where(x => x.ZoneId == zonePrice.ZoneId)
                .ToListAsync();

            var nozzles = stations
                .SelectMany(x => x.Tanks!)
                .Where(x => x.FuelType == zonePrice.FuelType)
                .SelectMany(x => x.Nozzles!)
                .ToList();

            foreach (var nozzle in nozzles)
            {
                nozzle.Price = zonePrice.Price;
                _context.Nozzles.Update(nozzle);
            }
        }
    }
}
