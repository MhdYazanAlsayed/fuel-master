using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;

public class TransactionResult
{
    public string? UId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    
    // Unit price of fuel (e.g. price per liter = $1.2).
    public decimal Price { get; set; }
    public NozzleResult? Nozzle { get; set; }

    // The financial amount of the transaction (total price paid).
    public decimal Amount { get; set; }

    // Quantity of fuel sold (in liters or cubic meters).
    public decimal Volume { get; set; }
    public EmployeeResult? Employee { get; set; }
    public decimal Totalizer { get; set; }
    public decimal TotalizerAfter { get; set; }
    public StationResult? Station { get; set; }
    public DateTime DateTime { get; set; }
}