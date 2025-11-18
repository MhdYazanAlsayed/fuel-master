namespace FuelMaster.HeadOffice.Core.Entities.Configs.Stations.Exceptions;

public class InvalidAssignStationException : Exception
{
    public InvalidAssignStationException(string message) : base(message)
    {
    }
}