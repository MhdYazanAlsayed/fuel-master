using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Stations
{
    public class StationRequest
    {
        [Required]
        public string EnglishName { get; set; } = null!;

        [Required]
        public string ArabicName { get; set; } = null!;

        [Required]
        public int CityId { get; set; }

        [Required]
        public int ZoneId { get; set; }
    }
}
