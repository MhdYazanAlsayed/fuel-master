import { Environment } from "../../core/enums/Environment";
import IHostEnvironment from "../../core/interfaces/host/IHostEnvironment";
import Settings from "../../core/appsettings.json";

export default class HostEnvironment implements IHostEnvironment {
  private readonly _environment: Environment =
    Settings.environment == "development"
      ? Environment.Development
      : Settings.environment == "production"
      ? Environment.Production
      : Settings.environment == "testing"
      ? Environment.Testing
      : Environment.Development;

  public get Environment(): Environment {
    return this._environment;
  }

  public get Api(): string {
    return this._environment === Environment.Development
      ? Settings.api.development
      : Settings.api.production;
  }

  public isDevelopment(): boolean {
    return this.Environment === Environment.Development;
  }

  public isProduction(): boolean {
    return this.Environment === Environment.Production;
  }

  public isTesting(): boolean {
    return this.Environment === Environment.Testing;
  }
}
