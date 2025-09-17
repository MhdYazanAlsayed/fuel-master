using FuelMaster.HeadOffice.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FuelMaster.HeadOffice.Core.Models.Requests.Zones
{
    public class ChangePriceDto
    {
        public List<ChangePricecDtoItem> Prices { get; set; } = null!;
    }

    public class ChangePricecDtoItem
    {
        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
