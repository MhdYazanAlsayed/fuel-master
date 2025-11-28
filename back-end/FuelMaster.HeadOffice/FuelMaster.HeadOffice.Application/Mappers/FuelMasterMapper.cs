using AutoMapper;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

namespace FuelMaster.HeadOffice.Application.Mappers
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