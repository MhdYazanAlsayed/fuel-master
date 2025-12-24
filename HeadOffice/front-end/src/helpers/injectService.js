import DependenciesInjector from 'app/core/utilities/DependenciesInjector';
import Services from 'app/core/utilities/Services';
import HostEnviroment from 'app/services/system/host-environment/HostEnviroment';
import IdentityService from 'app/services/system/Identity/IdentityService';
import AuthenticatorService from 'app/services/AuthenticatorService';
import HttpService from 'app/services/system/http/HttpService';
import LoggerService from 'app/services/system/loggers/LoggerService';
import LanguageService from 'app/services/system/language/LanguageService';
import LocalStorageService from 'app/services/system/storage/LocalStorageService';
import UserService from 'app/services/UserService';
import PermissionService from 'app/services/user-management/area-of-access/PermissionService';
import AccessOfAreaService from 'app/services/user-management/area-of-access/AccessOfAreaService';
import ReportService from 'app/services/reports/ReportService';
import CityService from 'app/services/management/CityService';
import FuelTypeService from 'app/services/management/FuelTypeService';
import AreaService from 'app/services/management/AreaService';
import ZoneService from 'app/services/management/zones/ZoneService';
import ZonePriceService from 'app/services/management/zones/ZonePriceService';
import ZonePriceHistoryService from 'app/services/management/zones/ZonePriceHistoryService';
import StationService from 'app/services/management/StationService';
import TankService from 'app/services/management/TankService';
import PumpService from 'app/services/management/PumpService';
import NozzleService from 'app/services/management/NozzleService';
import EmployeeService from 'app/services/user-management/EmployeeService';
import RoleService from 'app/services/user-management/RoleService';

export function injectServices() {
  // Host environment service
  DependenciesInjector.services.addService(
    Services.HostEnviromentService,
    HostEnviroment
  );

  // Auth services
  DependenciesInjector.services.addService(
    Services.IdentityService,
    IdentityService
  );
  DependenciesInjector.services.addService(
    Services.AuthenticatorService,
    AuthenticatorService
  );

  // Http services
  DependenciesInjector.services.addService(Services.HttpService, HttpService);

  // Logger service
  DependenciesInjector.services.addService(
    Services.LoggerService,
    LoggerService
  );

  // Language service
  DependenciesInjector.services.addService(
    Services.LanguageService,
    LanguageService
  );

  // Local storage service
  DependenciesInjector.services.addService(
    Services.LocalStorageService,
    LocalStorageService
  );

  // User service
  DependenciesInjector.services.addService(Services.UserService, UserService);

  // Permission service
  DependenciesInjector.services.addService(
    Services.PermissionService,
    PermissionService
  );
  DependenciesInjector.services.addService(
    Services.AccessOfAreaService,
    AccessOfAreaService
  );

  // Report service
  DependenciesInjector.services.addService(
    Services.ReportService,
    ReportService
  );

  // City service
  DependenciesInjector.services.addService(Services.CityService, CityService);
  DependenciesInjector.services.addService(
    Services.FuelTypeService,
    FuelTypeService
  );
  DependenciesInjector.services.addService(Services.AreaService, AreaService);
  DependenciesInjector.services.addService(Services.ZoneService, ZoneService);
  DependenciesInjector.services.addService(
    Services.ZonePriceService,
    ZonePriceService
  );
  DependenciesInjector.services.addService(
    Services.ZonePriceHistoryService,
    ZonePriceHistoryService
  );
  DependenciesInjector.services.addService(
    Services.StationService,
    StationService
  );
  DependenciesInjector.services.addService(Services.TankService, TankService);
  DependenciesInjector.services.addService(Services.PumpService, PumpService);
  DependenciesInjector.services.addService(
    Services.NozzleService,
    NozzleService
  );
  DependenciesInjector.services.addService(
    Services.EmployeeService,
    EmployeeService
  );
  DependenciesInjector.services.addService(
    Services.RoleService,
    RoleService
  );
}
