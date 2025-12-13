namespace FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;

public class ZoneNotFoundException : Exception
{
    public ZoneNotFoundException(string message) : base(message)
    {
    }
}