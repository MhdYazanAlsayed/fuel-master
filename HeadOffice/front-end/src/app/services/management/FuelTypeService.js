import { toast } from 'react-toastify';
import { useService } from 'hooks/useService';
import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';

export default class FuelTypeService extends WebService {
  _api = 'api/fuel-types';

  constructor() {
    super();
    this._httpService = this.getService(Services.HttpService);
    this._languageService = this.getService(Services.LanguageService);
  }

  async getAllAsync() {
    const response = await this._httpService.getData(this._api, false);

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return [];
    }

    return await response.json();
  }

  async createAsync(data) {
    const response = await this._httpService.postData(this._api, data);
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    const entity = await response.json();
    return { succeeded: true, data: entity };
  }

  async updateAsync(id, data) {
    const response = await this._httpService.putData(
      `${this._api}/${id}`,
      data
    );
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    const entity = await response.json();
    return { succeeded: true, data: entity };
  }
}
