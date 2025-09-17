using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class ZonePrice : EntityBase<int>
    {
        public int ZoneId { get; set; }
        public Zone? Zone { get; set; }
        public FuelType FuelType { get; set; }
        public decimal Price { get; private set; }
        public IEnumerable<ZonePriceHistory>? Histories { get; set; }

        protected ZonePrice()
        {
        }

        public ZonePrice(int zoneId, FuelType fuelType, decimal price)
        {
            if (price <= 0) 
            {
                throw new ArgumentException("Price must be greater than 0");
            }
            
            ZoneId = zoneId;
            FuelType = fuelType;
            Price = price;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
            {
                throw new ArgumentException("Price must be greater than 0");
            }
            Price = newPrice;
        }
    }
}