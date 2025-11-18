//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
//using FuelMaster.HeadOffice.Core.Models.Requests.Reports;
//using FuelMaster.HeadOffice.Core.Models.Requests.TankTransactions;
//using FuelMaster.HeadOffice.Core.Models.Requests.Transactions;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace FuelMaster.HeadOffice.Controllers
//{
//    [Route("api/reports") , Authorize]
//    public class ReportsController(IReportService _reportService) : FuelMasterController
//    {
//        [HttpGet("tank-transaction")]
//        public async Task<IActionResult> TankTransactionReportAsync([FromQuery] GetTankTransactionPaginationDto dto)
//        {
//            return Ok(await _reportService.GetTankTransactionReportAsync(dto));
//        }

//        [HttpGet("dashboard")]
//        public async Task<IActionResult> GetDashboardReport()
//        {
//            try
//            {
//                var report = await _reportService.GetDashboardReportsAsync();
//                return Ok(report);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("deliveries")]
//        public async Task<IActionResult> GetDeliveryReport([FromQuery] GetDeliveriesPaginationDto dto)
//        {
//            try
//            {
//                var report = await _reportService.GetDeliveryReportAsync(dto);
//                return Ok(report);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("realtime")]
//        public async Task<IActionResult> GetRealTimeReport([FromQuery] int? stationId)
//        {
//            var report = await _reportService.GetRealTimeReportAsync(stationId);

//            return Ok(report);
//        }

//        [HttpGet("transactions")]
//        public async Task<IActionResult> GetTransactionReport([FromQuery] GetTransactionPaginationDto dto)
//        {
//            try
//            {
//                var report = await _reportService.GetTransactionReportAsync(dto);
//                return Ok(report);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("time")]
//        public async Task<IActionResult> GetStoredProcedureReports([FromQuery] GetStoredProcedureReportRequest request)
//        {
//            try
//            {
//                var report = await _reportService.GetStoredProcedureReportsAsync(request);
//                return Ok(report);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
