using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Infrastructure.Seeders
{
    public class UserSeeder : ISeeder
    {
        private readonly TenantConfiguration _configuration;
        private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly ILogger<UserSeeder> _logger;

        public UserSeeder(TenantConfiguration configuration ,
            IContextFactory<FuelMasterDbContext> contextFactory ,
            ILogger<UserSeeder> logger,
            IUserManagerFactory userManagerFactory)
        {
            _configuration = configuration;
            _contextFactory = contextFactory;
            _logger = logger;
            _userManagerFactory = userManagerFactory;
        }

        public async Task SeedAsync()
        {
            foreach (var tenant in _configuration.Tenants) 
            {
                var context = _contextFactory.CreateDbContext(tenant.TenantId);
                var userManager = _userManagerFactory.CreateUserManager(tenant.TenantId);

                if (context is null || userManager is null)
                    throw new NullReferenceException();

                // Create Default Users Seeder
                _logger.LogInformation("Creating Default Users for {TenantId}", tenant.TenantId);
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == "Admin");
                if (user is not null) 
                {
                    _logger.LogInformation("User {UserName} already exists", user.UserName);
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user , "MyP@ssw0rd");

                    await userManager.UpdateAsync(user);
                    return;
                }

                _logger.LogInformation("User {UserName} does not exist", "Admim");

                var admin = new FuelMasterUser("Admin", true);
                var result = await userManager.CreateAsync(admin , "MyP@ssw0rd");

                if (!result.Succeeded)
                {
                    _logger.LogError("Error creating user {UserName} for {TenantId}", "Admin", tenant.TenantId);
                    _logger.LogError("Error: {ErrorMessage}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                _logger.LogInformation("User {UserName} created successfully for {TenantId}", "Admin", tenant.TenantId);
            }
     
        }
    }
}
