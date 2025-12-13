using System;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;

public class ZonePriceResult
{
    public FuelTypeResult? FuelType { get; private set; }
    public int FuelTypeId { get; private set; }
    public decimal Price { get; private set; }
}
