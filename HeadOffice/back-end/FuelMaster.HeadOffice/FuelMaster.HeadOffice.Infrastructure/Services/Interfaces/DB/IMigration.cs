using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces
{
    public interface IMigration: IScopedDependency
    {
        /// <summary>
        /// This method is used to apply migrations for all tenants.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the request fails.</exception>
        void ApplyMigrationsForAllTenants();

        /// <summary>
        /// This method is used to execute a specific procedure for all tenants.
        /// </summary>
        /// <param name="procedureName">The name of the procedure to execute.</param>
        /// <param name="parameters">The parameters to pass to the procedure.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the request fails.</exception>
        Task ExecuteSpecificProcedureForAllTenantsAsync(string procedureName, params object[] parameters);
    }
}
