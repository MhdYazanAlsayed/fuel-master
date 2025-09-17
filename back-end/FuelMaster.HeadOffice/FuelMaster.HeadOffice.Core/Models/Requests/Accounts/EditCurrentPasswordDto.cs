using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Accounts
{
    public class EditCurrentPasswordDto
    {
        [Required, MinLength(6)]
        public string CurrentPassword { get; set; } = null!;

        [Required , Compare(nameof(ConfirmNewPassword)), MinLength(6)]
        public string NewPassword { get; set; } = null!;

        [Required , MinLength(6)]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
