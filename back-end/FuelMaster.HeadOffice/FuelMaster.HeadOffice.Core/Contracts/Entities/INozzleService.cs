using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Nozzles;
using FuelMaster.HeadOffice.Core.Models.Responses.Nozzles;

namespace FuelMaster.HeadOffice.Core.Contracts.Entities
{
    public interface INozzleService : IScopedDependency
    {
        Task<IEnumerable<NozzleResponse>> GetAllAsync(GetNozzleDto dto);
        Task<PaginationDto<NozzleResponse>> GetPaginationAsync(GetNozzlePaginationDto dto);
        Task<ResultDto<Nozzle>> CreateAsync(NozzleDto dto);
        Task<ResultDto<Nozzle>> EditAsync(int id, NozzleDto dto);
        Task<NozzleResponse?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }

}
