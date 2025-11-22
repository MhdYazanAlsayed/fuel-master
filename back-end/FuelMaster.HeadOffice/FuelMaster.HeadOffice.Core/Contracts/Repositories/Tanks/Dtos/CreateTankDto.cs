namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Tanks.Dtos;

public class CreateTankDto
{
    public int StationId { get; set; }

    public int FuelTypeId { get; set; }

    public int Number { get; set; }

    public decimal Capacity { get; set; }
    
    public decimal MaxLimit { get; set; }

    public decimal MinLimit { get; set; }

    public decimal CurrentLevel { get; set; }

    public decimal CurrentVolume { get; set; }

    public bool HasSensor { get; set; }
}

