namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.DTOs;

public class DeliveryPaginationDto
{
    public int Page { get; set; }
    public int? CityId { get; set; }
    public int? AreaId { get; set; }
    public int? StationId { get; set; }
    public int? TankId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}