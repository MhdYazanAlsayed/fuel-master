using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Enums;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Command.DTOs;

public class CreateTransactionDto
{
    public required TransactionType TransactionType { get; set; }
    public required string UId { get; set; }
    public required int NozzleId { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required decimal Price { get; set; }
    public required decimal Amount { get; set; }
    public required decimal Volume { get; set; }
    public required decimal TotalVolume { get; set; }
    public required string EmployeeCardNumber { get; set; }
    public required int TankId { get; set; }
    public required DateTime DateTime { get; set; }
}