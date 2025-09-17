using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Core.Contracts.Authentication
{
    public interface IUserManagerFactory: IScopedDependency
    {
        public UserManager<FuelMasterUser> CreateUserManager(string tenantId);
        UserManager<FuelMasterUser> CreateUserManager();
    }
}
