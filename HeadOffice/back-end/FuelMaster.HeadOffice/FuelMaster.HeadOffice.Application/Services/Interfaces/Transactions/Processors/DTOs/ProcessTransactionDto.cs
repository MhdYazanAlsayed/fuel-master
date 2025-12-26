using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.DTOs;

public class ProcessTransactionDto
{
    public required string UId { get; set; }
    public required Nozzle Nozzle { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required decimal Price { get; set; }
    public required decimal Amount { get; set; }
    public required decimal Volume { get; set; }
    public required decimal TotalVolume { get; set; }
    public required EmployeeResult Employee { get; set; } // TODO : Change it to Employee later on
    public required Tank Tank { get; set; }
    public required DateTime DateTime { get; set; }
}