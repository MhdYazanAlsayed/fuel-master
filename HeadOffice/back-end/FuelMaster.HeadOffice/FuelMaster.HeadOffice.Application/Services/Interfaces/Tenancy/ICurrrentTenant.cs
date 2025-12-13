using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;

/// <summary>
/// This interface to get the current tenant of this request.
/// It's a scoped service.
/// It will set by TenantMiddleware.
/// </summary>
public interface ICurrentTenant : IScopedDependency
{
    Guid TenantId { get; }
    string ConnectionString { get; }
}