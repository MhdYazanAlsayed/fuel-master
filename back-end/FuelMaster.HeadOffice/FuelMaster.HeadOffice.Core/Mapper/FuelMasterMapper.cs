using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.City.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Station.Results;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Zones.Results;
using FuelMaster.HeadOffice.Core.Contracts.Services.PricingService;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Mapper
{
    public class FuelMasterMapper : Profile
    {
        public FuelMasterMapper()
        {
            CreateMap<Station, StationResult>();
            CreateMap<City, CityResult>();
            CreateMap<ChangePricecDtoItem, ChangePricesDto>();
            CreateMap<ZonePriceHistory, ZonePriceHistoryPaginationResult>()
            .ForMember(x => x.UserName, s => s.MapFrom(x => x.User!.UserName ?? ""))
            .ForMember(x => x.EmployeeName, s => s.MapFrom(x => x.User!.Employee!.FullName ?? ""))
            .ForMember(x => x.FuelType, s => s.MapFrom(x => x.ZonePrice!.FuelType!));
        }
    }
}
