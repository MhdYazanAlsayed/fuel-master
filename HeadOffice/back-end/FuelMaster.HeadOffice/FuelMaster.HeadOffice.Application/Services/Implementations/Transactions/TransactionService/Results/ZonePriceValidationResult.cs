using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService.Results;

public class ZonePriceValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public ZonePriceResult? ZonePrice { get; set; }
}
