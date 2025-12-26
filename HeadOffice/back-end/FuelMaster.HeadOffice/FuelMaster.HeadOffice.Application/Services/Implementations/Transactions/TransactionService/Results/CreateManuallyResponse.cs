
namespace FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;

public class CreateManuallyResponse
{
  public bool Succeeded { get; set; }
  public List<string> Messages { get; set; } = new List<string>();
  public TransactionResult? Transaction { get; set; } = null;
}