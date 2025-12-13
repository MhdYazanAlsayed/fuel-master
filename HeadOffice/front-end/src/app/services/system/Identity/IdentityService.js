import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';

/**
 * IdentityService is a service that handles the identity of the user.
 * It is used to store the identity of the user and to get the access token and the identity of the user.
 * It is used to check if the user is authenticated and to logout the user.
 * It contains only methods to do operations on the frontend app. (it doesn't call apis at all)
 */
export default class IdentityService extends WebService {
  constructor() {
    super();
    this._authonticator = this.getService(Services.AuthenticatorService);
    this._localStorage = this.getService(Services.LocalStorageService);
    this._logger = this.getService(Services.LoggerService);
  }

  get accessToken() {
    return this._authonticator.accessToken;
  }

  get currentUser() {
    return this._authonticator.identity?.user;
  }

  logout() {
    this._authonticator.identity = null;
    this._localStorage.removeItem('authentication');
  }

  isAuthenticated() {
    return this._authonticator.isAuthenticated;
  }

  setIdentity(data) {
    this._authonticator.identity = data;
  }
}
