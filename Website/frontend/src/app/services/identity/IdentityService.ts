import type IIdentityService from "../../core/interfaces/identity/IIdentityService";
import User from "../../core/entities/users/User";
import type ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import type IHttpService from "../../core/interfaces/http/IHttpService";
import AuthCheckResponse from "../../core/interfaces/apis/auth/results/AuthCheckResponse";
import { Services } from "../../core/utils/ServiceCollection";
import DependeciesInjector from "../../core/utils/DependeciesInjector";

const USER_STORAGE_KEY = "fuelmaster_user";
const TOKEN_EXPIRATION_KEY = "fuelmaster_token_expires_at";
const AUTH_STATUS_KEY = "fuelmaster_auth_status";

export default class IdentityService implements IIdentityService {
  private _currentUser: User | null = null;
  private _authStatus: AuthCheckResponse | null = null;
  private readonly _logger: ILoggerService;

  constructor() {
    this._logger = DependeciesInjector.services.getService<ILoggerService>(
      Services.LoggerService
    );
    this.loadUserFromStorage();
    this.loadAuthStatusFromStorage();
  }

  private get httpService(): IHttpService {
    return DependeciesInjector.services.getService<IHttpService>(
      Services.HttpService
    );
  }

  getCurrentUser(): User | null {
    return this._currentUser;
  }

  // async setUser(user: User, expiresAt: string): Promise<void> {
  //   this._currentUser = user;

  //   try {
  //     localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
  //     localStorage.setItem(TOKEN_EXPIRATION_KEY, expiresAt);

  //     this._logger.logInformation("User information stored successfully");
  //   } catch (error) {
  //     this._logger.logError(`Failed to store user information: ${error}`);
  //     throw error;
  //   }
  // }

  /**
   * Set authentication status from register/login responses
   * Stores auth status, updates current user, and persists to localStorage
   * @param authStatus - The authentication status including subscription/tenant info
   * @param expiresAt - Optional token expiration time (required for token expiration checking)
   */
  setAuthStatus(authStatus: AuthCheckResponse): void {
    // Store auth status
    this._authStatus = authStatus;

    // Update user info from auth status
    if (authStatus.user) {
      this._currentUser = authStatus.user;
      try {
        localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(authStatus.user));
      } catch (error) {
        this._logger.logError(`Failed to store user: ${error}`);
      }
    }

    // // Store token expiration if provided (needed for token expiration checking)
    // if (authStatus.expiresAt) {
    //   try {
    //     localStorage.setItem(TOKEN_EXPIRATION_KEY, authStatus.expiresAt);
    //   } catch (error) {
    //     this._logger.logError(`Failed to store token expiration: ${error}`);
    //   }
    // }

