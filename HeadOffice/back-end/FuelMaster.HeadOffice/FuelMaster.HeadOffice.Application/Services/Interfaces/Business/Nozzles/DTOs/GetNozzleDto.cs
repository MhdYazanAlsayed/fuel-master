using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;

public class GetNozzleDto
{
    public int? StationId { get; set; }
    public int? TankId { get; set; }
}

