using System.Text;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Database;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy.Configs;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB;

public class ConnectionStringService : IConnectionString
{
    private readonly IServerSettingsConfiguration _serverSettingsConfiguration;
    public ConnectionStringService(IServerSettingsConfiguration serverSettingsConfiguration)
    {
        _serverSettingsConfiguration = serverSettingsConfiguration;
    }
    public string GetConnectionString(string databaseName)
    {
        var connectionStringBuilder = new StringBuilder();
        connectionStringBuilder.Append($"Data Source={_serverSettingsConfiguration.Server};Initial Catalog={databaseName};");
        
        if (_serverSettingsConfiguration.IntegratedSecurity)
        {
            // Use Windows Authentication (Integrated Security) - no username/password needed
            connectionStringBuilder.Append($"Integrated Security=True;Trusted_Connection={_serverSettingsConfiguration.TrustedConnection};");
        }
        else
        {
            // Use SQL Server Authentication - include username and password
            var username = _serverSettingsConfiguration.User;
            var password = _serverSettingsConfiguration.Password;
            connectionStringBuilder.Append($"User ID={username};Password={password};Persist Security Info=True;");
        }
        
        connectionStringBuilder.Append($"MultipleActiveResultSets={_serverSettingsConfiguration.MultipleActiveResultSets};");
        connectionStringBuilder.Append($"TrustServerCertificate={_serverSettingsConfiguration.TrustServerCertificate};");
        
        return connectionStringBuilder.ToString();
    }
}