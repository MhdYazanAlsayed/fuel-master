namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Zone : EntityBase<int>
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public IEnumerable<Station>? Stations { get; set; }
        public IEnumerable<ZonePrice>? Prices { get; set; }

        public Zone(string arabicName, string englishName)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
        }
    }

}
