import { ServiceToken } from "../../types/ServiceToken";
import ILoggerService from "./ILoggerService";

export const ILoggerServiceToken =
  ServiceToken.create<ILoggerService>("ILoggerService");
