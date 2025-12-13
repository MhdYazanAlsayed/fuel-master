using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddFuelMasterAuthentication(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme =
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["BerearSettings:Audience"],
                    ValidIssuer = configuration["BerearSettings:Issuer"],
                    RequireExpirationTime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BerearSettings:Key"]!)),
                    ValidateIssuerSigningKey = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        // Skip the default logic.
                        context.HandleResponse();

                        // Return your own response to the client.
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new { error = "Unauthorized" });
                        return context.Response.WriteAsync(result);
                    }
                };

            });

            return services;
        }
    }
}
