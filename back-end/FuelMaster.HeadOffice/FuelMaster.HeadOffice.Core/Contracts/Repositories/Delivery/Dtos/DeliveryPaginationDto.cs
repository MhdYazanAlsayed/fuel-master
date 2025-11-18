namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Delivery.Dtos;

public class GetDeliveriesPaginationDto
{
    public int Page { get; set; }

    public int? StationId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}