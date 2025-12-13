import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

export default class WebService {
  getService(service) {
    return DependenciesInjector.services.getService(service);
  }
}
