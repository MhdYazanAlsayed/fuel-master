using FuelMaster.HeadOffice.Core.Entities.Configs.Tanks.Events;
using MediatR;

namespace FuelMaster.HeadOffice.Infrastructure.DomainEvents.Handlers
{
    /// <summary>
    /// Example handler for TankVolumeUpdatedEvent.
    /// MediatR will automatically route TankVolumeUpdatedEvent to this handler
    /// without any type checking or conditions needed.
    /// </summary>
    public class TankVolumeUpdatedHandler : INotificationHandler<TankVolumeUpdatedEvent>
    {
        public Task Handle(TankVolumeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Handle the event directly - no type checking needed!
            // The notification parameter is already strongly typed as TankVolumeUpdatedEvent
            
            // Example: Send SignalR notification, update cache, log, etc.
            // await _hubContext.Clients.All.SendAsync("tank-volume-updated", notification);
            
            return Task.CompletedTask;
        }
    }
}

