using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Cities
{
    public class CityDto
    {
        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;
    }
}
