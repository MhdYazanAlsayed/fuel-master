import User from "../../../../entities/users/User";

export default interface AuthCheckResponse {
  expiresAt?: string;
  isAuthenticated: boolean;
  hasActiveSubscription: boolean;
  hasActiveTenant: boolean;
  canPerformOperations: boolean;
  user: User;
  message: string | null;
}
