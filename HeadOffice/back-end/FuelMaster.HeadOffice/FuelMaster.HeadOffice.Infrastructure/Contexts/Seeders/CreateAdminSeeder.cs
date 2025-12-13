using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Infrastructure.Seeders
{
    public class CreateAdminSeeder : ISeeder
    {
        private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly ILogger<CreateAdminSeeder> _logger;

        public CreateAdminSeeder( 
            IContextFactory<FuelMasterDbContext> contextFactory ,
            ILogger<CreateAdminSeeder> logger,
            IUserManagerFactory userManagerFactory)
        {
            _contextFactory = contextFactory;
            _logger = logger;
            _userManagerFactory = userManagerFactory;
        }

        public int Order => 2;

        public async Task SeedAsync(Guid tenantId)
        {
            var context = await _contextFactory.CreateDbContextForTenantAsync(tenantId);
            var userManager =  _userManagerFactory.CreateUserManagerByContext(context);

            if (context is null || userManager is null)
                throw new NullReferenceException();

            // Create Default Users Seeder
            _logger.LogInformation("Creating Default Users for {TenantId}", tenantId);
            var user = await userManager.FindByNameAsync("admin");
            if (user is not null) 
            {
                return;
            }

            _logger.LogInformation("User {UserName} does not exist", "Admin");

            var transaction = await context.Database.BeginTransactionAsync();

            try 
            {
                var role = await context.FuelMasterRoles.FirstAsync();

                var employee = new Employee(
                    "System Adminstrator", 
                    "000-xxx-000", 
                    Scope.ALL, 
                    role.Id);
                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();
                
                var admin = new FuelMasterUser("Admin", true, employee.Id);
                var result = await userManager.CreateAsync(admin , "MyP@ssw0rd");

                if (!result.Succeeded)
                {
                    _logger.LogError("Error creating user {UserName} for {TenantId}", "Admin", tenantId);
                    _logger.LogError("Error: {ErrorMessage}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                _logger.LogInformation("User {UserName} created successfully for {TenantId}", "Admin", tenantId);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating user {UserName} for {TenantId}", "Admin", tenantId);
                _logger.LogError("Error: {ErrorMessage}", ex.Message);
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
