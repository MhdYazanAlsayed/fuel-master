using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Tanks.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Tanks.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Entities
{
   public interface ITankRepository : IScopedDependency
   {
       Task<IEnumerable<Tank>> GetCachedTanksAsync();
       Task<IEnumerable<TankResult>> GetAllAsync(GetTankRequest dto);
       Task<PaginationDto<TankResult>> GetPaginationAsync(int currentPage, GetTankRequest dto);
       Task<ResultDto<TankResult>> CreateAsync(CreateTankDto dto);
       Task<ResultDto<TankResult>> EditAsync(int id, EditTankDto dto);
       Task<TankResult?> DetailsAsync(int id);
       Task<ResultDto> DeleteAsync(int id);
   }
}
