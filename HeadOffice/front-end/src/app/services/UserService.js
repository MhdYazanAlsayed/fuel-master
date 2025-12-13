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
    this._authenticatorService = this.getService(Services.AuthenticatorService);
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
        stationId: responseAsJson.stationId,
        areaId: responseAsJson.areaId,
        cityId: responseAsJson.cityId
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

  async checkAuthHealth() {
    const storedData = this._localStorage.getItem('authentication');
    const response = await this._httpService.getData('/auth/health');
    if (!response || !response.ok || !storedData) {
      this._logger.logError('Failed to check authentication health');
      this._localStorage.removeItem('authentication');
      this._authonticator.identity = null;
      return false;
    }

    this._logger.logInfo('Authentication health checked successfully');
    return true;
  }

  async logoutAsync() {
    const response = await this._httpService.getData(this.endpoint + '/logout');

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      this._logger.logError('Failed to logout user');
      return { succeeded: false };
    }

    this._identityService.logout();
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
