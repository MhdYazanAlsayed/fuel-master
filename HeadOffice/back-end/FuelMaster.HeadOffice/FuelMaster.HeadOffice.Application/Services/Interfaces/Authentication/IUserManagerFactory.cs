using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication
{
    public interface IUserManagerFactory: IScopedDependency
    {
        Task<UserManager<FuelMasterUser>> CreateUserManagerAsync(Guid tenantId);
        UserManager<FuelMasterUser> CreateUserManager();
        UserManager<FuelMasterUser> CreateUserManagerByContext<TContext>(TContext context) where TContext : DbContext;
    }
}
