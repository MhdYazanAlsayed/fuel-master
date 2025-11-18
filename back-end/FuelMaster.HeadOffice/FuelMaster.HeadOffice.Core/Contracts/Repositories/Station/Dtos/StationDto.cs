namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Dtos;

public class StationDto
{
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;
    public int CityId { get; set; }
    public int ZoneId { get; set; }
}