import { toast } from 'react-toastify';

export default class ZoneService {
  _api = 'api/zones';

  constructor(httpService, languageService) {
    this._httpService = httpService;
    this._languageService = languageService;
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

    return { succeeded: true };
  }

  async editAsync(id, data) {
    const response = await this._httpService.putData(
      this._api + `/` + id,
      data
    );
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return { succeeded: false };
    }

    const entity = await response.json();
    return { succeeded: true, entity };
  }

  async deleteAsync(id) {
    const response = await this._httpService.deleteData(this._api + '/' + id);

    if (!response || !response.ok) {
      try {
        var resAsJson = await response.text();
        toast.error(resAsJson);
      } catch (error) {
        toast.error(this._languageService.resources.sthWentWrong);
      }

      return { succeeded: false };
    }

    return { succeeded: true };
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
}
