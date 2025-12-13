import { toast } from 'react-toastify';

export default class ZonePriceService {
  api = 'api/zones';

  constructor(_httpService, _languageService) {
    this._languageService = _languageService;
    this._httpService = _httpService;
  }

  async getPricesAsync(zoneId) {
    const response = await this._httpService.getData(
      this.api + '/' + zoneId + '/prices',
      false
    );

    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
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
}
