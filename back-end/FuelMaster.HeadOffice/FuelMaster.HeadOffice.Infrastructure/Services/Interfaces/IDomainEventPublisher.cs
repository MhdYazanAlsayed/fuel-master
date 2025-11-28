using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces
{
    public interface IDomainEventPublisher : IScopedDependency
    {
        Task PublishAsync(IDomainEvent domainEvent);
        void Publish(IDomainEvent domainEvent);
    }
}

