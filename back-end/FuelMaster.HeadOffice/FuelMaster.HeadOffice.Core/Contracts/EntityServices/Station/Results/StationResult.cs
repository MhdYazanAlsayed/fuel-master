namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Station.Results;

public class StationResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public StationCityResult? City { get; set; }
    public StationZoneResult? Zone { get; set; }
    public bool CanDelete { get; set; }
}