namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.DTOs;

public class RealtimeReportDto
{
    public int? StationId { get; set; }
    public int? FuelTypeId { get; set; }
    public int? PumpId { get; set; }
    public int? CityId { get; set; }
    public int? AreaId { get; set; }
}