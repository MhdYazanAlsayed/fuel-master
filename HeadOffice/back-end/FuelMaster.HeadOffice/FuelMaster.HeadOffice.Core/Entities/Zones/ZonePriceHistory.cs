namespace FuelMaster.HeadOffice.Core.Entities
{
    public class ZonePriceHistory : AggregateRoot<int>
    {
        public int ZonePriceId { get; private set; }
        public ZonePrice? ZonePrice { get; private set; }
        public string UserId { get; private set; }
        public FuelMasterUser? User { get; private set; }
        public decimal PriceBeforeChange { get; private set; }
        public decimal PriceAfterChange { get; private set; }

        internal ZonePriceHistory(int zonePriceId, string userId, decimal priceBeforeChange, decimal priceAfterChange)
        {
            ZonePriceId = zonePriceId;
            UserId = userId;
            PriceBeforeChange = priceBeforeChange;
            PriceAfterChange = priceAfterChange;
        }
    }
}