namespace FuelMaster.HeadOffice.Core.Entities.Configs.Tanks.Exceptions
{
    public class TankNotFoundException : Exception
    {
        public TankNotFoundException(string message) : base(message)
        {
        }
    }
}