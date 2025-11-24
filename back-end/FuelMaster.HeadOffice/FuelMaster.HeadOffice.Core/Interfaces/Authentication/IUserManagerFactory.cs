using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Core.Interfaces.Authentication
{
    public interface IUserManagerFactory: IScopedDependency
    {
        public UserManager<FuelMasterUser> CreateUserManager(string tenantId);
        UserManager<FuelMasterUser> CreateUserManager();
    }
}
