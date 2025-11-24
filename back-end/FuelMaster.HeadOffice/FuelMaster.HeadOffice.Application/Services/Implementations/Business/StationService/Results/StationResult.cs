namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;

public class StationResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public StationCityResult? City { get; set; }
    public StationZoneResult? Zone { get; set; }
    public bool CanDelete { get; set; }
}