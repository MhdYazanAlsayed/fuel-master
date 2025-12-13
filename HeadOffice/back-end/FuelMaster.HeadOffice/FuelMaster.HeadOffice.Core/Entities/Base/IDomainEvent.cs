using MediatR;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }

    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}

