using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Reports.Realtime;

public class RealtimeReportService : IRealtimeReport
{
    public async Task<ResultDto<GetRealTimeReportResponse>> GetRealTimeReportAsync(RealtimeReportDto dto)
    {
        throw new NotImplementedException();
    }
}