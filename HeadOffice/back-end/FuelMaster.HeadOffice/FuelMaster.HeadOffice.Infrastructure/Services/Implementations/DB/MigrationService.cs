using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Infrastructure.Configurations;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB
{
    public class MigrationService : IMigration
    {
        private readonly ITenants _tenants;
        
        public MigrationService(ITenants tenants)
        {
            _tenants = tenants;
        }

        public void ApplyMigrationsForAllTenants()
        {
            foreach (var tenant in _tenants.GetAllTenantsAsync().Result)
            {
                var connectionString = tenant.ConnectionString;

                var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using (var context = new FuelMasterDbContext(optionsBuilder.Options))
                {
                    context.Database.Migrate();
                }
            }
        }
        
        public async Task ExecuteSpecificProcedureForAllTenantsAsync(string procedureName, params object[] parameters)
        {
            foreach (var tenant in _tenants.GetAllTenantsAsync().Result)
            {
                try
                {
                    await ExecuteSpecificProcedureForTenantAsync(tenant, procedureName, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing procedure {procedureName} for tenant {tenant.TenantId}: {ex.Message}");
                }
            }
        }

        private async Task ExecuteSpecificProcedureForTenantAsync(TenantConfig tenant, string procedureName, object[] parameters)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(tenant.ConnectionString);

            using var context = new FuelMasterDbContext(optionsBuilder.Options);
            
            // Build the parameter string for the stored procedure
            var parameterString = string.Join(", ", parameters.Select((_, index) => $"@p{index}"));
            var sql = $"EXEC {procedureName} {parameterString}";
            
            await context.Database.ExecuteSqlRawAsync(sql, parameters);
            
            Console.WriteLine($"Successfully executed procedure {procedureName} for tenant {tenant.TenantId}");
        }
    }
}
