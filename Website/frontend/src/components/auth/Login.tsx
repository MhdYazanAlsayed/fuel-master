import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthLeftSide } from "./AuthLeftSide";
// import DI from "../../app/core/utils/DI";
import { IAuthServiceToken } from "../../app/core/interfaces/apis/auth/IAuthService.token";
import LoginDto from "../../app/core/interfaces/apis/auth/dtos/LoginDto";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import IAuthService from "../../app/core/interfaces/apis/auth/IAuthService";
import { Services } from "../../app/core/utils/ServiceCollection";
import { PasswordField } from "../shared/PasswordField";

export function Login() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      // Get AuthService from DI container using interface token (no coupling to implementation)
      const authService = DependeciesInjector.services.getService<IAuthService>(
        Services.AuthService
      );

      // Create login DTO
      const loginDto: LoginDto = {
        email,
        password,
      };

      // Call login API
      var result = await authService.login(loginDto);
      if (result.success) {
        navigate("/dashboard", { replace: true });
      } else {
        setError(result.message || "Login failed. Please try again.");
      }
    } catch (err) {
      setError("Login failed. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-split-layout">
        {/* Left side - Logo & Company Name (Fixed) */}
        <AuthLeftSide />

        {/* Right side - Login Form */}
        <div className="auth-form-side">
          <div className="auth-form-container">
            <div className="auth-form-header">
              <div className="auth-form-logo">
                <div className="logo-icon-small">⛽</div>
              </div>
              <h2>Welcome back</h2>
              <p>Sign in to your account to continue</p>
            </div>

            <form onSubmit={handleSubmit} className="auth-form">
              <div className="form-group">
                <label htmlFor="email">
                  Email address <span className="required">*</span>
                </label>
                <div className="form-input-wrapper">
                  <svg
                    className="form-input-icon"
                    width="18"
                    height="18"
                    viewBox="0 0 18 18"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M3 4.5H15C15.4142 4.5 15.75 4.83579 15.75 5.25V13.5C15.75 13.9142 15.4142 14.25 15 14.25H3C2.58579 14.25 2.25 13.9142 2.25 13.5V5.25C2.25 4.83579 2.58579 4.5 3 4.5Z"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                    <path
                      d="M2.25 5.25L9 9.75L15.75 5.25"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                  <input
                    type="email"
                    id="email"
                    className="form-input"
                    placeholder="you@company.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                </div>
              </div>

              <PasswordField
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Enter your password"
                label={
                  <div className="form-label-with-link">
                    <span>
                      Password <span className="required">*</span>
                    </span>
                    <button
                      type="button"
                      className="text-link"
                      onClick={() => navigate("/forgot-password")}
                    >
                      Forgot password?
                    </button>
                  </div>
                }
                required
              />

              {error && (
                <div
                  className="form-error"
                  style={{
                    color: "#ef4444",
                    fontSize: "14px",
                    marginBottom: "16px",
                    padding: "12px",
                    backgroundColor: "#fef2f2",
                    borderRadius: "6px",
                    border: "1px solid #fecaca",
                  }}
                >
                  {error}
                </div>
              )}

              <button
                type="submit"
                className="btn btn-primary btn-block btn-lg"
                disabled={isLoading}
              >
                {isLoading ? "Signing in..." : "Sign in"}
              </button>
            </form>

            <div className="auth-divider">
              <span>Or continue with</span>
            </div>

            <button type="button" className="btn btn-google btn-block">
              <svg width="18" height="18" viewBox="0 0 18 18" fill="none">
                <path
                  d="M17.64 9.2c0-.637-.057-1.251-.164-1.84H9v3.481h4.844c-.209 1.125-.843 2.078-1.796 2.717v2.258h2.908c1.702-1.567 2.684-3.874 2.684-6.615z"
                  fill="#4285F4"
                />
                <path
                  d="M9 18c2.43 0 4.467-.806 5.956-2.184l-2.908-2.258c-.806.54-1.837.86-3.048.86-2.344 0-4.328-1.584-5.036-3.711H.957v2.332C2.438 15.983 5.482 18 9 18z"
                  fill="#34A853"
                />
                <path
                  d="M3.964 10.707c-.18-.54-.282-1.117-.282-1.707 0-.593.102-1.17.282-1.709V4.958H.957C.347 6.173 0 7.548 0 9c0 1.452.348 2.827.957 4.042l3.007-2.335z"
                  fill="#FBBC05"
                />
                <path
                  d="M9 3.58c1.321 0 2.508.454 3.44 1.345l2.582-2.58C13.463.891 11.426 0 9 0 5.482 0 2.438 2.017.957 4.958L3.964 7.29C4.672 5.163 6.656 3.58 9 3.58z"
                  fill="#EA4335"
                />
              </svg>
              Continue with Google
            </button>

            <div className="auth-footer">
              <p>
                Don't have an account?{" "}
                <button
                  type="button"
                  className="text-link"
                  onClick={() => navigate("/register")}
                >
                  Sign up
                </button>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
