import { Environments } from '../core/enums/Environments';

export default class HostEnviroment {
  enviroment = Environments.Development;

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
