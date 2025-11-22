namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Dtos;

public class CreatePumpDto
{
    public int Number { get; set; }
    public int StationId { get; set; }
    public string? Manufacturer { get; set; }
}

