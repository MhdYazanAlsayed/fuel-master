using System;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Station.Dtos;

public class EditStationDto
{
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;
    public int ZoneId { get; set; }
}
