using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Seeders;

namespace FuelMaster.HeadOffice.ApplicationService.Database
{
    public class SeederService : ISeeder
    {
        private readonly TenantConfiguration _configuration;
        private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
        private readonly IUserManagerFactory _userManagerFactory;

        public SeederService(TenantConfiguration configuration ,
            IContextFactory<FuelMasterDbContext> contextFactory ,
            IUserManagerFactory userManagerFactory)
        {
            _configuration = configuration;
            _contextFactory = contextFactory;
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
                var createDefaultUserSeeder = 
                    new CreateDefaultUsersSeeder(userManager, context);

                await createDefaultUserSeeder.SeedAsync();
            }
     
        }
    }
}
