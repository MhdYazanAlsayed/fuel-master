namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Infrastructure.Authentication.UserService.Results;

public class FuelMasterUserResult
{
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}