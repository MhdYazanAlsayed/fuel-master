namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.Results;

public class FuelMasterRoleResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public List<FuelMasterPermissionResult> AreaOfAccess { get; set; } = new();
}

