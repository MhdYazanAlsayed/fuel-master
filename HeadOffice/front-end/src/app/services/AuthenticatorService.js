/**
 * AuthenticatorService is a service that handles the authentication of the user.
 * It is used to store the authentication data and the identity of the user.
 * It is used to check if the user is authenticated and to get the access token and the identity of the user.
 * It is used to set the identity of the user and to logout the user.
 *! It is used by IdentityService only, not in components.
 */
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
