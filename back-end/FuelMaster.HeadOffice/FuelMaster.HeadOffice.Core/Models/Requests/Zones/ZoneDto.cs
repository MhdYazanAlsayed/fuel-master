using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Zones
{
    public class ZoneDto
    {
        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public string EnglishName { get; set; } = null!;
    }
}
