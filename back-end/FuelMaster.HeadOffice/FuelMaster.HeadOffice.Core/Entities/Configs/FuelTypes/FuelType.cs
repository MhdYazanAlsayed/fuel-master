using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

public class FuelType : Entity<int>
{
    public FuelType(string arabicName, string englishName)
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

    public void Update(string arabicName, string englishName)
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