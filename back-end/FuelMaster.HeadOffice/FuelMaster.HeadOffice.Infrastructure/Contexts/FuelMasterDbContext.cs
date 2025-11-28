using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts
{
    public class FuelMasterDbContext : IdentityDbContext<FuelMasterUser>
    {
        private readonly IDomainEventPublisher? _domainEventPublisher;

        public FuelMasterDbContext(DbContextOptions<FuelMasterDbContext> options): base(options)
        {
        }

        public FuelMasterDbContext(DbContextOptions<FuelMasterDbContext> options, IDomainEventPublisher? domainEventPublisher = null) : base(options)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(FuelMasterDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            PublishDomainEvents();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await PublishDomainEventsAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void PublishDomainEvents()
        {
            if (_domainEventPublisher == null)
                return;

            var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    _domainEventPublisher.Publish(domainEvent);
                }
            }
        }

        private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
        {
            if (_domainEventPublisher == null)
                return;

            var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _domainEventPublisher.PublishAsync(domainEvent);
                }
            }
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FuelMasterGroup> FuelMasterGroup { get; set; }
        public DbSet<Nozzle> Nozzles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZonePrice> ZonePrices { get; set; }
        public DbSet<ZonePriceHistory> ZonePriceHistory { get; set; }
        public DbSet<NozzleHistory> NozzleHistories { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
    }
}
