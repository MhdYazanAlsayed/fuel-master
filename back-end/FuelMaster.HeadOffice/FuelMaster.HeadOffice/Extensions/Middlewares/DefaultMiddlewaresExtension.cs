using FuelMaster.HeadOffice.Core.Hubs;
using Microsoft.AspNetCore.Builder;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public static class DefaultMiddlewaresExtension
    {
        public static void UseMiddlewares (this WebApplication app) 
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.ApplyMigrationForAllTenants();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHub<RealTimeHub>("/realtime");

            app.UseMiddleware<FuelMasterRequestLoggerMiddleware>();

            app.UseMiddleware<TenantMiddleware>();

            app.UseMiddleware<LocalizationMiddlware>();

            app.MapControllers();
        }
    }

}
