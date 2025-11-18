using System;

namespace FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;

public class ChangePricesDto
{
    public int FuelTypeId { get; set; }
    public decimal Price { get; set; }
}

