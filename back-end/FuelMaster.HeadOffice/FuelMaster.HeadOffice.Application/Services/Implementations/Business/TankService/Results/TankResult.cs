using System;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.Results;

public class TankResult
{
    public int Id { get; set; }
    public StationResult? Station { get; set; }
    public FuelTypeResult? FuelType { get; set; }
    public int Number { get; set; }
    public decimal Capacity { get; set; }
    public decimal MaxLimit { get; set; }
    public decimal MinLimit { get; set; }
    public decimal CurrentLevel { get; set; }
    public decimal CurrentVolume { get; set; }
    public bool HasSensor { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool CanDelete { get; set; }
}
