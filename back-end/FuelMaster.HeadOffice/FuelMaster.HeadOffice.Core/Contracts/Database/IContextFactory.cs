using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Core.Interfaces.Database
{
    public interface IContextFactory<T> : IScopedDependency where T : DbContext
    {
        T CurrentContext { get; }
        T CreateDbContext();
        T CreateDbContext(string tenantId);
        T CreateDbContextForTanent (string tenantId);
    }
}
