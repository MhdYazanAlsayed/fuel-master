using FuelMaster.HeadOffice.Core.Contracts.Markers;

namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface IMigration: ISingletonDependency
    {
        void ApplyMigrationsForAllTenants();
        Task ExecuteStoredProceduresForAllTenantsAsync();
        Task ExecuteSpecificProcedureForAllTenantsAsync(string procedureName, params object[] parameters);
    }
}
