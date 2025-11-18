namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Dtos;

public class ChangePriceDto
{
    public List<ChangePricecDtoItem> Prices { get; set; } = null!;
}

public class ChangePricecDtoItem
{
    public int FuelTypeId { get; set; }

    public decimal Price { get; set; }
}