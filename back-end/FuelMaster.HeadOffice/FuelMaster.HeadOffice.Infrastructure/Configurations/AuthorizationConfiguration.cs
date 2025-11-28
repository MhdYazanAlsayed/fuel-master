namespace FuelMaster.HeadOffice.Infrastructure.Configurations
{
    public class AuthorizationConfiguration
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool RequireExpirationTime { get; set; }
        public string Key { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;
    }
}
