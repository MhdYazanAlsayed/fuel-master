namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.Results;

public class CityResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public bool CanDelete { get; set; }
}