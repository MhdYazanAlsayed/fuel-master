using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Interfaces.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class AccountService(IUserService _userService) : IAccountService
    {
        public async Task<ResultDto> EditCurrentPassword(EditCurrentPasswordDto dto)
        {
            var userId = await _userService.GetLoggedUserIdAsync();
            if (userId is null) return Results.Failure();

            var user = await _userService.UserManager.FindByIdAsync(userId);
            if (user is null) return Results.Failure(Resource.EntityNotFound);

            var result = await _userService.UserManager.CheckPasswordAsync(user, dto.CurrentPassword);
            if (!result) return Results.Failure(Resource.CurrentPasswordWrong);

            user.PasswordHash = _userService.UserManager.PasswordHasher.HashPassword(user , dto.NewPassword);

            await _userService.UpdateAsync(user);

            return Results.Success();
        }

        public async Task<ResultDto> EditPasswordAsync(string userId , EditPasswordDto dto)
        {
            var user = await _userService.UserManager.FindByIdAsync(userId);
            if (user is null) return Results.Failure(Resource.EntityNotFound);

            user.PasswordHash = _userService.UserManager.PasswordHasher.HashPassword(user , dto.Password);

            await _userService.UpdateAsync(user);

            return Results.Success();
        }

    }
}
