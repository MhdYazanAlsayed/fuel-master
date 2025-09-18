using FuelMaster.HeadOffice.Core.Contracts.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.ApplicationService.Database
{
    public class SeederDiscoveryService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeederDiscoveryService> _logger;

        public SeederDiscoveryService(IServiceProvider serviceProvider, ILogger<SeederDiscoveryService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task ExecuteAllSeedersAsync()
        {
            try
            {
                // Get all services that implement ISeeder
                var seeders = _serviceProvider.GetServices<ISeeder>();
                
                if (!seeders.Any())
                {
                    _logger.LogWarning("No seeders found to execute");
                    return;
                }

                _logger.LogInformation("Found {Count} seeders to execute", seeders.Count());

                foreach (var seeder in seeders)
                {
                    try
                    {
                        var seederType = seeder.GetType().Name;
                        _logger.LogInformation("Executing seeder: {SeederType}", seederType);
                        
                        await seeder.SeedAsync();
                        
                        _logger.LogInformation("Successfully executed seeder: {SeederType}", seederType);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing seeder: {SeederType}", seeder.GetType().Name);
                        throw; // Re-throw to stop execution if a seeder fails
                    }
                }

                _logger.LogInformation("All seeders executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during seeder execution");
                throw;
            }
        }
    }
}
