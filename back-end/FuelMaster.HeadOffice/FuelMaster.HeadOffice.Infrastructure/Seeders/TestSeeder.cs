using FuelMaster.HeadOffice.Core.Contracts.Database;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Infrastructure.Seeders
{
    public class TestSeeder : ISeeder
    {
        private readonly ILogger<TestSeeder> _logger;

        public TestSeeder(ILogger<TestSeeder> logger)
        {
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("TestSeeder: Starting test seeding process");
            _logger.LogInformation("TestSeeder: Executing test seeder");
            
            // Simulate some work
            await Task.Delay(100);
            
            _logger.LogInformation("TestSeeder: Test seeding completed successfully");
            _logger.LogInformation("TestSeeder: Test seeder execution completed");
        }
    }
}
