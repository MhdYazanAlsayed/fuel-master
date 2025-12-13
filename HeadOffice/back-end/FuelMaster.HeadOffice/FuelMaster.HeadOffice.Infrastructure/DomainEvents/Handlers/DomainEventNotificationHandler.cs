using FuelMaster.HeadOffice.Core.Entities;
using MediatR;

namespace FuelMaster.HeadOffice.Infrastructure.DomainEvents.Handlers
{
    /// <summary>
    /// Example handler demonstrating how to handle specific domain events.
    /// Create handlers for each domain event type by implementing INotificationHandler&lt;YourDomainEvent&gt;
    /// MediatR will automatically route the event to the correct handler based on the event type.
    /// 
    /// Example:
    /// public class TankVolumeUpdatedHandler : INotificationHandler&lt;TankVolumeUpdatedEvent&gt;
    /// {
    ///     public Task Handle(TankVolumeUpdatedEvent notification, CancellationToken cancellationToken)
    ///     {
    ///         // Handle the event directly - no type checking needed!
    ///         return Task.CompletedTask;
    ///     }
    /// }
    /// </summary>
    public class ExampleDomainEventHandler : INotificationHandler<IDomainEvent>
    {
        public Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        {
            // This is a generic handler for all domain events.
            // For specific event handling, create handlers like:
            // INotificationHandler<TankVolumeUpdatedEvent>
            // MediatR will automatically route TankVolumeUpdatedEvent to TankVolumeUpdatedHandler

            return Task.CompletedTask;
        }
    }
}

