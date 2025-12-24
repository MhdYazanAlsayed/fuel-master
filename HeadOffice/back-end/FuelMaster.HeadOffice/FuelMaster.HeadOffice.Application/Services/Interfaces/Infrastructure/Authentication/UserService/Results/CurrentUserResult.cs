using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;

namespace FuelMaster.HeadOffice.Application.DTOs.Authentication
{
    public class CurrentUserResult
    {
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Scope Scope { get; set; }
        public bool IsActive { get; set; }
        public List<string>? AreasOfAccess { get; set; }
        //public int? StationId { get; set; }
        //public int? AreaId { get; set; }
        //public int? CityId { get; set; }
    }
}
