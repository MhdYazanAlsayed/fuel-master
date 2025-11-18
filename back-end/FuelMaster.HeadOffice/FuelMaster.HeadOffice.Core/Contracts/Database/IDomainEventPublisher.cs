using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IDomainEvent domainEvent);
        void Publish(IDomainEvent domainEvent);
    }
}

