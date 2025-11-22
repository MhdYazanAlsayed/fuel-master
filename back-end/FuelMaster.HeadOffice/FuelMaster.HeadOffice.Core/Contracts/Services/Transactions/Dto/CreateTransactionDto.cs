using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;

public class CreateTransactionDto
{
   public required string UId { get; set; }
   public required int NozzleId { get; set; }
   public required PaymentMethod PaymentMethod { get; set; }
   public required decimal Price { get; set; }
   public required decimal Amount { get; set; }
   public required decimal Volume { get; set; }
   public required decimal TotalVolume { get; set; }
   public required int EmployeeId { get; set; }
   public required int TankId { get; set; }
   public required DateTime DateTime { get; set; }
}