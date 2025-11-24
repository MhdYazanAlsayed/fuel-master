using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps;

public interface IPumpRepository : IScopedDependency
{
    Task<IEnumerable<Pump>> GetCachedPumpsAsync();
    Task<IEnumerable<PumpResult>> GetAllAsync(GetPumpRequest dto);
    Task<PaginationDto<PumpResult>> GetPaginationAsync(int currentPage, GetPumpRequest dto);
    Task<ResultDto<PumpResult>> CreateAsync(CreatePumpDto dto);
    Task<ResultDto<PumpResult>> EditAsync(int id, EditPumpDto dto);
    Task<PumpResult?> DetailsAsync(int id);
    Task<ResultDto> DeleteAsync(int id);
}
