//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Requests.Permissions;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace FuelMaster.HeadOffice.Controllers.Groups___Permission
//{
//    [Authorize]
//    [Route("api/groups")]
//    public class PermissionsController : FuelMasterController
//    {
//        private readonly IFuelMasterPermission _fuelMasterPermission;

//        public PermissionsController(IFuelMasterPermission fuelMasterPermission)
//        {
//            _fuelMasterPermission = fuelMasterPermission;
//        }

//        [HttpGet("{groupId}/permissions")]
//        public async Task<IActionResult> GetAsync (int groupId)
//        {
//            return Ok(await _fuelMasterPermission.GetAsync(groupId));
//        }

//        [HttpPut("{groupId}/permissions")]
//        public async Task<IActionResult> UpdateAsync(int groupId , [FromBody] UpdatePermissionsRequest request)
//        {
//            var result = await _fuelMasterPermission.UpdateAsync(groupId, request);
//            if (!result.Succeeded) return BadRequest();

//            return Ok();
//        }
//    }
//}
