using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Station.Results;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Results;

public class PumpResult
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int StationId { get; set; }
    public StationResult? Station { get; set; }
    public string? Manufacturer { get; set; }
    public int NozzleCount { get; set; }
    public bool CanDelete => NozzleCount == 0;
}

