//using FuelMaster.HeadOffice.Core.Contracts.Markers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Pumps;
//using FuelMaster.HeadOffice.Core.Models.Responses.Pumps;

//namespace FuelMaster.HeadOffice.Core.Contracts.Entities
//{
//    public interface IPumpService : IScopedDependency
//    {
//        Task<PaginationDto<PumpResponse>> GetPaginationAsync(int page , int? stationId);
//        Task<IEnumerable<PumpResponse>> GetAllAsync(GetPumpRequest request);
//        Task<ResultDto<PumpResponse>> CreateAsync(PumpRequest dto);
//        Task<ResultDto<PumpResponse>> EditAsync(int id, PumpRequest dto);
//        Task<PumpResponse?> DetailsAsync(int id);
//        Task<ResultDto> DeleteAsync(int id);
//    }
//}
