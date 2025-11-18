using FuelMaster.HeadOffice.Core.Entities.Zones.Events;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Zone : AggregateRoot<int>
    {
        public Zone(string arabicName, string englishName)
        {
            if (string.IsNullOrEmpty(arabicName))
            {
                throw new ArgumentException("Arabic name cannot be null or empty");
            }
            if (string.IsNullOrEmpty(englishName))
            {
                throw new ArgumentException("English name cannot be null or empty");
            }

            ArabicName = arabicName;
            EnglishName = englishName;
        }

        public string ArabicName { get; private set; }
        public string EnglishName { get; private set; }
        protected List<Station> _stations = new List<Station>();
        public IReadOnlyList<Station> Stations => _stations.AsReadOnly();

        protected List<ZonePrice> _prices = new List<ZonePrice>();
        public IReadOnlyList<ZonePrice> Prices => _prices.AsReadOnly();
    
        /// Change the price of a fuel type in the zone
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="fuelTypeId">The fuel type id</param>
        /// <param name="newPrice">The new price</param>
        /// <returns>True if the price was changed, false otherwise</returns>
        public bool ChangePrice (string userId, int fuelTypeId, decimal newPrice)
        {
            var zonePrice = _prices.SingleOrDefault(x => x.FuelTypeId == fuelTypeId);
            if (zonePrice is null)
            {
                _prices.Add(new ZonePrice(Id, fuelTypeId, newPrice));
                return true;
            }

            if (zonePrice.Price == newPrice)
                return false;

            zonePrice.ChangePrice(userId, newPrice);

            return true;
        }

        public void InitializePrice(int fuelTypeId)
        {
            var zonePrice = _prices.SingleOrDefault(x => x.FuelTypeId == fuelTypeId);
            if (zonePrice is not null)
                return;

            _prices.Add(new ZonePrice(Id, fuelTypeId, 0));
        }

        public void Update (string arabicName, string englishName)
        {
            if (string.IsNullOrEmpty(arabicName))
            {
                throw new ArgumentException("Arabic name cannot be null or empty");
            }
            if (string.IsNullOrEmpty(englishName))
            {
                throw new ArgumentException("English name cannot be null or empty");
            }

            ArabicName = arabicName;
            EnglishName = englishName;
            UpdatedAt = DateTimeCulture.Now;
        }
    }
}
