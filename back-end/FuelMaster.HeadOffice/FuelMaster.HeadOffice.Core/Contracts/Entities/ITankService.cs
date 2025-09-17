using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Tanks;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface ITankService : IScopedDependency
    {
        Task<IEnumerable<TankResponse>> GetAllAsync(GetTankRequest dto);
        Task<PaginationDto<TankResponse>> GetPaginationAsync(int currentPage, GetTankRequest dto);
        Task<ResultDto<TankResponse>> CreateAsync(TankRequest dto);
        Task<ResultDto<TankResponse>> EditAsync(int id, TankRequest dto);
        Task<TankResponse?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }
}
