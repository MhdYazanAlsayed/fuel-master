using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;

namespace FuelMaster.HeadOffice.ApplicationService.Database
{
    public class MigrationService : IMigration
    {
        private readonly TenantConfiguration _configuration;
        private readonly string _proceduresPath;

        public MigrationService(TenantConfiguration configuration)
        {
            _configuration = configuration;
            _proceduresPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Procedures");
        }

        public void ApplyMigrationsForAllTenants()
        {
            foreach (var tenant in _configuration.Tenants)
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

        public async Task ExecuteStoredProceduresForAllTenantsAsync()
        {
            var procedureFiles = GetProcedureFiles();
            
            foreach (var tenant in _configuration.Tenants)
            {
                try
                {
                    await ExecuteProceduresForTenantAsync(tenant, procedureFiles);
                }
                catch (Exception ex)
                {
                    // Log the error but continue with other tenants
                    Console.WriteLine($"Error executing procedures for tenant {tenant.TenantId}: {ex.Message}");
                }
            }
        }

        private async Task ExecuteProceduresForTenantAsync(TenantItem tenant, IEnumerable<string> procedureFiles)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
            optionsBuilder.UseSqlServer(tenant.ConnectionString);

            using var context = new FuelMasterDbContext(optionsBuilder.Options);
            
            foreach (var procedureFile in procedureFiles)
            {
                try
                {
                    var procedureName = Path.GetFileNameWithoutExtension(procedureFile);
                    var procedureContent = await File.ReadAllTextAsync(procedureFile, Encoding.UTF8);
                    
                    // Execute the stored procedure creation script
                    await context.Database.ExecuteSqlRawAsync(procedureContent);
                    
                    Console.WriteLine($"Successfully executed procedure {procedureName} for tenant {tenant.TenantId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing procedure {Path.GetFileName(procedureFile)} for tenant {tenant.TenantId}: {ex.Message}");
                }
            }
        }

        private IEnumerable<string> GetProcedureFiles()
        {
            if (!Directory.Exists(_proceduresPath))
            {
                throw new DirectoryNotFoundException($"Procedures directory not found at: {_proceduresPath}");
            }

            return Directory.GetFiles(_proceduresPath, "*.sql", SearchOption.TopDirectoryOnly);
        }

        public async Task ExecuteSpecificProcedureForAllTenantsAsync(string procedureName, params object[] parameters)
        {
            foreach (var tenant in _configuration.Tenants)
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

        private async Task ExecuteSpecificProcedureForTenantAsync(TenantItem tenant, string procedureName, object[] parameters)
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
