using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService.Results;

public class EmployeeValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public EmployeeResult? Employee { get; set; }
}
