using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Entities.Configs.Tanks.Exceptions;
using FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;
using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Station : AggregateRoot<int>
    {
        public string EnglishName { get; private set; }
        public string ArabicName { get; private set; }
        public int CityId { get; private set; }
        public City? City { get; private set; }

        // Zone is required, it is used to pricing purposes.
        public Zone? Zone { get; private set; }
        public int ZoneId { get; private set; }

        // Area is optional, it is only for organization purposes
        // It also used to get data for multiple stations for Area manager.
        public int? AreaId { get; private set; }
        public Area? Area { get; private set; }

        public List<Tank> _tanks = new List<Tank>();
        public IReadOnlyList<Tank> Tanks => _tanks.AsReadOnly();

        public Station(string englishName, string arabicName , int cityId , int zoneId)
        {
            if (string.IsNullOrEmpty(englishName))
            {
                throw new ArgumentException("English name cannot be null or empty");
            }
            if (string.IsNullOrEmpty(arabicName))
            {
                throw new ArgumentException("Arabic name cannot be null or empty");
            }
            if (cityId <= 0)
            {
                throw new ArgumentException("City id must be greater than 0");
            }
            if (zoneId <= 0)
            {
                throw new ArgumentException("Zone id must be greater than 0");
            }

            EnglishName = englishName;
            ArabicName = arabicName;
            CityId = cityId;
            ZoneId = zoneId;
        }
    
        public void Update(string englishName, string arabicName, int zoneId)
        {
            if (string.IsNullOrEmpty(englishName))
            {
                throw new ArgumentException("English name cannot be null or empty");
            }
            if (string.IsNullOrEmpty(arabicName))
            {
                throw new ArgumentException("Arabic name cannot be null or empty");
            }

            // !TODO : Add history for moving station to another zone 
            if (zoneId <= 0)
            {
                throw new ArgumentException("Zone id must be greater than 0");
            }

            EnglishName = englishName;
            ArabicName = arabicName;
            ZoneId = zoneId;
            UpdatedAt = DateTimeCulture.Now;
        }
    
    }
}
