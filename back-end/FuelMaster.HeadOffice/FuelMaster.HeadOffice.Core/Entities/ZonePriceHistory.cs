namespace FuelMaster.HeadOffice.Core.Entities
{
    public class ZonePriceHistory : EntityBase<int>
    {
        public int ZonePriceId { get; set; }
        public ZonePrice? ZonePrice { get; set; }
        public string UserId { get; set; }
        public FuelMasterUser? User { get; set; }
        public decimal PriceBeforeChange { get; set; }

        public ZonePriceHistory(int zonePriceId, string userId, decimal priceBeforeChange)
        {
            ZonePriceId = zonePriceId;
            UserId = userId;
            PriceBeforeChange = priceBeforeChange;
        }
    }
}