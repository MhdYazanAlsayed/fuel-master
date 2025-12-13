import { toast } from 'react-toastify';

export default class ZonePriceHistory {
  api = 'api/zones/prices';

  constructor(_httpService, _languageService) {
    this._languageService = _languageService;
    this._httpService = _httpService;
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
