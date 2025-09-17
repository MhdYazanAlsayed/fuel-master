using FuelMaster.HeadOffice.Core.Contracts.Markers;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Core.Contracts.Database
{
    public interface IContextFactory<T> : IScopedDependency where T : DbContext
    {
        T CurrentContext { get; }
        T CreateDbContext();
        T CreateDbContext(string tenantId);
        T CreateDbContextForTanent (string tenantId);
    }
}
