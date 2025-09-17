namespace FuelMaster.HeadOffice.Core.Entities
{
    public class TankTransaction : EntityBase<int>
    {
        public int TankId { get; set; }
        public Tank? Tank { get; set; }
        public decimal CurrentVolume { get; set; }
        public decimal CurrentLevel { get; set; }

        public TankTransaction(int tankId, decimal currentVolume, decimal currentLevel)
        {
            TankId = tankId;
            CurrentVolume = currentVolume;
            CurrentLevel = currentLevel;
        }
    }
}
