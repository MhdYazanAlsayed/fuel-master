namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Delivery.Dtos;

public class DeliveryDto
{
    public string Transport { get; set; } = null!;

    public string InvoiceNumber { get; set; } = null!;

    public decimal PaidVolume { get; set; }

    public decimal ReceivedVolume { get; set; }

    public int TankId { get; set; }
}