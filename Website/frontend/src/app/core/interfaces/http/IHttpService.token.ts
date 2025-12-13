import { ServiceToken } from "../../types/ServiceToken";
import IHttpService from "./IHttpService";

/**
 * Token for IIdentityService interface
 * Use this token to register and retrieve IIdentityService implementations
 */
export const IHttpServiceToken =
  ServiceToken.create<IHttpService>("IHttpService");
