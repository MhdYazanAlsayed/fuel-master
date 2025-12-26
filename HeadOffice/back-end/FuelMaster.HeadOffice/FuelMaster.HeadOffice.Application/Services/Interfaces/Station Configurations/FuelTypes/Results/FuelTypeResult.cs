using System;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;

public class FuelTypeResult
{
    public int Id { get; private set; }
    public string ArabicName { get; private set; } = null!;
    public string EnglishName { get; private set; } = null!;
    public bool CanDelete { get; private set; }
}
