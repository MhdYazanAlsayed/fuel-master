using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.FuelMasterGroups;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface IFuelMasterGroupService : IScopedDependency
    {
        Task<IEnumerable<FuelMasterGroup>> GetAllAsync();
        Task<PaginationDto<FuelMasterGroup>> GetPaginationAsync(int currentPage);
        Task<ResultDto<FuelMasterGroup>> CreateAsync(FuelMasterGroupDto dto);
        Task<ResultDto<FuelMasterGroup>> EditAsync(int id, FuelMasterGroupDto dto);
        Task<ResultDto> DeleteAsync(int id);
        Task<FuelMasterGroup?> DetailsAsync(int id);
    }

}
