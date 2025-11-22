import Settings from '../../core/appsettings.json';

export default class HttpService {
  _api = Settings.Api.Development;

  constructor(
    _logger,
    _authorization,
    _language,
    _hostEnviroment,
    _tenantService,
    _loadingService
  ) {
    this._logger = _logger;
    this._authorization = _authorization;
    this._language = _language;
    this._hostEnviroment = _hostEnviroment;
    this._tenantService = _tenantService;
    this._api = Settings.Api[_hostEnviroment.enviroment];
    this._loadingService = _loadingService;
  }

  async postData(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Tenant-ID': this._tenantService.tenant,
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          // 'Content-Type': 'application/x-www-form-urlencoded',
          Authorization:
            this._authorization.accessToken != null
              ? `Bearer ${this._authorization.accessToken}`
              : 'none'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');

      this.stopLoading(loading);

      return response;
    } catch (err) {
      this.stopLoading(loading);

      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async patchData(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'PATCH', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Tenant-ID': this._tenantService.tenant,
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          // 'Content-Type': 'application/x-www-form-urlencoded',
          Authorization: this._authorization.accessToken
            ? `Bearer ${this._authorization.accessToken}`
            : 'none'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'PATCH');

      this.stopLoading(loading);

      return response;
    } catch (err) {
      this.stopLoading(loading);

      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async putData(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'PUT', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Tenant-ID': this._tenantService.tenant,
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          // 'Content-Type': 'application/x-www-form-urlencoded',
          Authorization: this._authorization.accessToken
            ? `Bearer ${this._authorization.accessToken}`
            : 'none'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data === null ? null : JSON.stringify(data) // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'PUT');
      this.stopLoading(loading);

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      this.stopLoading(loading);

      return null;
    }
  }

  async postDataWithFile(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          'X-Tenant-ID': this._tenantService.tenant,
          Authorization:
            this._authorization.accessToken != null
              ? `Bearer ${this._authorization.accessToken}`
              : 'none'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');
      this.stopLoading(loading);

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      this.stopLoading(loading);

      return null;
    }
  }

  async putDataWithFile(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'PUT', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          'X-Tenant-ID': this._tenantService.tenant,
          Authorization:
            this._authorization.accessToken != null
              ? `Bearer ${this._authorization.accessToken}`
              : 'none'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');
      this.stopLoading(loading);

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      this.stopLoading(loading);

      return null;
    }
  }

  async getData(url, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'X-Tenant-ID': this._tenantService.tenant,
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          Authorization: this._authorization.accessToken
            ? `Bearer ${this._authorization.accessToken}`
            : 'none'
        }
      });

      this.logByStatus(response, url, 'GET');
      this.stopLoading(loading);

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      this.stopLoading(loading);

      return null;
    }
  }

  async deleteData(url, data, loading = true) {
    try {
      this.runLoading(loading);

      let response = await fetch(this._api + url, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'X-Tenant-ID': this._tenantService.tenant,
          'X-Language': this._language.isRTL ? 'ar' : 'en',
          Authorization:
            this._authorization.accessToken != null
              ? `Bearer ${this._authorization.accessToken}`
              : 'none'
        },
        body: !data ? null : JSON.stringify(data)
      });

      this.logByStatus(response, url, 'DELETE');
      this.stopLoading(loading);

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      this.stopLoading(loading);

      return null;
    }
  }

  logByStatus(response, url, verb) {
    if (response.status === 200)
      this._logger.logInformation(`${verb} ${response.status} [${url}]`);
    else {
      this._logger.logError(`${verb} ${response.status} [${url}]`);
      response.text().then(text => {
        this._logger.logError(text);
      });
    }
  }

  runLoading(loading) {
    if (loading) this._loadingService.setLoading(true);
  }

  stopLoading(loading) {
    if (loading) this._loadingService.setLoading(false);
  }
}
