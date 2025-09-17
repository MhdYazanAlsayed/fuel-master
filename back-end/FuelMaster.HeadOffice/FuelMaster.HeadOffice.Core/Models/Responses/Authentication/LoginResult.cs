namespace FuelMaster.HeadOffice.Core.Models.Responses.Authentication
{
    public class LoginResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public UserResult? Data { get; set; }
    }
    public class UserResult
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public int? StationId { get; set; }
        public IEnumerable<PermissionResult>? Permissions { get; set; }
    }

    public class PermissionResult
    {
        public string? Key { get; set; }
        public bool Value { get; set; }
    }
}
