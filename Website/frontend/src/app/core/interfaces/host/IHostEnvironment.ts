import { Environment } from "../../enums/Environment";

export default interface IHostEnvironment {
  get Environment(): Environment;
  get Api(): string;
  isDevelopment(): boolean;
  isProduction(): boolean;
  isTesting(): boolean;
}
