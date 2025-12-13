namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy.Configs;

/// <summary>
/// This interface should used as a configuration for the server settings.
/// It's a singleton service.
/// It will be injected in ServerSettingsConfigurationExtension.cs file.
/// </summary>

public interface IServerSettingsConfiguration
{
    string Server { get; }
    int Port { get; }
    bool IntegratedSecurity { get; }
    bool TrustedConnection { get; }
    bool MultipleActiveResultSets { get; }
    bool TrustServerCertificate { get; }
    string User { get; }
    string Password { get; }
}