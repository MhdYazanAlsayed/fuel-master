using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Tank : AggregateRoot<int>
    {
        public int StationId { get; private set; }
        public Station? Station { get; private set; }
        public int FuelTypeId { get; private set; }
        public FuelType? FuelType { get; private set; }
        public int Number { get; private set; }

        // Full tank capacity (maximum fuel quantity in liters or cubic meters).
        public decimal Capacity { get; private set; }

        // Maximum storage capacity (usually slightly less than Capacity for security reasons).
        public decimal MaxLimit { get; private set; }

        // The minimum fuel level should be lowered before the "Tank Almost Empty" warning appears.
        public decimal MinLimit { get; private set; }

        // Current fuel level in the tank (e.g. column height in cm).
        public decimal CurrentLevel { get; private set; }

        // Actual fuel volume (in liters or m³).
        public decimal CurrentVolume { get; private set; }
        public bool HasSensor { get; private set; }
        public List<Nozzle> _nozzles = new List<Nozzle>();
        public IReadOnlyList<Nozzle> Nozzles => _nozzles.AsReadOnly();

        public Tank(int stationId, int fuelTypeId, int number, decimal capacity, decimal maxLimit, decimal minLimit, decimal currentLevel, decimal currentVolume, bool hasSensor)
        {
            var result = Validate(capacity, maxLimit, minLimit, currentLevel, currentVolume, number, stationId, fuelTypeId);
            if (!result.Succeeded)
            {
                throw new ArgumentException(result.Message);
            }
           
            StationId = stationId;
            FuelTypeId = fuelTypeId;
            Number = number;
            Capacity = capacity;
            MaxLimit = maxLimit;
            MinLimit = minLimit;
            CurrentLevel = currentLevel;
            CurrentVolume = currentVolume;
            HasSensor = hasSensor;
        }

        public void Update(decimal capacity, decimal maxLimit, decimal minLimit, decimal currentLevel, decimal currentVolume, bool hasSensor)
        {
            var result = Validate(capacity, maxLimit, minLimit, currentLevel, currentVolume, Number, StationId, FuelTypeId);
            if (!result.Succeeded)
            {
                throw new ArgumentException(result.Message);
            }

            Capacity = capacity;
            MaxLimit = maxLimit;
            MinLimit = minLimit;
            CurrentLevel = currentLevel;
            CurrentVolume = currentVolume;
            HasSensor = hasSensor;
            UpdatedAt = DateTimeCulture.Now;
        }

        public void AddNozzle(int pumpId, int number, decimal amount, decimal volume, decimal totalizer, decimal price, string? readerNumber = null)
        {
            var nozzle = new Nozzle(Id, pumpId, number, amount, volume, totalizer, price, readerNumber);
            _nozzles.Add(nozzle);
        }

        private ResultDto Validate (decimal capacity, decimal maxLimit, decimal minLimit, decimal currentLevel, decimal currentVolume, int number, int stationId, int fuelTypeId)
        {
            if (capacity <= 0) return Results.Failure("Capacity must be greater than 0");
            if (maxLimit <= 0) return Results.Failure("Max limit must be greater than 0");
            if (minLimit <= 0) return Results.Failure("Min limit must be greater than 0");
            if (currentLevel <= 0) return Results.Failure("Current level must be greater than 0");
            if (currentVolume <= 0) return Results.Failure("Current volume must be greater than 0");
            if (number <= 0) return Results.Failure("Number must be greater than 0");
            if (stationId <= 0) return Results.Failure("Station id must be greater than 0");
            if (fuelTypeId <= 0) return Results.Failure("Fuel type id must be greater than 0");
            if (maxLimit >= capacity) return Results.Failure("Max limit must be less than capacity");
            if (minLimit >= maxLimit) return Results.Failure("Min limit must be less than max limit");
            if (currentVolume > maxLimit) return Results.Failure("Current volume must be less than max limit");
            if (currentVolume < minLimit) return Results.Failure("Current volume must be greater than min limit");
            if (currentLevel > maxLimit) return Results.Failure("Current level must be less than max limit");
            if (currentLevel < minLimit) return Results.Failure("Current level must be greater than min limit");
            
            return Results.Success();
        }
     
    }
}
