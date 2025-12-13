using System.ComponentModel.DataAnnotations.Schema;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public abstract class AggregateRoot<T> : Base, IHasDomainEvents
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public AggregateRoot()
        {
            CreatedAt = DateTimeCulture.Now;
        }

        public T Id { get; set; } = default(T)!;

        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
