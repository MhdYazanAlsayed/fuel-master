namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Queries.DTOs;

public class TransactionsDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int? AreaId { get; set; }
    public int? CityId { get; set; }
    public int? StationId { get; set; }
    public int? NozzleId { get; set; }
    public int? PumpId { get; set; }
    public int? EmployeeId { get; set; }
}