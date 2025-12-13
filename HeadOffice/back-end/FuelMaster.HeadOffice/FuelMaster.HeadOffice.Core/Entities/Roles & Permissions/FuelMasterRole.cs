namespace FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;

public class FuelMasterRole : Entity<int> 
{
    public string ArabicName { get; set; }
    public string EnglishName { get; set; }

    protected List<FuelMasterPermission> _areasOfAccess = new List<FuelMasterPermission>();
    public IReadOnlyList<FuelMasterPermission> AreasOfAccess => _areasOfAccess.AsReadOnly();

    public FuelMasterRole(string arabicName, string englishName)
    {
        ArabicName = arabicName;
        EnglishName = englishName;
    }

    public void AddAreaOfAccess(int areaOfAccessId)
    {
        _areasOfAccess.Add(new FuelMasterPermission(Id, areaOfAccessId));
    }

    public void RemoveAreaOfAccess(FuelMasterPermission areaOfAccess)
    {
        _areasOfAccess.Remove(areaOfAccess);
    }
}