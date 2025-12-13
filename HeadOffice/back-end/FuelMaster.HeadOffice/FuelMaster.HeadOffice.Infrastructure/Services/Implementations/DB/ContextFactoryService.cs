using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB
{
    public class ContextFactoryService : IContextFactory<FuelMasterDbContext>
    {
        private readonly IDomainEventPublisher? _domainEventPublisher;
        private FuelMasterDbContext? _context;
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenants _tenants;
        public ContextFactoryService( 
            ICurrentTenant currentTenant,
            ITenants tenants,
            IDomainEventPublisher? domainEventPublisher = null)
        {
            _domainEventPublisher = domainEventPublisher;
            _currentTenant = currentTenant;
            _tenants = tenants;
        }

        public FuelMasterDbContext CurrentContext => 
            _context ?? (_context = CreateDbContext());

        private FuelMasterDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(_currentTenant.ConnectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            var context = new FuelMasterDbContext(optionsBuilder.Options, _domainEventPublisher);
            return context;
        }
    
        public async Task<FuelMasterDbContext> CreateDbContextForTenantAsync (Guid tenantId)
        {
            var tenant = await _tenants.GetTenantAsync(tenantId);

            var connectionString = tenant?.ConnectionString;
            if (connectionString is null)
                throw new NullReferenceException("Cannot find a connection string for this tenant .");

            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new FuelMasterDbContext(optionsBuilder.Options, _domainEventPublisher);
        }
    }
}
