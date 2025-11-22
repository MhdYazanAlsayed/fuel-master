using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Core.Interfaces.Database
{
    public interface ISeeder : ITransientDependency
    {
        Task SeedAsync(string tenantId);
    }
}
