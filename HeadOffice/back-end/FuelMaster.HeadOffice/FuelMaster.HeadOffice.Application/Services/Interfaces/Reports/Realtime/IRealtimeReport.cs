using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Reports;

public interface IRealtimeReport : IScopedDependency
{
    Task<ResultDto<GetRealTimeReportResponse>> GetRealTimeReportAsync(RealtimeReportDto dto);
}