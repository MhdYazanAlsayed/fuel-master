import { ServiceToken } from "../../types/ServiceToken";
import IHostEnvironment from "./IHostEnvironment";

export const IHostEnvironmentToken =
  ServiceToken.create<IHostEnvironment>("IHostEnvironment");
