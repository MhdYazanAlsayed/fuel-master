using FuelMaster.HeadOffice.Core.Models.Responses.Cities;
using FuelMaster.HeadOffice.Core.Models.Responses.Zones;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Station
{
    public class StationResponse
    {
        public int Id { get; set; }
        public string ArabicName { get; set; } = null!;
        public string EnglishName { get; set; } = null!;
        public CityResponse? City { get; set; }
        public ZoneResponse? Zone { get; set; }
    }

}
