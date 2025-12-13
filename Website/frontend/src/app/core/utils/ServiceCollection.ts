export default class ServiceCollection {
  private readonly services: Map<Services, any> = new Map();
  private readonly serviceRefs: Map<Services, any> = new Map();

  addService(service: Services, ref: any): void {
    this.serviceRefs.set(service, ref);
  }

  getService<T>(service: Services): any {
    var serviceInstance = this.services.get(service);
    if (!serviceInstance) {
      var ref = this.serviceRefs.get(service);
      if (!ref) {
        throw new Error(`Service ${service} not found`);
      }
      serviceInstance = new ref();
      this.services.set(service, serviceInstance);
    }
    return serviceInstance as T;
  }
}

export enum Services {
  AuthService = "AuthService",
  IdentityService = "IdentityService",
  LoggerService = "LoggerService",
  HttpService = "HttpService",
  HostEnvironment = "HostEnvironment",
  SubscriptionService = "SubscriptionService",
  TenantService = "TenantService",
}
