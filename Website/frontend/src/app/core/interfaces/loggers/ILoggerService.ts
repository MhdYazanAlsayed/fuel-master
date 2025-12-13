export default interface ILoggerService {
  logInformation(message: string): void;
  logWarning(message: string): void;
  logError(message: string): void;
  logDebug(message: string): void;
  logTrace(message: string): void;
  logFatal(message: string): void;
  logCritical(message: string): void;
}
