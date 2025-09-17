namespace FuelMaster.HeadOffice.Core.Entities
{
    public class City : EntityBase<int>
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }

        public City(string arabicName, string englishName)
        {
            ArabicName = arabicName;
            EnglishName = englishName;
        }
    }

}
