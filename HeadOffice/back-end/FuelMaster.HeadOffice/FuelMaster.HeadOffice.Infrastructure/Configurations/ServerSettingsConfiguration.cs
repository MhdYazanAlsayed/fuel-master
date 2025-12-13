using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy.Configs;

namespace FuelMaster.HeadOffice.Infrastructure.Configurations
{
    public class ServerSettingsConfiguration : IServerSettingsConfiguration
    {
        public string Server { get; set; } = null!;
        public int Port { get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool TrustedConnection { get; set; }
        public bool MultipleActiveResultSets { get; set; }
        public bool TrustServerCertificate { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

