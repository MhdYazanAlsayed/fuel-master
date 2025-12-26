using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Business.Reports;

[Route("api/reports/dashboard")]
[Authorize]
public class DashboardReportsController : FuelMasterController
{
    private readonly IDashboardReportService _dashboardReportService;
    private readonly ILogger<DashboardReportsController> _logger;

    public DashboardReportsController(
        IDashboardReportService dashboardReportService,
        ILogger<DashboardReportsController> logger)
    {
        _dashboardReportService = dashboardReportService;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive dashboard reports including tank levels, daily sales, station comparisons, payment methods, employee performance, and monthly trends.
    /// Reports are automatically filtered based on the user's scope (ALL, City, Area, Station, Self).
    /// </summary>
    /// <returns>Dashboard reports result containing all statistics</returns>
    [HttpGet]
    public async Task<IActionResult> GetDashboardReportsAsync()
    {
        try
        {
            var result = await _dashboardReportService.GetDashboardReportsAsync();
            
            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard reports");
            return BadRequest(ex.Message);
        }
    }
}

