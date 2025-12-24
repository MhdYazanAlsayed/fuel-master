import WebService from 'app/core/abstracts/webService';
import Services from 'app/core/utilities/Services';

export default class ZonePriceService extends WebService {
  api = 'api/zones';

  constructor() {
    super();
    this._httpService = this.getService(Services.HttpService);
    this._languageService = this.getService(Services.LanguageService);
    this._logger = this.getService(Services.LoggerService);
  }

  async getPricesAsync(zoneId) {
    const response = await this._httpService.getData(
      this.api + '/' + zoneId + '/prices',
      false
    );

    if (!response || !response.ok) {
      return null;
    }

    return await response.json();
  }

  async changePriceAsync(zoneId, data) {
    const response = await this._httpService.putData(
      this.api + `/${zoneId}/prices`,
      { prices: data }
    );

    if (!response || !response.ok) {
      return { succeeded: false };
    }

    return { succeeded: true };
  }
}
