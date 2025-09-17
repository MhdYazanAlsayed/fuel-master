import { toast } from 'react-toastify';

export default class UserService {
  endpoint = 'api/account';

  constructor(_httpService, _identityService, _localStorage, _languageService) {
    this._httpService = _httpService;
    this._identityService = _identityService;
    this._localStorage = _localStorage;
    this._languageService = _languageService;
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
      accessToken: responseAsJson.accessToken,
      refreshToken: responseAsJson.refreshToken,
      user: {
        userName: responseAsJson.data.userName,
        fullName: responseAsJson.data.fullName,
        stationId: responseAsJson.data.stationId,
        permissions: responseAsJson.data.permissions
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
