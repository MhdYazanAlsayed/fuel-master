import Settings from "../../core/appsettings.json";
import type IHostEnvironment from "../../core/interfaces/host/IHostEnvironment";
import type IHttpService from "../../core/interfaces/http/IHttpService";
import type ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import DependeciesInjector from "../../core/utils/DependeciesInjector";
import { Services } from "../../core/utils/ServiceCollection";

export default class HttpService implements IHttpService {
  private readonly _logger: ILoggerService;
  private readonly _hostEnviroment: IHostEnvironment;
  public readonly _api = Settings.api.development;

  constructor() {
    this._logger = DependeciesInjector.services.getService<ILoggerService>(
      Services.LoggerService
    );
    this._hostEnviroment =
      DependeciesInjector.services.getService<IHostEnvironment>(
        Services.HostEnvironment
      );
    this._api = this._hostEnviroment.Api;
  }

  async postData<T>(url: string, data?: T) {
    try {
      let response = await fetch(this._api + url, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "include", // include, *same-origin, omit
        headers: {
          "Content-Type": "application/json",
          language: "en",
          // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null, // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, "POST");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async patchData<T>(url: string, data?: T) {
    try {
      let response = await fetch(this._api + url, {
        method: "PATCH", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "include", // include, *same-origin, omit
        headers: {
          "Content-Type": "application/json",
          // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data ? JSON.stringify(data) : null, // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, "PATCH");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async putData<T>(url: string, data: T): Promise<Response | null> {
    try {
      let response = await fetch(this._api + url, {
        method: "PUT", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "include", // include, *same-origin, omit
        headers: {
          "Content-Type": "application/json",
          // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data), // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, "PUT");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);

      return null;
    }
  }

  async postDataWithFile(url: string, data: FormData) {
    try {
      let response = await fetch(this._api + url, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "include", // include, *same-origin, omit
        headers: {},
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data, // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, "POST");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async putDataWithFile(url: string, data: FormData): Promise<Response | null> {
    try {
      let response = await fetch(this._api + url, {
        method: "PUT", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "include", // include, *same-origin, omit
        headers: {},
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data, // body data type must match "Content-Type" header
      });

      this.logByStatus(response, url, "PUT");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async getData(url: string): Promise<Response | null> {
    try {
      let response = await fetch(this._api + url, {
        method: "GET",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
      });

      this.logByStatus(response, url, "GET");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  async deleteData<T>(url: string, data?: T) {
    try {
      let response = await fetch(this._api + url, {
        method: "DELETE",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: !data ? null : JSON.stringify(data),
      });

      this.logByStatus(response, url, "DELETE");

      return response;
    } catch (err) {
      this._logger.logError(`error [${url}]: ${err}`);
      return null;
    }
  }

  private logByStatus(response: Response, url: string, verb: string) {
    if (response.status === 200)
      this._logger.logInformation(`${verb} ${response.status} [${url}]`);
    else this._logger.logError(`${verb} ${response.status} [${url}]`);
  }
}
