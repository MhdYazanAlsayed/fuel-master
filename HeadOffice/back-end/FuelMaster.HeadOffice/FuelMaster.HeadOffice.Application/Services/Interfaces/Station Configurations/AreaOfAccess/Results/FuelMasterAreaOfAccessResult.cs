using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterAreaOfAccessService.Results;

public class FuelMasterAreaOfAccessResult
{
    public int Id { get; set; }
    public AreaOfAccess AreaOfAccess { get; set; }
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;
    public string EnglishDescription { get; set; } = null!;
    public string ArabicDescription { get; set; } = null!;
}

