using FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.Results;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.Results;

public class AreaResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public CityResult? City { get; set; }
    public bool CanDelete { get; set; }
}

