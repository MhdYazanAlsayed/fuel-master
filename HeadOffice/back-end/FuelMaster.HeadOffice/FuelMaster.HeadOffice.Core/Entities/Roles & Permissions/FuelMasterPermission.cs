
namespace FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;

public class FuelMasterPermission : Entity<int>
{
    public int RoleId { get; private set; }
    public FuelMasterRole? Role { get; private set; }

    public FuelMasterAreaOfAccess? AreaOfAccess { get; private set; }
    public int AreaOfAccessId { get; private set; }

    internal FuelMasterPermission(int roleId, int areaOfAccessId)
    {
        RoleId = roleId;
        AreaOfAccessId = areaOfAccessId;
    }
}