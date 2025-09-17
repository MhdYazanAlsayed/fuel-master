export default class LoggerService {
  constructor(_hostEnviroment) {
    this._hostEnviroment = _hostEnviroment;
  }

  logInformation(msg) {
    if (!this._hostEnviroment.isDevelopment()) return;

    console.log('%c' + msg, 'color: #2196f3;');
  }
  logError(msg) {
    if (!this._hostEnviroment.isDevelopment()) return;

    console.log('%c' + msg, 'color: #db2a22;');
  }
  logWarning(msg) {
    if (!this._hostEnviroment.isDevelopment()) return;

    console.log('%c' + msg, 'color: yellow;');
  }
}
