using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
using Microsoft.AspNetCore.Mvc;
namespace FuelMaster.HeadOffice.Controllers;

[Route("api/deliveries")]
public class DeliveryController : FuelMasterController
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DeliveryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var result = await _deliveryService.CreateAsync(dto);
        if (!result.Succeeded) return BadRequest(result.Message);

        return Ok(result.Entity);
      
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Edit(int id, [FromBody] DeliveryDto dto)
    //{
    //    try
    //    {
    //        await _deliveryService.EditAsync(id, dto);
    //        return Ok();
    //    }
    //    catch (NullReferenceException)
    //    {
    //        return NotFound("Delivery not found.");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var delivery = await _deliveryService.DetailsAsync(id);
        if (delivery == null)
        {
            return NotFound("Delivery not found.");
        }
        return Ok(delivery);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deliveryService.DeleteAsync(id);
        if (!result.Succeeded) return BadRequest(result.Message);

        return Ok();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] GetDeliveriesPaginationDto dto)
    {
        var result = await _deliveryService.GetPaginationAsync(dto);
        return Ok(result);
    }
}
