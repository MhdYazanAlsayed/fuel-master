import { toast } from 'react-toastify';
import WebService from 'app/core/abstracts/webService';
import Services from 'app/core/utilities/Services';

export default class AccessOfAreaService extends WebService {
  _api = 'api/areas-of-access';

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
}
