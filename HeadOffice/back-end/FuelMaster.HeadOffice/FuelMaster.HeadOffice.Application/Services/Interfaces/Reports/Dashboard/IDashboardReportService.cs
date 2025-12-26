using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard;

public interface IDashboardReportService : IScopedDependency
{
    Task<ResultDto<DashboardReportsResult>> GetDashboardReportsAsync();
}

