using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class FuelMasterUser : IdentityUser
    {
        public FuelMasterUser(string userName , bool isActive , int? employeeId = null)
        {
            UserName = userName;
            EmployeeId = employeeId;
            IsActive = isActive;
        }

        public Employee? Employee { get; set; } 
        public int? EmployeeId { get; set; }

        public int? GroupId { get; set; }
        public FuelMasterGroup? Group { get; set; }

        public bool IsActive { get; set; }
    }
}
