import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';

export default class HttpService extends WebService {
  _api = '';

  constructor() {
    super();
    this._logger = this.getService(Services.LoggerService);
    this._authorization = this.getService(Services.IdentityService);
    this._language = this.getService(Services.LanguageService);
    this._hostEnviroment = this.getService(Services.HostEnviromentService);
    this._api = this._hostEnviroment.api;
  }

  async postData(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'include', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async patchData(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'PATCH', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'include', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'PATCH');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async putData(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'PUT', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'include', // include, *same-origin, omit
        headers: {
          'Content-Type': 'application/json',
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data === null ? null : JSON.stringify(data) // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'PUT');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  async postDataWithFile(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'include', // include, *same-origin, omit
        headers: {
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  async putDataWithFile(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'PUT', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'include', // include, *same-origin, omit
        headers: {
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, 'POST');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  async getData(url) {
    try {
      let response = await fetch(this._api + url, {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        }
      });

      this.logByStatus(response, url, 'GET');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  async deleteData(url, data) {
    try {
      let response = await fetch(this._api + url, {
        method: 'DELETE',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          'X-Language': this._language.isRTL ? 'ar' : 'en'
        },
        body: !data ? null : JSON.stringify(data)
      });

      this.logByStatus(response, url, 'DELETE');

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  logByStatus(response, url, verb) {
    if (response.status >= 200 && response.status < 300)
      this._logger.logInformation(`${verb} ${response.status} [${url}]`);
    else {
      this._logger.logError(`${verb} ${response.status} [${url}]`);
      response.text().then(text => {
        this._logger.logError(text);
      });
    }
  }
}
