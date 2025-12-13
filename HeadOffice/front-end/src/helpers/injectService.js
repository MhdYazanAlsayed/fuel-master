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
}
