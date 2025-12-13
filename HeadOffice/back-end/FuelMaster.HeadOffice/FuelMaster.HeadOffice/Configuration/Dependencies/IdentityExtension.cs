using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddFuelMasterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<FuelMasterUser , IdentityRole>()
                .AddUserManager<UserManager<FuelMasterUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<FuelMasterDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<IdentityLocalizationDescriber>();

            return services;
        }
    }
}
