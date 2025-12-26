namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.DTOs;

public class DeliveryDto
{
    public required string Transport { get; set; }
    public required string InvoiceNumber { get; set; }
    public required decimal PaidVolume { get; set; }
    public required decimal RecivedVolume { get; set; }
    public required int TankId { get; set; }
}