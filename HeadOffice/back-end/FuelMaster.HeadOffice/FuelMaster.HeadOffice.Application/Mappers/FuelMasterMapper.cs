using AutoMapper;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.AreaService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterAreaOfAccessService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.PumpService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.Cities.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Pricing.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Infrastructure.Authentication.UserService.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Helpers;

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

            CreateMap<ZonePrice , ZonePriceResult>();

            CreateMap<FuelMasterRole, FuelMasterRoleResult>();
            CreateMap<Station, StationResult>();
            CreateMap<Area, AreaResult>();
            CreateMap<Employee, EmployeeResult>();
            CreateMap<FuelMasterUser, FuelMasterUserResult>();
            CreateMap<FuelMasterAreaOfAccess, FuelMasterAreaOfAccessResult>();
            CreateMap<FuelMasterPermission, FuelMasterAreaOfAccessResult>()
            .ForMember(x => x.AreaOfAccess, s => s.MapFrom(x => x.AreaOfAccess!.AreaOfAccess))
            .ForMember(x => x.EnglishName, s => s.MapFrom(x => x.AreaOfAccess!.EnglishName))
            .ForMember(x => x.ArabicName, s => s.MapFrom(x => x.AreaOfAccess!.ArabicName))
            .ForMember(x => x.EnglishDescription, s => s.MapFrom(x => x.AreaOfAccess!.EnglishDescription))
            .ForMember(x => x.ArabicDescription, s => s.MapFrom(x => x.AreaOfAccess!.ArabicDescription));
            //CreateMap<TenantConfig, TenantConfigResponse>();
        }
    }
}