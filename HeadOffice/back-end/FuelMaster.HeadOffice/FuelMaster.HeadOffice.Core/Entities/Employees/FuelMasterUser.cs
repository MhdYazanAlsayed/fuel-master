using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class FuelMasterUser : IdentityUser
    {
        protected FuelMasterUser() {}
        public FuelMasterUser(string userName , bool isActive, int? employeeId = null)
        {
            UserName = userName;
            //RoleId = roleId;
            EmployeeId = employeeId;
            IsActive = isActive;
        }

        public Employee? Employee { get; set; } 
        public int? EmployeeId { get; set; }

        //public FuelMasterRole? Role { get; private set; }
        //public int? RoleId { get; private set; }

        public bool IsActive { get; set; }
    }
}
