using FuelMaster.HeadOffice.Core.Contracts.Database;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public static class MigrationExtension
    {
        public static WebApplication ApplyMigrationForAllTenants (this WebApplication app)
        {
            var scop = app.Services.CreateScope();
            var migrationService = scop.ServiceProvider.GetService<IMigration>();

            if (migrationService is null)
                throw new NullReferenceException("migrationService was null");

            migrationService.ApplyMigrationsForAllTenants();

            return app;
        }

        public static async Task<WebApplication> ExecuteStoredProceduresForAllTenantsAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var migrationService = scope.ServiceProvider.GetService<IMigration>();

            if (migrationService is null)
                throw new NullReferenceException("migrationService was null");

            await migrationService.ExecuteStoredProceduresForAllTenantsAsync();

            return app;
        }

        public static async Task<WebApplication> ExecuteSpecificProcedureForAllTenantsAsync(this WebApplication app, string procedureName, params object[] parameters)
        {
            var scope = app.Services.CreateScope();
            var migrationService = scope.ServiceProvider.GetService<IMigration>();

            if (migrationService is null)
                throw new NullReferenceException("migrationService was null");

            await migrationService.ExecuteSpecificProcedureForAllTenantsAsync(procedureName, parameters);

            return app;
        }
    }
}
