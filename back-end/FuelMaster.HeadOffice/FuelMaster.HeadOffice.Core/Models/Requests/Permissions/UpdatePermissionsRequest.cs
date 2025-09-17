using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Permissions
{
    public class UpdatePermissionsRequest
    {
        [Required]
        public UpdatePermissionItem[] Permissions { get; set; } = null!;
    }

    public class UpdatePermissionItem
    {
        [Required]
        public string Key { get; set; } = null!;

        [Required]
        public bool Value { get; set; }
    }
}
