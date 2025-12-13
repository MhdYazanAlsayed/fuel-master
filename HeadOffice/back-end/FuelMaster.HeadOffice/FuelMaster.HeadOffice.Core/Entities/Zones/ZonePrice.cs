using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class ZonePrice : Entity<int>
    {
        public int ZoneId { get; private set; }
        public Zone? Zone { get; private set; }
        public FuelType? FuelType { get; private set; }
        public int FuelTypeId { get; private set; }
        public decimal Price { get; private set; }
        public IReadOnlyList<ZonePriceHistory> Histories => _histories.AsReadOnly();
        protected List<ZonePriceHistory> _histories = new List<ZonePriceHistory>();

        internal ZonePrice(int zoneId, int fuelTypeId, decimal price)
        {
            ZoneId = zoneId;
            FuelTypeId = fuelTypeId;
            Price = price;
        }

        /// <summary>
        /// Change the price of the zone price
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="newPrice">The new price</param>
        /// <returns>True if the price was changed, false otherwise</returns>
        public bool ChangePrice(string userId, decimal newPrice)
        {
            if (newPrice <= 0)
            {
                throw new ZonePriceInvalidException("Price must be greater than 0");
            }

            if (Price == newPrice)
            {
                return false;
            }

            _histories.Add(new ZonePriceHistory(Id, userId, Price, newPrice));
            Price = newPrice;
            UpdatedAt = DateTimeCulture.Now;
            return true;
        }
    }
}