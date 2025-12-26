
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Commands;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Enums;
using FuelMaster.HeadOffice.Controllers.Business.Transactions.DTOs;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
   [Route("api/transactions") , Authorize]
   public class TransactionsController(ITransactionCommand _transactionService) : FuelMasterController
   {
       [HttpPost]
       public async Task<IActionResult> SellFuel([FromBody] CreateTransactionDto dto)
       {
            try
            {
                var result = await _transactionService.CreateAsync(new()
                {
                    TransactionType = TransactionType.Manual,
                    UId = dto.UId,
                    NozzleId = dto.NozzleId,
                    PaymentMethod = dto.PaymentMethod,
                    Price = dto.Price,
                    Amount = dto.Amount,
                    Volume = dto.Volume,
                    TotalVolume = dto.TotalVolume,
                    EmployeeCardNumber = dto.EmployeeCardNumber,
                    TankId = dto.TankId,
                    DateTime = dto.DateTime
                });
                if (!result.Succeeded) return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
       }
   }
}