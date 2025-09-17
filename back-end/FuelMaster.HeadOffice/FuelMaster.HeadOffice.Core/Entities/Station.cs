namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Station : EntityBase<int>
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
        public Zone? Zone { get; set; }
        public int ZoneId { get; set; }
        public IEnumerable<Tank>? Tanks { get; set; }

        public Station(string englishName, string arabicName , int cityId , int zoneId)
        {
            EnglishName = englishName;
            ArabicName = arabicName;
            CityId = cityId;
            ZoneId = zoneId;
        }
    }

}
