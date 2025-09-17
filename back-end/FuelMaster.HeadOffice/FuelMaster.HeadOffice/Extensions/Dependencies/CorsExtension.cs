namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class CorsExtension
    {
        public static IServiceCollection AddFuelMasterCors (this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(cors =>
                {
                    cors
                    .WithOrigins("http://foryou.localhost:3000")
                    .WithOrigins("http://qserv.localhost:3000")
                    .WithOrigins("http://localhost:3000")
                    .WithOrigins("http://20.21.106.9:9998")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            return services;
        }
    }
}
