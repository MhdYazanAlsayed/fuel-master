namespace FuelMaster.HeadOffice.Core.Entities
{
    public class FuelMasterGroup(string arabicName , string englishName) : EntityBase<int>
    {
        public string ArabicName { get; set; } = arabicName;
        public string EnglishName { get; set; } = englishName;
        public IEnumerable<Permission>? Permissions { get; set; }
    }
}
