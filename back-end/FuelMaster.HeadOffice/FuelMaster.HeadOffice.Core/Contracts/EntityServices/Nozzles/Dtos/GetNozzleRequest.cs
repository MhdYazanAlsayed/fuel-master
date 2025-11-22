namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Dtos;

public class GetNozzleRequest
{
    public int? StationId { get; set; }
    public int? TankId { get; set; }
}

