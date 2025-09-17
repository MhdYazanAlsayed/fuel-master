using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.FuelMasterGroups
{
    public class FuelMasterGroupDto
    {
        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;
    }
}
