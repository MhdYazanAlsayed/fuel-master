using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Accounts
{
    public class EditPasswordDto
    {
        [Required , Compare(nameof(ConfirmPassword))]
        public string Password { get; set; } = null!;

        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
