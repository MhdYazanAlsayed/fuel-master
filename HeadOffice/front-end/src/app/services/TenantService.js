export default class TenantService {
  constructor(_hostEnvironment) {
    this._hostEnvironment = _hostEnvironment;
  }

  get tenant() {
    if (this._hostEnvironment.isTesting()) return 'test';
    
    let hostname = window.location.hostname;
    let tenant = hostname.slice(0, hostname.indexOf('.'));

    if (!tenant || tenant.trim() === '') throw Error();

    return tenant;
  }
}