    // Store auth status in localStorage
    try {
      localStorage.setItem(AUTH_STATUS_KEY, JSON.stringify(authStatus));
      this._logger.logInformation("Authentication status stored successfully");
    } catch (error) {
      this._logger.logError(`Failed to store auth status: ${error}`);
    }
  }

  clearUser(): void {
    this._currentUser = null;
    this._authStatus = null;
    localStorage.removeItem(USER_STORAGE_KEY);
    // localStorage.removeItem(TOKEN_EXPIRATION_KEY);
    localStorage.removeItem(AUTH_STATUS_KEY);
    this._logger.logInformation("User information cleared");
  }

  checkTokenExpiration(): boolean {
    const expiresAt = localStorage.getItem(TOKEN_EXPIRATION_KEY);

    console.log("expiresAt", expiresAt);

    if (!expiresAt) {
      return false; // No token expiration stored
    }

    try {
      const expirationDate = new Date(expiresAt);
      const now = new Date();

      // Check if token is expired (with 1 minute buffer)
      const isExpired = expirationDate.getTime() <= now.getTime() + 60000; // 1 minute buffer

      if (isExpired) {
        this._logger.logInformation("Token has expired");
        this.clearUser();
        return false;
      }

      return true;
    } catch (error) {
      this._logger.logError(`Error checking token expiration: ${error}`);
      this.clearUser();
      return false;
    }
  }

  isAuthenticated(): boolean {
    // Reload user from storage to ensure we have the latest state
    this.loadUserFromStorage();

    if (!this._currentUser) {
      return false;
    }

    // Check if token is still valid
    // return this.checkTokenExpiration();
    return true;
  }

  /**
   * Check authentication status by calling the API
   * @returns AuthCheckResponse if authenticated, null otherwise
   */
  async checkAuthentication(): Promise<AuthCheckResponse | null> {
    try {
      const response = await this.httpService.getData("/api/auth/check");

      if (!response) {
        this._logger.logError(
          "Failed to check authentication: No response from server"
        );
        return null;
      }

      if (response.status === 200) {
        // Parse the full response
        const authStatus: AuthCheckResponse = await response.json();

        // Store auth status
        this._authStatus = authStatus;

        // Update user info from response
        if (authStatus.user) {
          this._currentUser = authStatus.user;
          try {
            localStorage.setItem(
              USER_STORAGE_KEY,
              JSON.stringify(authStatus.user)
            );
          } catch (error) {
            this._logger.logError(`Failed to store user: ${error}`);
          }
        }

        // Store auth status in localStorage
        try {
          localStorage.setItem(AUTH_STATUS_KEY, JSON.stringify(authStatus));
        } catch (error) {
          this._logger.logError(`Failed to store auth status: ${error}`);
        }

        this._logger.logInformation(
          "User authentication status checked successfully"
        );

        return authStatus;
      } else if (response.status === 401) {
        // User is not authenticated
        this._logger.logInformation("User is not authenticated (401)");
        this.clearUser();
        return null;
      } else {
        // Other error status
        this._logger.logError(
          `Authentication check failed with status: ${response.status}`
        );
        return null;
      }
    } catch (error) {
      this._logger.logError(`Error checking authentication: ${error}`);
      // On error, clear user and return null
      this.clearUser();
      return null;
    }
  }

  /**
   * Get current authentication status
   */
  getAuthStatus(): AuthCheckResponse | null {
    return this._authStatus;
  }

  /**
   * Check if user has active subscription
   */
  hasActiveSubscription(): boolean {
    return this._authStatus?.hasActiveSubscription ?? false;
  }

  /**
   * Check if user has active tenant
   */
  hasActiveTenant(): boolean {
    return this._authStatus?.hasActiveTenant ?? false;
  }

  /**
   * Check if user can perform operations
   */
  canPerformOperations(): boolean {
    return this._authStatus?.canPerformOperations ?? false;
  }

  /**
   * Initialize and check authentication status
   * This method should be called on app startup
   */
  async initialize(): Promise<void> {
    // First, load user and auth status from storage
    this.loadUserFromStorage();
    this.loadAuthStatusFromStorage();

    // Always check with server to get latest status
    const authStatus = await this.checkAuthentication();

    if (!authStatus || !authStatus.isAuthenticated) {
      // Clear invalid user data
      this.clearUser();
    }
  }

  /**
   * Logout the current user
   * Calls the logout API endpoint and clears user data
   */
  async logout(): Promise<void> {
    try {
      // Call logout API endpoint
      const response = await this.httpService.postData("/api/auth/logout");

      if (response && response.ok) {
        this._logger.logInformation("User logged out successfully");
      } else {
        this._logger.logError(
          `Logout API call failed with status: ${response?.status || "unknown"}`
        );
      }
    } catch (error) {
      this._logger.logError(`Error during logout API call: ${error}`);
    } finally {
      // Always clear user data from localStorage, even if API call fails
      // This ensures the user is logged out locally regardless of API response
      this.clearUser();
    }
  }

  private loadUserFromStorage(): void {
    try {
      const userJson = localStorage.getItem(USER_STORAGE_KEY);
      if (userJson) {
        this._currentUser = JSON.parse(userJson) as User;
      }
    } catch (error) {
      this._logger.logError(`Failed to load user from storage: ${error}`);
      this._currentUser = null;
    }
  }

  private loadAuthStatusFromStorage(): void {
    try {
      const authStatusJson = localStorage.getItem(AUTH_STATUS_KEY);
      if (authStatusJson) {
        this._authStatus = JSON.parse(authStatusJson) as AuthCheckResponse;
      }
    } catch (error) {
      this._logger.logError(
        `Failed to load auth status from storage: ${error}`
      );
      this._authStatus = null;
    }
  }
}
