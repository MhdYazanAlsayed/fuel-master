namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.DTOs;

public class EditTankDto
{
    public decimal Capacity { get; set; }
    
    public decimal MaxLimit { get; set; }

    public decimal MinLimit { get; set; }

    public decimal CurrentLevel { get; set; }

    public decimal CurrentVolume { get; set; }

    public bool HasSensor { get; set; }
}