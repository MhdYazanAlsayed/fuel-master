using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;

namespace FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;

public class FuelMasterAreaOfAccess : Entity<int>
{
    public AreaOfAccess AreaOfAccess { get; private set; }
    public string EnglishName { get; private set; }
    public string ArabicName { get; private set; }

    public string EnglishDescription { get; private set; }
    public string ArabicDescription { get; private set; }

    public FuelMasterAreaOfAccess(AreaOfAccess areaOfAccess, string englishName, string arabicName, string englishDescription, string arabicDescription)
    {
        AreaOfAccess = areaOfAccess;
        EnglishName = englishName;
        ArabicName = arabicName;
        EnglishDescription = englishDescription;
        ArabicDescription = arabicDescription;
    }
}