using FuelMaster.HeadOffice.Core.Models.Responses.Nozzles;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Pumps
{
    public class PumpResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int StationId { get; set; }
        public StationResponse? Station { get; set; }
        public string Manufacturer { get; set; } = null!;
        public List<NozzleResponse> Nozzles { get; set; } = null!;
    }
}
