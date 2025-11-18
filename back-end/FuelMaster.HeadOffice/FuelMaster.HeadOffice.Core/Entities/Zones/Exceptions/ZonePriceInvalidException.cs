namespace FuelMaster.HeadOffice.Core.Entities.Zones.Exceptions;

public class ZonePriceInvalidException : Exception
{
    public ZonePriceInvalidException(string message) : base(message)
    {
    }
}