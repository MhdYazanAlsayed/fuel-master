import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';
import { AreaOfAccess } from 'app/core/helpers/AreaOfAccess';

export default class PermissionService extends WebService {
  constructor() {
    super();
    this._identityService = this.getService(Services.IdentityService);
  }

  check(areaOfAccess) {
    if (
      this._identityService.currentUser?.areasOfAccess.includes(
        AreaOfAccess.ALL
      )
    )
      return true;

    return this._identityService.currentUser?.areasOfAccess?.includes(
      areaOfAccess
    );
  }
}
