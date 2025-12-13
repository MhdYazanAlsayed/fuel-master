using FuelMaster.HeadOffice.Core.Helpers;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Pump : AggregateRoot<int>
    {
        public Pump(int number, int stationId, string? manufacturer)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Number must be greater than 0");
            }
            if (stationId <= 0)
            {
                throw new ArgumentException("Station id must be greater than 0");
            }
       

            Number = number;
            StationId = stationId;
            Manufacturer = manufacturer;
        }
        public int Number { get; private set; }
        public int StationId { get; private set; }
        public Station? Station { get; private set; }
        public string? Manufacturer { get; private set; }

        public List<Nozzle> _nozzles = new List<Nozzle>();
        public IReadOnlyList<Nozzle> Nozzles => _nozzles.AsReadOnly();

        public void Update(int number, int stationId, string? manufacturer)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Number must be greater than 0");
            }
            if (stationId <= 0)
            {
                throw new ArgumentException("Station id must be greater than 0");
            }

            Number = number;
            StationId = stationId;
            Manufacturer = manufacturer;
            UpdatedAt = DateTimeCulture.Now;
        }
    }
}
