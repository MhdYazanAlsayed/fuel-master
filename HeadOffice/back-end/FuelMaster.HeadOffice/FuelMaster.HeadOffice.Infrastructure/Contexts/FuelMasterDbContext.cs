using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Infrastructure.Contexts.QueryFilters;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts
{
    public class FuelMasterDbContext : IdentityDbContext<FuelMasterUser>
    {
        private readonly IDomainEventPublisher? _domainEventPublisher;
        private readonly ISigninService _signinService;

        public FuelMasterDbContext(DbContextOptions<FuelMasterDbContext> options, ISigninService signinService): base(options)
        {
            _signinService = signinService;
        }

        public FuelMasterDbContext(DbContextOptions<FuelMasterDbContext> options, ISigninService signinService, IDomainEventPublisher? domainEventPublisher = null) : base(options)
        {
            _domainEventPublisher = domainEventPublisher;
            _signinService = signinService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>().ApplyFilterAsync(
                 _signinService.GetCurrentScope() ?? throw new InvalidOperationException("Current scope is not set"),
                 _signinService.GetCurrentCityId(), 
                 _signinService.GetCurrentAreaId(), 
                 _signinService.GetCurrentStationId());
                 
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

        public DbSet<Area> Areas { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Nozzle> Nozzles { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZonePrice> ZonePrices { get; set; }
        public DbSet<ZonePriceHistory> ZonePriceHistory { get; set; }
        public DbSet<NozzleHistory> NozzleHistories { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<FuelMasterRole> FuelMasterRoles { get; set; }
        public DbSet<FuelMasterPermission> FuelMasterPermissions { get; set; }
        public DbSet<FuelMasterAreaOfAccess> FuelMasterAreasOfAccess { get; set; }
    }
}
