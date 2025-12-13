using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;

public class ChangePriceDto
{
    public List<ChangePricecDtoItem> Prices { get; set; } = null!;
}

public class ChangePricecDtoItem
{
    public int FuelTypeId { get; set; }

    public decimal Price { get; set; }
}