using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Core.Interfaces.Database
{
    public interface IMigration: ISingletonDependency
    {
        void ApplyMigrationsForAllTenants();
        Task ExecuteStoredProceduresForAllTenantsAsync();
        Task ExecuteSpecificProcedureForAllTenantsAsync(string procedureName, params object[] parameters);
    }
}
