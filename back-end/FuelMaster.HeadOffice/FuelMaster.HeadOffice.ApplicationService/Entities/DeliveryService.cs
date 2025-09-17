using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class DeliveryService : IDeliveryService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IAuthorization _authorization;

        public DeliveryService(IContextFactory<FuelMasterDbContext> contextFactory, IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _authorization = authorization;
        }

        public async Task<ResultDto<Delivery>> CreateAsync(DeliveryDto dto)
        {
            var tank = await _context.Tanks
                .SingleOrDefaultAsync(x => x.Id == dto.TankId);
            if (tank is null)
                return new(false, null, Resource.CannotFindTank);

            if (tank.CurrentVolume + dto.ReceivedVolume > tank.MaxLimit)
                return new(false, null, Resource.TankHasNoSpace);

            var delivery = Delivery.Create(
                dto.Transport,
                dto.InvoiceNumber,
                dto.PaidVolume,
                dto.ReceivedVolume,
                dto.TankId,
                tank.CurrentLevel,
                tank.CurrentVolume
            );
            await _context.Deliveries.AddAsync(delivery);

            // Update tank level and volume
            tank.CurrentVolume += dto.ReceivedVolume;
            _context.Tanks.Update(tank);

            await _context.SaveChangesAsync();

            return new(true);
        }

        public void EditAsync(int id, DeliveryDto dto)
        {
            throw new NotImplementedException();
            //var delivery = await _context.Deliveries.FindAsync(id);

            //if (delivery != null)
            //{
            //    delivery.Transport = dto.Transport;
            //    delivery.InvoiceNumber = dto.InvoiceNumber;
            //    delivery.PaidVolume = dto.PaidVolume;
            //    delivery.RecivedVolume = dto.RecivedVolume;
            //    delivery.TankOldLevel = dto.TankOldLevel;
            //    delivery.TankNewLevel = dto.TankNewLevel;
            //    delivery.TankOldVolume = dto.TankOldVolume;
            //    delivery.TankNewVolume = dto.TankNewVolume;
            //    delivery.GL = dto.GL;
            //    delivery.TankId = dto.TankId;

            //    _context.Deliveries.Update(delivery);
            //    await _context.SaveChangesAsync();
            //}
        }

        public async Task<Delivery?> DetailsAsync(int id)
        {
            return await _context.Deliveries
                .Include(x => x.Tank)
                .Include(x => x.Tank!.Station)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ResultDto> DeleteAsync(int id)
        {
            var delivery = await _context.Deliveries
                .SingleOrDefaultAsync(x => x.Id == id);
            if (delivery is null) return Results.Failure();

            var tank = await _context.Tanks
                .SingleOrDefaultAsync(x => x.Id == delivery.TankId);
            if (tank is null) return Results.Failure();

            tank.CurrentVolume -= delivery.RecivedVolume;
            _context.Deliveries.Remove(delivery);

            await _context.SaveChangesAsync();

            return Results.Success();
        }

        public async Task<PaginationDto<Delivery>> GetPaginationAsync(GetDeliveriesPaginationDto dto)
        {
            if (dto.From is null) 
            {
                dto.From = DateTime.Now.AddDays(-10);
            }
            
            if (dto.To is null)
            {
                dto.To = DateTime.Now;
            }

            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;

            return await _context.Deliveries
                .Include(x => x.Tank)
                .Include(x => x.Tank!.Station)
                .Where(x => !dto.StationId.HasValue ||
                    (x.Tank != null && x.Tank.StationId == dto.StationId))
                .Where(x => !dto.From.HasValue || x.CreatedAt >= dto.From)
                .Where(x => !dto.To.HasValue || x.CreatedAt <= dto.To)
                .OrderByDescending(x => x.CreatedAt)
                .ToPaginationAsync(dto.Page);
        }
    }
}
