
namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Tanks.Dtos;

public class TankDto
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
