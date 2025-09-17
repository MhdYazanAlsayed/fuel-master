using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Tanks
{
    public class TankResponse
    {
        public int Id { get; set; }
        public StationResponse? Station { get; set; }
        public FuelType FuelType { get; set; }
        public int Number { get; set; }
        public decimal Capacity { get; set; }
        public decimal MaxLimit { get; set; }
        public decimal MinLimit { get; set; }
        public decimal CurrentLevel { get; set; }
        public decimal CurrentVolume { get; set; }
        public bool HasSensor { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
