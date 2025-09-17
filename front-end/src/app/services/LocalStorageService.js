export default class LocalStorageService {
  _prefix = 'fuel-master-';

  constructor(tenantService) {
    this._prefix += tenantService.tenant + '-';
  }

  getItem(key) {
    const item = localStorage.getItem(this._prefix + key);
    try {
      return JSON.parse(item);
    } catch {
      return item;
    }
  }

  setItem(key, data) {
    localStorage.setItem(this._prefix + key, JSON.stringify(data));
  }

  removeItem(key) {
    localStorage.removeItem(this._prefix + key);
  }
}
