export default class AutheticatorService {
  isAuthenticated = false;
  _identity = null;

  get accessToken() {
    return this._identity?.accessToken ?? null;
  }

  get identity() {
    return this._identity;
  }

  set identity(data) {
    this.isAuthenticated = data !== null;
    this._identity = data;
  }
}
