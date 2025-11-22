using FuelMaster.HeadOffice.Core.Interfaces.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Delivery.Dtos;
using FuelMaster.HeadOffice.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace FuelMaster.HeadOffice.Controllers;

[Route("api/deliveries")]
public class DeliveryController : FuelMasterController
{
    private readonly IDeliveryRepository _deliveryService;

    public DeliveryController(IDeliveryRepository deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DeliveryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        try 
        {
            var result = await _deliveryService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try 
        {
            var delivery = await _deliveryService.DetailsAsync(id);
            if (delivery is null)
            {
                return NotFound();
            }

            return Ok(delivery);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            var result = await _deliveryService.DeleteAsync(id);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] GetDeliveriesPaginationDto dto)
    {
        try 
        {
            var result = await _deliveryService.GetPaginationAsync(dto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
