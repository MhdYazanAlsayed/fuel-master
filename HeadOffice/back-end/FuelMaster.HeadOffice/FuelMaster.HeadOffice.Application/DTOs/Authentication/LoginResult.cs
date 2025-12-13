namespace FuelMaster.HeadOffice.Application.DTOs.Authentication
{
    public class LoginResult
    {
        public TokenResult AccessToken { get; set; } = null!;
        public TokenResult RefreshToken { get; set; } = null!;
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public Scope Scope { get; set; }
        public int? StationId { get; set; }
        public int? AreaId { get; set; }
        public int? CityId { get; set; }
    }

    public class TokenResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}