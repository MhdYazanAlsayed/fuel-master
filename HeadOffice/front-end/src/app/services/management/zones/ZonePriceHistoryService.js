import { toast } from 'react-toastify';
import WebService from 'app/core/abstracts/webService';
import Services from 'app/core/utilities/Services';

export default class ZonePriceHistory extends WebService {
  api = 'api/zones/prices';

  constructor() {
    super();
    this._httpService = this.getService(Services.HttpService);
    this._languageService = this.getService(Services.LanguageService);
  }

  async getHistoriesPaginationAsync(zonePriceId, page) {
    const response = await this._httpService.getData(
      this.api + `/${zonePriceId}/histories?page=` + page,
      false
    );

    if (!response || !response.ok) {
      toast.error(this._languageService.response.sthWentWrong);
      return null;
    }

    return await response.json();
  }
}
