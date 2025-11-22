using AutoMapper;

namespace FuelMaster.HeadOffice.ApplicationService.Mappers;
{
    public class FuelMasterMapper : Profile
    {
        public FuelMasterMapper()
        {
            CreateMap<Station, StationResult>();
            CreateMap<City, CityResult>();
            CreateMap<ChangePricecDtoItem, ChangePricesDto>();
            CreateMap<Zone, StationZoneResult>();
            CreateMap<Zone, ZoneResult>();
            CreateMap<City, StationCityResult>();
            CreateMap<FuelType, FuelTypeResult>();
            CreateMap<Tank, TankResult>();
            CreateMap<Nozzle, NozzleResult>();
            CreateMap<Pump, PumpResult>();
            CreateMap<ZonePriceHistory, ZonePriceHistoryPaginationResult>()
            .ForMember(x => x.UserName, s => s.MapFrom(x => x.User!.UserName ?? ""))
            .ForMember(x => x.EmployeeName, s => s.MapFrom(x => x.User!.Employee!.FullName ?? ""))
            .ForMember(x => x.FuelType, s => s.MapFrom(x => x.ZonePrice!.FuelType!));
        }
    }
}