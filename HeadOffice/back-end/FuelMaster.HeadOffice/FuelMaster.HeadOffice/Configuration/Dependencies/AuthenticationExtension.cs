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
                    OnMessageReceived = context =>
                    {
                        // Try to read token from Authorization header first
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                        // If not found in header, try to read from cookies
                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Cookies["access_token"];
                        }

                        // Set the token if found
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
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
