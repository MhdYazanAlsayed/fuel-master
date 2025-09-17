using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Tank : EntityBase<int>
    {
        public int StationId { get; set; }
        public Station? Station { get; set; }
        public FuelType FuelType { get; set; }
        public int Number { get; set; }

        // Full tank capacity (maximum fuel quantity in liters or cubic meters).
        public decimal Capacity { get; set; }

        // Maximum storage capacity (usually slightly less than Capacity for security reasons).
        public decimal MaxLimit { get; set; }

        // The minimum fuel level should be lowered before the "Tank Almost Empty" warning appears.
        public decimal MinLimit { get; set; }

        // Current fuel level in the tank (e.g. column height in cm).
        public decimal CurrentLevel { get; set; }

        // Actual fuel volume (in liters or m³).
        public decimal CurrentVolume { get; set; }
        public bool HasSensor { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<Nozzle>? Nozzles { get; set; }

        public Tank(int stationId, FuelType fuelType, int number, decimal capacity, decimal maxLimit, decimal minLimit, decimal currentLevel, decimal currentVolume, bool hasSensor)
        {
            StationId = stationId;
            FuelType = fuelType;
            Number = number;
            Capacity = capacity;
            MaxLimit = maxLimit;
            MinLimit = minLimit;
            CurrentLevel = currentLevel;
            CurrentVolume = currentVolume;
            HasSensor = hasSensor;
        }
    }
}
