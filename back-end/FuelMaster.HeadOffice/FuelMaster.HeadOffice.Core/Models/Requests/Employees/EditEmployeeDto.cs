using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Employees
{
    public class EditEmployeeDto
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string CardNumber { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public int GroupId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public short? Age { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? Address { get; set; }
        public int? StationId { get; set; }

    }
}
