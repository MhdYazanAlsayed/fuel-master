using FuelMaster.HeadOffice.Core.Entities.Zones.Events;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.DomainEvents.Handlers.Zone;

public class ZonePriceChangedHandler : INotificationHandler<ZonePriceChangedEvent>
{
    private readonly FuelMasterDbContext _context;
    public ZonePriceChangedHandler(FuelMasterDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ZonePriceChangedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // // Get all nozzles in that zone
        // var nozzles = await _context.Nozzles
        // .Include(x => x.Tank)
        // .Include(x => x.Tank!.Station)
        // .Where(x => x.Tank!.Station!.ZoneId == notification.ZoneId)
        // .ToListAsync();

        // var transaction = await _context.Database.BeginTransactionAsync();
        // try
        // {
        //     foreach (var nozzle in nozzles)
        //     {
        //         nozzle.ChangePrice(notification.NewPrice);
        //         _context.Nozzles.Update(nozzle);
        //     }

        //     await _context.SaveChangesAsync();
        //     await transaction.CommitAsync();

        // }
        // catch (Exception)
        // {
        //     await transaction.RollbackAsync();
        // }

    }
}