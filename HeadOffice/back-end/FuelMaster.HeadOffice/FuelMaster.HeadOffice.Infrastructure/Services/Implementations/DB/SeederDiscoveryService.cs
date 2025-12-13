using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces.DB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB
{
    public class SeederDiscoveryService : ISeederDiscovery
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeederDiscoveryService> _logger;
        private readonly ITenants _tenants;

        public SeederDiscoveryService(
            IServiceProvider serviceProvider, 
            ITenants tenants,
            ILogger<SeederDiscoveryService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _tenants = tenants;
        }

        public async Task ExecuteAllSeedersAsync()
        {
            // Get all services that implement ISeeder
            var seeders = _serviceProvider.GetServices<ISeeder>().ToList();

            if (!seeders.Any())
            {
                _logger.LogWarning("No seeders found to execute");
                return;
            }

            _logger.LogInformation("Found {Count} seeders to execute", seeders.Count());

            var tenants = await _tenants.GetAllTenantsAsync();
            foreach (var tenant in tenants)
            {
                foreach (var seeder in seeders.OrderBy(x => x.Order))
                {
                    try
                    {
                        var seederType = seeder.GetType().Name;
                        _logger.LogInformation("Executing seeder: {SeederType}", seederType);
                        
                        await seeder.SeedAsync(tenant.TenantId);
                        
                        _logger.LogInformation("Successfully executed seeder: {SeederType}", seederType);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing seeder: {SeederType}", seeder.GetType().Name);
                        throw; // Re-throw to stop execution if a seeder fails
                    }
                }
            }
            

            _logger.LogInformation("All seeders executed successfully");
        }
    }
}
