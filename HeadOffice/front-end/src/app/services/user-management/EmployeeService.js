import { toast } from 'react-toastify';
import WebService from 'app/core/abstracts/webService';
import Services from 'app/core/utilities/Services';

export default class EmployeeService extends WebService {
  _api = 'api/employees';

  constructor() {
    super();
    this._httpService = this.getService(Services.HttpService);
    this._languageService = this.getService(Services.LanguageService);
  }

  async getAllAsync(stationId) {
    let _ = this._api + '?';
    if (stationId) _ += 'stationId=' + stationId;

    const response = await this._httpService.getData(_, false);

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async getPaginationAsync(currentPage) {
    const response = await this._httpService.getData(
      this._api + '/pagination?page=' + currentPage,
      false
    );

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async detailsAsync(id) {
    const response = await this._httpService.getData(
      this._api + '/' + id,
      false
    );

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async createAsync(data) {
    if (data.password.trim() !== data.confirmPassword.trim()) {
      toast.error(this._languageService.resources.passwordsDontMatch);
      return { succeeded: false };
    }

    if (data.password.trim().length < 6) {
      toast.error(this._languageService.resources.passwordIsShort);
      return { succeeded: false };
    }

    const response = await this._httpService.postData(this._api, data);
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    return { succeeded: true };
  }

  async editAsync(id, data) {
    const response = await this._httpService.putData(
      this._api + `/` + id,
      data
    );
    if (!response || !response.ok) {
      try {
        const resObject = await response.text();
        toast.error(resObject);
      } catch {
        toast.error(this._languageService.resources.sthWentWrong);
      }

      return { succeeded: false };
    }

    return { succeeded: true };
  }

  async editPasswordAsync(id, data) {
    const response = await this._httpService.putData(
      this._api + `/` + id + '/edit-password',
      data
    );
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    return { succeeded: true };
  }
}
