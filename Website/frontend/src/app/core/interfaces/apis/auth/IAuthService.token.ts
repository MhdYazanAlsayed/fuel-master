import { ServiceToken } from "../../../types/ServiceToken";
import IAuthService from "./IAuthService";

/**
 * Token for IAuthService interface
 * Use this token to register and retrieve IAuthService implementations
 */
export const IAuthServiceToken = ServiceToken.create<IAuthService>("IAuthService");

