using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts
{
    public class TenantDbContextFactory(TenantConfiguration _configuration)
    {
        public FuelMasterDbContext CreateDbContext(IHttpContextAccessor _httpContextAccessor)
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
                return new FuelMasterDbContext(optionsBuilder.Options);
            }

            throw new InvalidOperationException("Tenant ID not found in request header.");
        }

        public FuelMasterDbContext CreateDbContext (string tenantId)
        {
            var connectionString = GetConnectionStringForTenant(tenantId.ToString()!);
            if (connectionString is null)
                throw new NullReferenceException("Cannot find a connection string for this tenant .");

            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new FuelMasterDbContext(optionsBuilder.Options);
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
