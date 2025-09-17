using FuelMaster.HeadOffice.Core.Configurations;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class AuthenticationConfigurationExtension
    {
        public static IServiceCollection AddAuthenticationConfiguration (this IServiceCollection services , IConfiguration configuration)
        {
            var authSettings = configuration
                .GetSection("BerearSettings")
                .Get<AuthorizationConfiguration>();
   
            if (authSettings is null)
                throw new Exception();

            services.AddSingleton(authSettings);

            return services;
        }
    }
}
