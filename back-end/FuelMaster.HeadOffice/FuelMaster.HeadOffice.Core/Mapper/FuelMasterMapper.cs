using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Pumps.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Tanks.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

namespace FuelMaster.HeadOffice.Core.Mapper
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
