using FuelMaster.HeadOffice.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Tanks
{
    public class TankRequest
    {
        [Required]
        public int StationId { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public int Number { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Capacity { get; set; }
        
        [Required, Range(0, double.MaxValue)]
        public decimal MaxLimit { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal MinLimit { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal CurrentLevel { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal CurrentVolume { get; set; }

        [Required]
        public bool HasSensor { get; set; }
    }
}
