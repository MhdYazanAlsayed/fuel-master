import Services from 'app/core/utilities/Services';
import WebService from 'app/core/abstracts/webService';

export default class LoggerService extends WebService {
  constructor() {
    super();
    this._hostEnviroment = this.getService(Services.HostEnviromentService);
    this.isDevelopment = this._hostEnviroment.isDevelopment();
  }

  logInformation(msg) {
    if (!this.isDevelopment) return;

    console.log(
      '%c[INFO]%c ' + msg,
      'background: #2196F3; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #2196F3;'
    );
  }
  logError(msg) {
    if (!this.isDevelopment) return;

    console.error(
      '%c[ERROR]%c ' + msg,
      'background: #F44336; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #F44336;'
    );
  }
  logWarning(msg) {
    if (!this.isDevelopment) return;

    console.warn(
      '%c[WARN]%c ' + msg,
      'background: #FF9800; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #FF9800;'
    );
  }
  logDebug(msg) {
    if (!this.isDevelopment) return;
    console.debug(
      '%c[DEBUG]%c ' + msg,
      'background: #9E9E9E; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #9E9E9E;'
    );
  }
  logTrace(msg) {
    if (!this.isDevelopment) return;
    console.trace(
      '%c[TRACE]%c ' + msg,
      'background: #9C27B0; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #9C27B0;'
    );
  }
  logFatal(msg) {
    if (!this.isDevelopment) return;
    console.error(
      '%c[FATAL]%c ' + msg,
      'background: #D32F2F; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #D32F2F; font-weight: bold;'
    );
  }
  logCritical(msg) {
    if (!this.isDevelopment) return;
    console.error(
      '%c[CRITICAL]%c ' + msg,
      'background: #B71C1C; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;',
      'color: #B71C1C; font-weight: bold;'
    );
  }
}
