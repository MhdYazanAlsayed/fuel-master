using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;

public class CreateStationDto
{
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;
    public int CityId { get; set; }
    public int ZoneId { get; set; }
}