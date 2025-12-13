namespace FuelMaster.HeadOffice.Core.Entities;

public class NozzleHistory : Entity<int>
{
    public int NozzleId { get; private set; }
    public Nozzle? Nozzle { get; private set; }
    public decimal Volume { get; private set; }
    public decimal Amount { get; private set; }
    public int StationId { get; private set; }
    public Station? Station { get; private set; }
    public int TankId { get; private set; }
    public Tank? Tank { get; private set; }

    public NozzleHistory(int nozzleId, decimal volume, decimal amount, int stationId, int tankId)
    {
        NozzleId = nozzleId;
        Volume = volume;
        Amount = amount;    
        StationId = stationId;
        TankId = tankId;
    }
}