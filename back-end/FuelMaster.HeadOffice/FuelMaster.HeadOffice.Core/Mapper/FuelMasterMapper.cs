using AutoMapper;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Responses.Cities;
using FuelMaster.HeadOffice.Core.Models.Responses.Nozzles;
using FuelMaster.HeadOffice.Core.Models.Responses.Pumps;
using FuelMaster.HeadOffice.Core.Models.Responses.Reports;
using FuelMaster.HeadOffice.Core.Models.Responses.Station;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;
using FuelMaster.HeadOffice.Core.Models.Responses.ZonePriceHistories;
using FuelMaster.HeadOffice.Core.Models.Responses.Zones;

namespace FuelMaster.HeadOffice.Core.Mapper
{
    public class FuelMasterMapper : Profile
    {
        public FuelMasterMapper()
        {
            CreateMap<Tank, GetRealTimeReportResposne_Tanks>()
                .ForMember(x => x.CapacityPercent , src => src.MapFrom(x => x.CurrentVolume == 0 || x.Capacity == 0 ? 0 : x.CurrentVolume / x.Capacity
                ));

            CreateMap<ZonePriceHistory, ZonePriceHistoryPaginationResponse>()
                .ForMember(x => x.UserName, s => s.MapFrom(x => x.User!.UserName))
                .ForMember(x => x.EmployeeName, s => s.MapFrom(x => x.User!.Employee!.FullName))
                .ForMember(x => x.FuelType, s => s.MapFrom(x => x.ZonePrice!.FuelType));

            CreateMap<City, CityResponse>();
            CreateMap<Zone, ZoneResponse>();
            CreateMap<Station, StationResponse>();
            CreateMap<Tank, TankResponse>();
            CreateMap<Nozzle, NozzleResponse>();
            CreateMap<Pump, PumpResponse>();
        }
    }
}
