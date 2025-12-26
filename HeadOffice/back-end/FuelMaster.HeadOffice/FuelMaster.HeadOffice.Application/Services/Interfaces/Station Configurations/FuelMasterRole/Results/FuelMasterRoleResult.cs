using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterAreaOfAccessService.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterRoleService.Results;

public class FuelMasterRoleResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public List<FuelMasterAreaOfAccessResult> AreasOfAccess { get; set; } = new();
}

