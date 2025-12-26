using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.DTOs;

public class EditStationDto
{
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;
    public int ZoneId { get; set; }
    public int? AreaId { get; set; }
}
