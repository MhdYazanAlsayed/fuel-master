import { createRoot } from "react-dom/client";
import App from "./App";
import "./index.css";
import DependeciesInjector from "./app/core/utils/DependeciesInjector";
import { Services } from "./app/core/utils/ServiceCollection";
import Logger from "./app/services/loggers/Logger";
import AuthService from "./app/services/auth/AuthService";
import IdentityService from "./app/services/identity/IdentityService";
import HttpService from "./app/services/http/HttpService";
import HostEnvironment from "./app/services/host/HostEnvironment";
import SubscriptionService from "./app/services/subscription/SubscriptionService";
import TenantService from "./app/services/tenant/TenantService";

DependeciesInjector.services.addService(Services.LoggerService, Logger);
DependeciesInjector.services.addService(Services.AuthService, AuthService);
DependeciesInjector.services.addService(Services.HttpService, HttpService);
DependeciesInjector.services.addService(
  Services.HostEnvironment,
  HostEnvironment
);
DependeciesInjector.services.addService(
  Services.IdentityService,
  IdentityService
);
DependeciesInjector.services.addService(
  Services.SubscriptionService,
  SubscriptionService
);
DependeciesInjector.services.addService(Services.TenantService, TenantService);

// Create the app
createRoot(document.getElementById("root")!).render(<App />);
