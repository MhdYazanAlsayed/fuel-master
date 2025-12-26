namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService.Results;

public class TankVolumeValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
