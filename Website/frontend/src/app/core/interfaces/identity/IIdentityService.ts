import User from "../../entities/users/User";
import AuthCheckResponse from "../apis/auth/results/AuthCheckResponse";

export default interface IIdentityService {
  getCurrentUser(): User | null;
  // setUser(user: User, expiresAt: string): Promise<void>;
  clearUser(): void;
  checkTokenExpiration(): boolean;
  isAuthenticated(): boolean;
  checkAuthentication(): Promise<AuthCheckResponse | null>;
  getAuthStatus(): AuthCheckResponse | null;
  hasActiveSubscription(): boolean;
  hasActiveTenant(): boolean;
  canPerformOperations(): boolean;
  initialize(): Promise<void>;
  logout(): Promise<void>;
  setAuthStatus(authStatus: AuthCheckResponse): void;
}
