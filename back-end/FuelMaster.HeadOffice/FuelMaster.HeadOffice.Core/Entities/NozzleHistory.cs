namespace FuelMaster.HeadOffice.Core.Entities;

public class NozzleHistory : EntityBase<int>
{
    public int NozzleId { get; set; }
    public Nozzle? Nozzle { get; set; }
    public decimal Volume { get; set; }
    public decimal Amount { get; set; }
    public int StationId { get; set; }
    public Station? Station { get; set; }
    public int TankId { get; set; }
    public Tank? Tank { get; set; }

    public NozzleHistory(int nozzleId, decimal volume, decimal amount, int stationId, int tankId)
    {
        NozzleId = nozzleId;
        Volume = volume;
        Amount = amount;    
        StationId = stationId;
        TankId = tankId;
    }
}