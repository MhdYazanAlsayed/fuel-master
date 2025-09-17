import { toast } from 'react-toastify';

export default class PermissionService {
  endpoint = 'api/groups';

  constructor(_httpService, _languageSerivce) {
    this._httpService = _httpService;
    this._languageSerivce = _languageSerivce;
  }

  async getAsync(groupId) {
    const response = await this._httpService.getData(
      this.endpoint + '/' + groupId + '/permissions',
      false
    );
    if (!response || !response.ok) {
      toast.error(this._languageSerivce.resources.sthWentWrong);
      return null;
    }

    return await response.json();
  }

  async updateAsync(groupId, permissions) {
    const response = await this._httpService.putData(
      this.endpoint + '/' + groupId + '/permissions',
      { permissions }
    );
    if (!response || !response.ok) {
      toast.error(this._languageSerivce.resources.sthWentWrong);
      return { succeeded: false };
    }

    return { succeeded: true };
  }
}
