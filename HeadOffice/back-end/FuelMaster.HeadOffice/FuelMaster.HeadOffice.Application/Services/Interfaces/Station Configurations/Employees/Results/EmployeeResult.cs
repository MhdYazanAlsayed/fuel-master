using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterRoleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Infrastructure.Authentication.UserService.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;

public class EmployeeResult
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string? EmailAddress { get; private set; }
    public int? Age { get; private set; }
    public string? IdentificationNumber { get; private set; }
    public string? Address { get; private set; }
    public string? PTSNumber { get; private set; }
    public Scope Scope { get; private set; }
    public FuelMasterRoleResult Role { get; set; } = null!;
    public FuelMasterUserResult User { get; set; } = null!;
    public StationResult? Station { get; set; }
    public AreaResult? Area { get; set; }
    public CityResult? City { get; set; }
}