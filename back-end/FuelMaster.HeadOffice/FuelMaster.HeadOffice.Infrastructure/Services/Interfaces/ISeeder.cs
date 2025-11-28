using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces
{
    public interface ISeeder : ITransientDependency
    {
        Task SeedAsync(string tenantId);
    }
}
