import DependenciesInjector from 'app/core/utilities/DependenciesInjector';

export function useService(service) {
  return DependenciesInjector.services.getService(service);
}
