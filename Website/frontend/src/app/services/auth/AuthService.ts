import LoginDto from "../../core/interfaces/apis/auth/dtos/LoginDto";
import RegisterDto from "../../core/interfaces/apis/auth/dtos/RegisterDto";
import type IAuthService from "../../core/interfaces/apis/auth/IAuthService";
import AuthCheckResponse from "../../core/interfaces/apis/auth/results/AuthCheckResponse";
import type IHttpService from "../../core/interfaces/http/IHttpService";
import type IIdentityService from "../../core/interfaces/identity/IIdentityService";
import type ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import { Services } from "../../core/utils/ServiceCollection";
import DependeciesInjector from "../../core/utils/DependeciesInjector";
import { ResultDto } from "../../core/dtos/ResultDto";

export default class AuthService implements IAuthService {
  private readonly _httpService: IHttpService;
  private readonly _identityService: IIdentityService;
  private readonly _logger: ILoggerService;

  constructor() {
    this._httpService = DependeciesInjector.services.getService<IHttpService>(
      Services.HttpService
    );
    this._identityService =
      DependeciesInjector.services.getService<IIdentityService>(
        Services.IdentityService
      );
    this._logger = DependeciesInjector.services.getService<ILoggerService>(
      Services.LoggerService
    );
  }

  async register(registerDto: RegisterDto): Promise<ResultDto> {
    try {
      const response = await this._httpService.postData<RegisterDto>(
        "/api/auth/register",
        registerDto
      );

      if (!response) {
        return {
          success: false,
          message: "Failed to register: No response from server",
        };
      }

      if (!response.ok) {
        // Handle error responses (400, 500, etc.)
        let errorMessage = "Registration failed. Please try again.";

        try {
          const errorData = await response.json();

          // Handle 400 Bad Request - Validation errors
          if (response.status === 400) {
            // Check if there are field-specific validation errors
            if (errorData.errors && typeof errorData.errors === "object") {
              // Combine all validation errors into a single message
              const validationErrors: string[] = [];
              Object.keys(errorData.errors).forEach((field) => {
                const fieldErrors = errorData.errors[field];
                if (Array.isArray(fieldErrors)) {
                  validationErrors.push(...fieldErrors);
                } else if (typeof fieldErrors === "string") {
                  validationErrors.push(fieldErrors);
                }
              });

              if (validationErrors.length > 0) {
                errorMessage = validationErrors.join(". ");
              } else if (errorData.message) {
                errorMessage = errorData.message;
              }
            } else if (errorData.message) {
              errorMessage = errorData.message;
            }
          } else if (response.status === 500) {
            // Handle 500 Internal Server Error
            errorMessage =
              errorData.message ||
              "An error occurred on the server. Please try again later.";
          } else if (errorData.message) {
            errorMessage = errorData.message;
          }
        } catch (parseError) {
          // If we can't parse the error response, use a default message
          this._logger.logError(
            `Failed to parse error response: ${parseError}`
          );
        }

        this._logger.logError(`Registration failed: ${errorMessage}`);
        return {
          success: false,
          message: errorMessage,
        };
      }

      // Success response (200 OK)
      const authStatus: AuthCheckResponse = await response.json();

      // Store auth status (subscription/tenant info) and token expiration
      // The token is automatically set in secure HTTP-only cookie by the server
      this._identityService.setAuthStatus(authStatus);

      this._logger.logInformation("User registered successfully");
      return { success: true };
    } catch (error) {
      const errorMessage =
        error instanceof Error
          ? error.message
          : "Registration failed. Please try again.";
      this._logger.logError(`Registration error: ${errorMessage}`);
      return {
        success: false,
        message: errorMessage,
      };
    }
  }

  async login(loginDto: LoginDto): Promise<ResultDto> {
    try {
      const response = await this._httpService.postData<LoginDto>(
        "/api/auth/login",
        loginDto
      );

      if (!response) {
        return { success: false };
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        return { success: false };
      }

      const authStatus: AuthCheckResponse = await response.json();

      // Store auth status (subscription/tenant info) and token expiration
      this._identityService.setAuthStatus(authStatus);

      this._logger.logInformation("User logged in successfully");
      return { success: true };
    } catch (error) {
      this._logger.logError(`Login error: ${error}`);
      return { success: false };
    }
  }
}
