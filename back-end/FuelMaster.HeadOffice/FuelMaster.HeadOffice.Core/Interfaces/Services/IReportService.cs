//using FuelMaster.HeadOffice.Core.Contracts.Markers;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
//using FuelMaster.HeadOffice.Core.Models.Requests.Reports;
//using FuelMaster.HeadOffice.Core.Models.Requests.TankTransactions;
//using FuelMaster.HeadOffice.Core.Models.Requests.Transactions;
//using FuelMaster.HeadOffice.Core.Models.Responses.Reports;

//namespace FuelMaster.HeadOffice.Core.Contracts.Services
//{
//    public interface IReportService : IScopedDependency
//    {
//        Task<DashboardReportResponse> GetDashboardReportAsync(int? stationId);
//        Task<DashboardReportsDto> GetDashboardReportsAsync();
//        Task<PaginationDto<TankTransaction>> GetTankTransactionReportAsync(GetTankTransactionPaginationDto dto);
//        Task<GetRealTimeReportResponse> GetRealTimeReportAsync(int? stationId);
//        Task<PaginationDto<GetTransactionReportResponse>> GetTransactionReportAsync(GetTransactionPaginationDto dto);
//        Task<PaginationDto<Delivery>> GetDeliveryReportAsync(GetDeliveriesPaginationDto dto);
//        Task<StoredProcedureReportsResponse> GetStoredProcedureReportsAsync(GetStoredProcedureReportRequest request);
//    }
//}
