using FuelMaster.HeadOffice.Application.Constants;
using FuelMaster.HeadOffice.Infrastructure.Configurations;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB
{
    public class ContextFactoryService : IContextFactory<FuelMasterDbContext>
    {
        // TODO : Call tanent service instead of getting it from configuration
        private readonly TenantConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDomainEventPublisher? _domainEventPublisher;
        private FuelMasterDbContext? _context;

        public ContextFactoryService(TenantConfiguration configuration , 
            IHttpContextAccessor httpContextAccessor,
            IDomainEventPublisher? domainEventPublisher = null)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _domainEventPublisher = domainEventPublisher;
        }

        public FuelMasterDbContext CurrentContext => 
            _context ?? (_context = CreateDbContext());

        // Returns new context with store it .
        public FuelMasterDbContext CreateDbContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext!.Items.TryGetValue(ConfigKeys.TanentId, out var tenantId))
            {
                if (tenantId is null || string.IsNullOrEmpty(tenantId.ToString()))
                    throw new NullReferenceException("tenantId was not found .");

                var connectionString = GetConnectionStringForTenant(tenantId.ToString()!);
                if (connectionString is null)
                    throw new NullReferenceException("Cannot find a connection string for this tenant .");

                var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                var context = new FuelMasterDbContext(optionsBuilder.Options, _domainEventPublisher);
                _context = context;
                return context;
            }

            throw new InvalidOperationException("Tenant ID not found in request header.");
        }

        // Returns new context without store it .
        public FuelMasterDbContext CreateDbContext(string tenantId)
        {
            var connectionString = GetConnectionStringForTenant(tenantId.ToString()!);
            if (connectionString is null)
                throw new NullReferenceException("Cannot find a connection string for this tenant .");

            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new FuelMasterDbContext(optionsBuilder.Options, _domainEventPublisher);
        }

        public FuelMasterDbContext CreateDbContextForTanent (string tenantId)
        {
            var connectionString = _configuration.Tenants.Single(x => x.TenantId == tenantId).ConnectionString;
            if (connectionString is null)
                throw new NullReferenceException("Cannot find a connection string for this tenant .");

            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new FuelMasterDbContext(optionsBuilder.Options, _domainEventPublisher);
        }

        private string? GetConnectionStringForTenant(string tenantId)
        {
            var tenantSetting = _configuration.Tenants.FirstOrDefault(x => x.TenantId == tenantId);

            if (tenantSetting is null)
                throw new Exception($"Cannot find tenant setting for '{tenantId}'");

            return tenantSetting.ConnectionString;
        }

    }
}
