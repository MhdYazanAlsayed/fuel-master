using System;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.DTOs;

public class ChangePriceDto
{
    public List<ChangePricecDtoItem> Prices { get; set; } = null!;
}

public class ChangePricecDtoItem
{
    public int FuelTypeId { get; set; }

    public decimal Price { get; set; }
}