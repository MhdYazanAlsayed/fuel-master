using System;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.PumpService.Results;

public class PumpResult
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int StationId { get; set; }
    public StationResult? Station { get; set; }
    public string? Manufacturer { get; set; }
    public int NozzleCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool CanDelete { get; set; }
}

