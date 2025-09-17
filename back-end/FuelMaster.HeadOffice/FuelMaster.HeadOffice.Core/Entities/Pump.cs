namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Pump : EntityBase<int>
    {
        public int Number { get; set; }
        public int StationId { get; set; }
        public Station? Station { get; set; }
        public string Manufacturer { get; set; }

        public IEnumerable<Nozzle>? Nozzles { get; set; }

        public Pump(int number, int stationId, string manufacturer)
        {
            Number = number;
            StationId = stationId;
            Manufacturer = manufacturer;
        }
    }
}
