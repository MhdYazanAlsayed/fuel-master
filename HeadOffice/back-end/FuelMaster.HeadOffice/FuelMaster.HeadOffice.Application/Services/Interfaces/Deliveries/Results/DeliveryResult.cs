using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.Results;

public class DeliveryResult
{
    public int Id { get; set; }
    public string? Transport { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal PaidVolume { get; set; }
    public decimal RecivedVolume { get; set; }
    public decimal TankOldLevel { get; set; }
    public decimal TankNewLevel { get; set; }
    public decimal TankOldVolume { get; set; }
    public decimal TankNewVolume { get; set; }
    public decimal GL { get; private set; }
    public TankResult? Tank { get; set; }
}