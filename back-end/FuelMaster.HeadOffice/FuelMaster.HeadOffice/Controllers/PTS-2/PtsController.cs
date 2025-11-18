//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Models.Requests.PTS;
//using Microsoft.AspNetCore.Mvc;

//namespace FuelMaster.HeadOffice.Controllers.PTS_2
//{
//    [ApiController]
//    [Route("api/pts-controller")]
//    public class PtsController : ControllerBase
//    {
//        private readonly IPTSController _pTSController;
//        private readonly ILogger<PtsController> _logger;

//        public PtsController(IPTSController pTSController ,
//            ILogger<PtsController> logger)
//        {
//            _pTSController = pTSController;
//            _logger = logger;
//        }

//        [HttpPost("upload-status")]
//        public async Task<IActionResult> UploadStatusAsync([FromBody] UpdateStatusRequest request)
//        {
//            try
//            {
//                var response = await _pTSController.UploadStatusAsync(request);

//                return Ok(response);
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError("UploadStatusAsync: \n" + ex.Message);
//                return BadRequest();
//            }
//        }

//        [HttpPost("pump-transaction")]
//        public async Task<IActionResult> PumpTransactionAsync([FromBody] PumpTransactionRequest request)
//        {
//            try
//            {
//                var response = await _pTSController.PumpTransactionAsync(request);

//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("PumpTransactionAsync: \n" + ex.Message);
//                return BadRequest();
//            }
//        }

//        [HttpPost("upload-tank-delivery")]
//        public async Task<IActionResult> UploadTankDeliveryAsync([FromBody] UploadTankDeliveryRequest request)
//        {
//            try
//            {
//                var response = await _pTSController.UploadTankDeliveryAsync(request);

//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("UploadTankDeliveryAsync: \n" + ex.Message);
//                return BadRequest();
//            }
//        }

//        [HttpPost("upload-tank-measurment")]
//        public async Task<IActionResult> UploadTankMeasurementAsync([FromBody] UploadTankMeasurementRequest request)
//        {
//            try
//            {
//                var response = await _pTSController.UploadTankMeasurementAsync(request);

//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("UploadTankMeasurementAsync: \n" + ex.Message);
//                return BadRequest();
//            }
//        }
//    }
//}
