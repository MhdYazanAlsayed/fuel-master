namespace FuelMaster.HeadOffice.Core.Models.Dtos.Zones
{
    public class CreateHistoryDto(int zonePriceId , decimal price)
    {
        public int ZonePriceId { get; set; } = zonePriceId;
        public decimal Price { get; set; } = price;
    }
}
