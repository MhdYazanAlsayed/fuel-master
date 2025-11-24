using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;

public class ZoneResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
    public bool CanDelete { get; set; }
}

