using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface IAccountService : IScopedDependency
    {
        Task<ResultDto> EditPasswordAsync(string userId, EditPasswordDto dto);
        Task<ResultDto> EditCurrentPassword(EditCurrentPasswordDto dto);
        Task Reset();
    }
}
