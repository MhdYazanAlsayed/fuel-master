export default class IdentityService {
  constructor(_authonticator, _localStorage) {
    this._authonticator = _authonticator;
    this._localStorage = _localStorage;
  }

  get accessToken() {
    return this._authonticator.accessToken;
  }

  get currentUser() {
    return this._authonticator.identity?.user ?? null;
  }

  logout() {
    this._authonticator.identity = null;
    this._localStorage.removeItem('authentication');
  }

  isAuthenticated() {
    return this._authonticator.isAuthenticated;
  }

  loadIdentity() {
    var data = this._localStorage.getItem('authentication');
    if (!data) return false;

    // Check the expiration date overhere ...

    this._authonticator.identity = data;
    return true;
  }

  setIdentity(data) {
    this._authonticator.identity = data;
  }
}
