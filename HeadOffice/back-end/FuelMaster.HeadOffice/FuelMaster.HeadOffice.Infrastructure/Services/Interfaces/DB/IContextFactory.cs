using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// This interface is used to create a new context for the current tenant.
    /// It's used to get the current context from the memory.
    /// It's also used to create a new context for a specific tenant.
    /// </summary>
    /// <typeparam name="T">The type of the context to create.</typeparam>
    public interface IContextFactory<T> : IScopedDependency where T : DbContext
    {
        /// <summary>
        /// It actually used the CreateDbContext method to create a new context for the current tenant.
        /// It's used to get the current context from the memory.
        /// </summary>
        T CurrentContext { get; }

        /// <summary>
        /// This method is used to create a new context for a specific tenant.
        /// It's used when we need to create a new context for a specific tenant without storing it in the memory.
        /// </summary>
        /// <param name="tenantId">The tenant id that came in request header or jwt claims.</param>
        /// <returns>FuelMasterDbContext</returns>
        Task<T> CreateDbContextForTenantAsync (Guid tenantId);
    }
}
