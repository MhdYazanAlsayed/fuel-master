import { toast } from 'react-toastify';

export default class ReportService {
  endpoint = 'api/reports';

  constructor(_httpService, _languageService) {
    this._httpService = _httpService;
    this._languageService = _languageService;
  }

  async getRealTimeAsync(stationId) {
    let _ = this.endpoint + '/realtime';
    if (stationId) _ += '?stationId=' + stationId;

    const response = await this._httpService.getData(_);
    if (!response || !response.ok) {
      toast.error(this._languageService.response.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async getTransactionsAsync(page, from, to, stationId, employeeId) {
    let _ = this.endpoint + '/transactions?page=' + page + '&';

    if (from) _ += 'from=' + new Date(from).toLocaleString() + '&';
    if (to) _ += 'to=' + new Date(to).toLocaleString() + '&';
    if (stationId) _ += 'stationId=' + stationId + '&';
    if (employeeId) _ += 'employeeId=' + employeeId;

    const response = await this._httpService.getData(_);
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async getDeliveriesAsync(page, from, to, stationId) {
    let _ = this.endpoint + '/deliveries?page=' + page + '&';

    if (from) _ += 'from=' + new Date(from).toLocaleString() + '&';
    if (to) _ += 'to=' + new Date(to).toLocaleString() + '&';
    if (stationId) _ += 'stationId=' + stationId;

    const response = await this._httpService.getData(_);
    if (!response || !response.ok) {
      console.log(await response.text());
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async getTimeReportAsync(from, to, stationId) {
    let _ = this.endpoint + '/time?';

    if (from) _ += 'from=' + new Date(from).toLocaleString() + '&';
    if (to) _ += 'to=' + new Date(to).toLocaleString() + '&';
    if (stationId) _ += 'stationId=' + stationId;

    const response = await this._httpService.getData(_);
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async getDashboardReports() {
    const response = await this._httpService.getData(
      this.endpoint + '/dashboard'
    );
    if (!response || !response.ok) {
      toast.error(this._languageService.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }
}
