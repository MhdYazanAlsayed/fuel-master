using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Core.Enums;
namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Realtime.Results;

public class GetRealTimeReportResponse
{
    public GetRealTimeReportResponse(List<GetRealTimeReportResposne_Nozzle> nozzles , List<GetRealTimeReportResposne_Tanks> tanks)
    {
        Nozzles = nozzles;
        Tanks = tanks;
    }
    public List<GetRealTimeReportResposne_Nozzle> Nozzles { get; set; } = null!;
    public List<GetRealTimeReportResposne_Tanks> Tanks { get; set; } = null!;
}

public class GetRealTimeReportResposne_Nozzle
{
    public int Number { get; set; }
    public PumpResult? Pump { get; set; }
    public FuelTypeResult? FuelType { get; set; }
    public decimal Amount { get; set; }
    public decimal Volume { get; set; }
    public decimal Price { get; set; }
    public NozzleStatus Status { get; set; }
}

public class GetRealTimeReportResposne_Tanks
{
    public decimal CapacityPercent { get; set; }
    public decimal CurrentVolume { get; set; }
    public decimal MaxLimit { get; set; }
    public decimal MinLimit { get; set; }
    public decimal CurrentLevel { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

