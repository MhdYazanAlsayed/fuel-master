using System;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.FuelTypes.Results;

public class FuelTypeResult
{
    public int Id { get; set; }
    public string ArabicName { get; set; } = null!;
    public string EnglishName { get; set; } = null!;
}
