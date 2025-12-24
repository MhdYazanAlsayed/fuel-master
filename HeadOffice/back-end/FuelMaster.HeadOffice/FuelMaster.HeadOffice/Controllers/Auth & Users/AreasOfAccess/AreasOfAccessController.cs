using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Business;

[Route("api/areas-of-access")]
public class AreasOfAccessController : FuelMasterController
{
    private readonly IFuelMasterAreaOfAccessService _areaOfAccessService;
    private readonly ILogger<AreasOfAccessController> _logger;

    public AreasOfAccessController(IFuelMasterAreaOfAccessService areaOfAccessService, ILogger<AreasOfAccessController> logger)
    {
        _areaOfAccessService = areaOfAccessService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var areasOfAccess = await _areaOfAccessService.GetAllAsync();
            return Ok(areasOfAccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all areas of access");
            return BadRequest(ex.Message);
        }
    }
}

