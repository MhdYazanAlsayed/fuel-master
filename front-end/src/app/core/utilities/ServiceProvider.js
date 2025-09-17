import HostEnviroment from 'app/services/HostEnviroment';
import LanguageService from 'app/services/LanguageService';
import LocalStorageService from 'app/services/LocalStorageService';
import AutheticatorService from 'app/services/AuthenticatorService';
import LoggerService from 'app/services/LoggerService';
import LoadingService from 'app/services/LoadingService';
import HttpService from 'app/services/http/HttpService';
import UserService from 'app/services/UserService';
import IdentityService from 'app/services/IdentityService';
import CityService from 'app/services/management/CityService';
import TenantService from 'app/services/TenantService';
import ZoneService from 'app/services/management/zones/ZoneService';
import ZonePriceService from 'app/services/management/zones/ZonePriceService';
import ZonePriceHistory from 'app/services/management/zones/ZonePriceHistoryService';
import StationService from 'app/services/management/StationService';
import TankService from 'app/services/management/TankService';
import PumpSerivce from 'app/services/management/PumpService';
import NozzleService from 'app/services/management/NozzleService';
import DeliverySerivce from 'app/services/management/DeliverySerivce';
import EmployeeService from 'app/services/user-management/EmployeeService';
import FuelMasterGroupService from 'app/services/user-management/FuelMasterGroupService';
import ReportService from 'app/services/reports/ReportService';
import PermissionService from 'app/services/user-management/PermissionService';
import RoleManager from 'app/services/RoleManager';

export class ServiceProvider {
  hostEnvironment = new HostEnviroment();
  tenantService = new TenantService(this.hostEnvironment);
  localStorageService = new LocalStorageService(this.tenantService);
  languageService = new LanguageService(this.localStorageService);
  authenticatorService = new AutheticatorService();
  loggerService = new LoggerService(this.hostEnvironment);
  loadingService = new LoadingService();
  httpService = new HttpService(
    this.loggerService,
    this.authenticatorService,
    this.languageService,
    this.hostEnvironment,
    this.tenantService,
    this.loadingService
  );
  identityService = new IdentityService(
    this.authenticatorService,
    this.localStorageService
  );
  userService = new UserService(
    this.httpService,
    this.identityService,
    this.localStorageService,
    this.languageService
  );
  roleManager = new RoleManager(this.identityService);
  // Managements
  cityService = new CityService(this.httpService, this.languageService);
  zoneService = new ZoneService(this.httpService, this.languageService);
  zonePriceService = new ZonePriceService(
    this.httpService,
    this.languageService
  );
  zonePriceHistoryService = new ZonePriceHistory(
    this.httpService,
    this.languageService
  );
  stationService = new StationService(this.httpService, this.languageService);
  tankService = new TankService(this.httpService, this.languageService);
  pumpService = new PumpSerivce(this.httpService, this.languageService);
  nozzleService = new NozzleService(this.httpService, this.languageService);
  deliveryService = new DeliverySerivce(this.httpService, this.languageService);
  employeeSerivce = new EmployeeService(this.httpService, this.languageService);
  groupService = new FuelMasterGroupService(
    this.httpService,
    this.languageService
  );
  reportService = new ReportService(this.httpService, this.languageService);
  permissionService = new PermissionService(
    this.httpService,
    this.languageService
  );
}
