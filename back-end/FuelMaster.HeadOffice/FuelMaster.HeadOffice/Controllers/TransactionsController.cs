using FuelMaster.HeadOffice.ApplicationService.Services;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/transactions") , Authorize]
    public class TransactionsController(ITransactionService _transactionService) : FuelMasterController
    {
        [HttpPost]
        public async Task<IActionResult> SellFuel([FromBody] CreateManuallyTransactionDto dto)
        {
            var result = await _transactionService.CreateManuallyAsync(dto);
            if (!result.Succeeded) return BadRequest(result);
            
            return Ok(result);
        }
    }
}