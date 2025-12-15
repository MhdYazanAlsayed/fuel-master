import { toast } from 'react-toastify';
import WebService from 'app/core/abstracts/webService';
import Services from 'app/core/utilities/Services';

export default class UserService extends WebService {
  endpoint = 'api/auth';

  constructor() {
    super();
    this._httpService = this.getService(Services.HttpService);
    this._identityService = this.getService(Services.IdentityService);
    this._localStorage = this.getService(Services.LocalStorageService);
    this._languageService = this.getService(Services.LanguageService);
    this._logger = this.getService(Services.LoggerService);
  }

  async loginAsync({ userName, password, rememberMe }) {
    const response = await this._httpService.postData(
      this.endpoint + '/login',
      {
        userName,
        password
      }
    );

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.loginFaild);
      return { succeeded: false };
    }

    let responseAsJson = await response.json();
    if (!responseAsJson) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    if (responseAsJson.succeeded === false) {
      toast.error(this._languageService.resources.loginFaild);
      return { succeeded: false };
    }

    let data = {
      user: {
        userName: responseAsJson.userName,
        fullName: responseAsJson.fullName,
        email: responseAsJson.email,
        scope: responseAsJson.scope,
        areasOfAccess: responseAsJson.areasOfAccess
      }
    };

    if (rememberMe) {
      this._localStorage.setItem('authentication', {
        ...data,
        expirationDate: null
      });
    }

    this._identityService.setIdentity(data);

    toast.success(this._languageService.resources.loginSuccessfully);
    return { succeeded: true };
  }

  async checkAuthHealthAsync() {
    const response = await this._httpService.getData(this.endpoint + '/health');
    if (!response || !response.ok) {
      this._logger.logError('Failed to check authentication health');
      this._localStorage.removeItem('authentication');
      this._identityService.setIdentity(null);
      return false;
    }

    var updatedData = await response.json();

    const data = {
      user: updatedData
    };
    this._localStorage.setItem('authentication', data);
    this._identityService.setIdentity(data);

    this._logger.logInformation('Authentication health checked successfully');
    return true;
  }

  async logoutAsync() {
    console.log('logoutAsync');
    this._identityService.logout();

    const response = await this._httpService.getData(this.endpoint + '/logout');

    if (!response || !response.ok) {
      this._logger.logError('Failed to logout user');
      return { succeeded: false };
    }

    this._logger.logInformation('User logged out successfully');

    return { succeeded: true };
  }

  async changePasswordAsync(data) {
    const response = await this._httpService.postData(
      this.endpoint + '/update-password',
      data
    );

    if (!response || !response.ok) {
      const error = await response.text();
      toast.error(error ?? this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    toast.success(this._languageService.resources.taskSuccessfully);
    return { succeeded: true };
  }
}
