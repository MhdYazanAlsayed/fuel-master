using FuelMaster.HeadOffice.Core.Entities;
namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService.Results;

public class NozzleValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public Nozzle? Nozzle { get; set; }
}
