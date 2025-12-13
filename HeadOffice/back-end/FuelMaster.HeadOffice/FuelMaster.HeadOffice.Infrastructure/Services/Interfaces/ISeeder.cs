using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces
{
    public interface ISeeder : ITransientDependency
    {
        /// <summary>
        /// This is the order of the seeder in the execution sequence.
        /// It's impportant to run a seeder before another when the second depends on the data that the first seed.
        /// </summary>
        int Order { get; }
        Task SeedAsync(Guid tenantId);
    }
}
