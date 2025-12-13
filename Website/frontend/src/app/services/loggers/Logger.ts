import IHostEnvironment from "../../core/interfaces/host/IHostEnvironment";
import ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import DependeciesInjector from "../../core/utils/DependeciesInjector";
import { Services } from "../../core/utils/ServiceCollection";

export default class Logger implements ILoggerService {
  private readonly _hostEnvironment: IHostEnvironment;
  constructor() {
    this._hostEnvironment =
      DependeciesInjector.services.getService<IHostEnvironment>(
        Services.HostEnvironment
      );
  }

  logInformation(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;

    console.log(
      "%c[INFO]%c " + message,
      "background: #2196F3; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #2196F3;"
    );
  }
  logWarning(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.warn(
      "%c[WARN]%c " + message,
      "background: #FF9800; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #FF9800;"
    );
  }
  logError(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.error(
      "%c[ERROR]%c " + message,
      "background: #F44336; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #F44336;"
    );
  }
  logDebug(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.debug(
      "%c[DEBUG]%c " + message,
      "background: #9E9E9E; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #9E9E9E;"
    );
  }
  logTrace(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.trace(
      "%c[TRACE]%c " + message,
      "background: #9C27B0; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #9C27B0;"
    );
  }
  logFatal(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.error(
      "%c[FATAL]%c " + message,
      "background: #D32F2F; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #D32F2F; font-weight: bold;"
    );
  }
  logCritical(message: string): void {
    if (!this._hostEnvironment.isDevelopment()) return;
    console.error(
      "%c[CRITICAL]%c " + message,
      "background: #B71C1C; color: white; padding: 2px 6px; border-radius: 3px; font-weight: bold;",
      "color: #B71C1C; font-weight: bold;"
    );
  }
}
