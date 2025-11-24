using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles;

public interface INozzleRepository : IScopedDependency
{
    Task<IEnumerable<Nozzle>> GetCachedNozzlesAsync();
    Task<IEnumerable<NozzleResult>> GetAllAsync(GetNozzleRequest dto);
    Task<PaginationDto<NozzleResult>> GetPaginationAsync(int currentPage, GetNozzleRequest dto);
    Task<ResultDto<NozzleResult>> CreateAsync(CreateNozzleDto dto);
    Task<ResultDto<NozzleResult>> EditAsync(int id, EditNozzleDto dto);
    Task<NozzleResult?> DetailsAsync(int id);
    Task<ResultDto> DeleteAsync(int id);
}

