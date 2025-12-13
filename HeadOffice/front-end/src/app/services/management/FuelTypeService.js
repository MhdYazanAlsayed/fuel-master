import { toast } from 'react-toastify';

export default class FuelTypeService {
  _api = 'api/fuel-types';

  constructor(httpService, languageService) {
    this._httpService = httpService;
    this._languageService = languageService;
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
