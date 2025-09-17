using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Seeders
{
    public class CreateDefaultUsersSeeder(
            UserManager<FuelMasterUser> _userManager,
            FuelMasterDbContext _context)
    {
        public async Task SeedAsync ()
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == "Admin");
            if (user is not null) 
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user , "MyP@ssw0rd");

                await _userManager.UpdateAsync(user);
                return;
            }

            var admin = new FuelMasterUser("Admin", true);
            await _userManager.CreateAsync(admin , "MyP@ssw0rd");
        }
    }
}
