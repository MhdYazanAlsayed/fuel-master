namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.DTOs;

public class CreateEmployeeDto
{
    public string FullName { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public int RoleId { get; set; }
    public int? StationId { get; set; }
    public int? AreaId { get; set; }
    public int? CityId { get; set; }
    public Scope Scope { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    public int? Age { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Address { get; set; }
    public string? PTSNumber { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}