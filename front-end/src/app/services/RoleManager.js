export default class RoleManager {
  constructor(_identityService) {
    this._identityService = _identityService;
  }

  check(permission) {
    if (this._identityService.currentUser?.userName?.toLowerCase() === 'admin')
      return true;

    const result = this._identityService?.currentUser?.permissions?.find(
      x => x.key === permission
    );

    return result?.value ?? false;
  }
}
