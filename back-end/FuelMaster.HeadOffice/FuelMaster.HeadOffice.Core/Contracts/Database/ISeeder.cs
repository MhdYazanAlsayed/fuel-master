using FuelMaster.HeadOffice.Core.Contracts.Markers;

namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface ISeeder : ITransientDependency
    {
        Task SeedAsync();
    }
}
