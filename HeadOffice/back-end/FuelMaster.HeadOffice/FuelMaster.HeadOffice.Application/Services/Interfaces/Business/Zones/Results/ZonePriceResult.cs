using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;

public class ZonePriceResult
{
    public int Id { get; set; }
    public FuelTypeResult? FuelType { get; private set; }
    public int FuelTypeId { get; private set; }
    public decimal Price { get; private set; }
}
