import { Environments } from 'app/core/enums/Environments';
import Settings from 'app/core/appsettings.json';
import SettingsDevelopment from 'app/core/appsettings.development.json';
import SettingsTesting from 'app/core/appsettings.testing.json';

export default class HostEnviroment {
  enviroment = Environments.Development;

  get api() {
    return this.isProduction()
      ? Settings.Api.Url
      : this.isDevelopment()
      ? SettingsDevelopment.Api.Url
      : this.isTesting()
      ? SettingsTesting.Api.Url
      : SettingsDevelopment.Api.Url;
  }

  isProduction() {
    return this.enviroment === Environments.Production;
  }

  isDevelopment() {
    return this.enviroment === Environments.Development;
  }

  isTesting() {
    return this.enviroment === Environments.Testing;
  }
}
