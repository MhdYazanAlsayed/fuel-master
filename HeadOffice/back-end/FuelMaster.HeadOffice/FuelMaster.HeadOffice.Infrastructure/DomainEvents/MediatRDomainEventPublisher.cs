using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using MediatR;

namespace FuelMaster.HeadOffice.Infrastructure.DomainEvents
{
    public class MediatRDomainEventPublisher : IDomainEventPublisher, IScopedDependency
    {
        private readonly IMediator _mediator;

        public MediatRDomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            // Domain events now implement INotification directly, so we can publish them directly
            await _mediator.Publish(domainEvent);
        }

        public void Publish(IDomainEvent domainEvent)
        {
            // Domain events now implement INotification directly, so we can publish them directly
            _mediator.Publish(domainEvent).GetAwaiter().GetResult();
        }
    }
}

