namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles.Dtos;

public class GetNozzleRequest
{
    public int? StationId { get; set; }
    public int? TankId { get; set; }
}

