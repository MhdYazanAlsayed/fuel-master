namespace FuelMaster.HeadOffice.Core.Entities.Zones.Events;

public class ZonePriceChangedEvent : IDomainEvent
{
    public int ZoneId { get; private set; }
    public int FuelTypeId { get; private set; }
    public decimal NewPrice { get; private set; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ZonePriceChangedEvent(int zoneId, int fuelTypeId, decimal newPrice)
    {
        ZoneId = zoneId;
        FuelTypeId = fuelTypeId;
        NewPrice = newPrice;
    }
}