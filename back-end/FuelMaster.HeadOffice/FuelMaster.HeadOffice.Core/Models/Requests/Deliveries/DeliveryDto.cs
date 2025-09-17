namespace FuelMaster.HeadOffice.Core.Models.Requests.Deliveries
{
    using System.ComponentModel.DataAnnotations;

    public class DeliveryDto
    {
        [Required]
        public string Transport { get; set; } = null!;

        [Required]
        public string InvoiceNumber { get; set; } = null!;

        [Required]
        public decimal PaidVolume { get; set; }

        [Required]
        public decimal ReceivedVolume { get; set; }

        [Required]
        public int TankId { get; set; }
    }

}
