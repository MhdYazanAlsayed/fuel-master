using System;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;

public class ChangePricesDto
{
    public int FuelTypeId { get; set; }
    public decimal Price { get; set; }
}

