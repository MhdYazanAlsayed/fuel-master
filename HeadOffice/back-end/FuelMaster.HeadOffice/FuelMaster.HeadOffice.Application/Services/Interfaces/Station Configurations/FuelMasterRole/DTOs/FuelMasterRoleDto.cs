namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterRoleService.DTOs;

public class FuelMasterRoleDto
{
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public List<int> AreasOfAccessIds { get; set; } = new();
}

