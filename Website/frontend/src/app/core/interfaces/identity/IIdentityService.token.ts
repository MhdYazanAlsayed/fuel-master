import { ServiceToken } from "../../types/ServiceToken";
import IIdentityService from "./IIdentityService";

/**
 * Token for IIdentityService interface
 * Use this token to register and retrieve IIdentityService implementations
 */
export const IIdentityServiceToken = ServiceToken.create<IIdentityService>("IIdentityService");

