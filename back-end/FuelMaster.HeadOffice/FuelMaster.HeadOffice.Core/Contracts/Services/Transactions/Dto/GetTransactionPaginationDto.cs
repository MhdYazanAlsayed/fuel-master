namespace FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;

public class GetTransactionPaginationDto
{
   public int Page { get; set; }
   public int? EmployeeId { get; set; }
   public int? NozzleId { get; set; }
   public DateTime? From { get; set; }
   public DateTime? To { get; set; }
   public int? StationId { get; set; }
}
