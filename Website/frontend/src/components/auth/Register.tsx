import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthLeftSide } from "./AuthLeftSide";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import IAuthService from "../../app/core/interfaces/apis/auth/IAuthService";
import { Services } from "../../app/core/utils/ServiceCollection";
import RegisterDto from "../../app/core/interfaces/apis/auth/dtos/RegisterDto";
import ILoggerService from "../../app/core/interfaces/loggers/ILoggerService";
import { PasswordField } from "../shared/PasswordField";

export function Register() {
  const logger = DependeciesInjector.services.getService<ILoggerService>(
    Services.LoggerService
  );
  const authService = DependeciesInjector.services.getService<IAuthService>(
    Services.AuthService
  );

  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
    agreeToTerms: false,
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    logger.logInformation("Registering user ...");

    // Validation
    if (!formData.firstName.trim()) {
      setError("First name is required");
      logger.logError("First name is required");
      return;
    }
    if (!formData.lastName.trim()) {
      setError("Last name is required");
      logger.logError("Last name is required");
      return;
    }
    if (formData.password !== formData.confirmPassword) {
      setError("Passwords do not match");
      logger.logError("Passwords do not match");
      return;
    }
    if (!formData.agreeToTerms) {
      setError("Please agree to Terms & Conditions");
      logger.logError("Please agree to Terms & Conditions");
      return;
    }

    setIsLoading(true);

    try {
      // Create register DTO matching API requirements
      const registerDto: RegisterDto = {
        email: formData.email.trim(),
        password: formData.password,
        firstName: formData.firstName.trim(),
        lastName: formData.lastName.trim(),
      };

      // Call register API
      const result = await authService.register(registerDto);

      if (result.success) {
        // Success - user is registered and token is set in secure cookie
        // User information is stored by IdentityService
        logger.logInformation("User registered successfully");
        navigate("/dashboard");
      } else {
        // Handle API errors (400 validation errors, 500 server errors, etc.)
        const errorMessage =
          result.message || "Registration failed. Please try again.";
        setError(errorMessage);
        logger.logError(`Registration failed: ${errorMessage}`);
      }
    } catch (err) {
      // Handle unexpected errors
      const errorMessage =
        err instanceof Error
          ? err.message
          : "An unexpected error occurred. Please try again.";
      setError(errorMessage);
      logger.logError(`Registration error: ${errorMessage}`);
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  return (
    <div className="auth-container">
      <div className="auth-split-layout">
        {/* Left side - Logo & Company Name (Fixed) */}
        <AuthLeftSide />

        {/* Right side - Register Form */}
        <div className="auth-form-side">
          <div className="auth-form-container">
            <div className="auth-form-header">
              <div className="auth-form-logo">
                <div className="logo-icon-small">⛽</div>
              </div>
              <h2>Create your account</h2>
              <p>Get started with FuelStation today</p>
            </div>

            <form onSubmit={handleSubmit} className="auth-form">
              <div className="form-group">
                <label htmlFor="firstName">
                  First Name <span className="required">*</span>
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
                      d="M9 9C11.0711 9 12.75 7.32107 12.75 5.25C12.75 3.17893 11.0711 1.5 9 1.5C6.92893 1.5 5.25 3.17893 5.25 5.25C5.25 7.32107 6.92893 9 9 9Z"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                    <path
                      d="M2.25 16.5C2.25 13.6054 5.10538 11.25 9 11.25C12.8946 11.25 15.75 13.6054 15.75 16.5"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                  <input
                    type="text"
                    id="firstName"
                    name="firstName"
                    className="form-input"
                    placeholder="John"
                    value={formData.firstName}
                    onChange={handleChange}
                    required
                  />
                </div>
              </div>

              <div className="form-group">
                <label htmlFor="lastName">
                  Last Name <span className="required">*</span>
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
                      d="M9 9C11.0711 9 12.75 7.32107 12.75 5.25C12.75 3.17893 11.0711 1.5 9 1.5C6.92893 1.5 5.25 3.17893 5.25 5.25C5.25 7.32107 6.92893 9 9 9Z"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                    <path
                      d="M2.25 16.5C2.25 13.6054 5.10538 11.25 9 11.25C12.8946 11.25 15.75 13.6054 15.75 16.5"
                      stroke="currentColor"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                  <input
                    type="text"
                    id="lastName"
                    name="lastName"
                    className="form-input"
                    placeholder="Doe"
                    value={formData.lastName}
                    onChange={handleChange}
                    required
                  />
                </div>
              </div>

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
                    name="email"
                    className="form-input"
                    placeholder="you@company.com"
                    value={formData.email}
                    onChange={handleChange}
                    required
                  />
                </div>
              </div>

              <PasswordField
                id="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                placeholder="Create a strong password"
                label="Password"
                required
              />

              <PasswordField
                id="confirmPassword"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                placeholder="Re-enter your password"
                label="Confirm Password"
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

              <div className="form-group-checkbox">
                <input
                  type="checkbox"
                  id="agreeToTerms"
                  name="agreeToTerms"
                  checked={formData.agreeToTerms}
                  onChange={handleChange}
                />
                <label htmlFor="agreeToTerms">
                  I agree to the{" "}
                  <a href="#" className="text-link">
                    Terms & Conditions
                  </a>{" "}
                  and{" "}
                  <a href="#" className="text-link">
                    Privacy Policy
                  </a>
                </label>
              </div>

              <button
                type="submit"
                className="btn btn-primary btn-block btn-lg"
                disabled={isLoading}
              >
                {isLoading ? "Creating account..." : "Create Account"}
              </button>
            </form>

            <div className="auth-divider">
              <span>Or sign up with</span>
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
              Sign up with Google
            </button>

            <div className="auth-footer">
              <p>
                Already have an account?{" "}
                <button
                  type="button"
                  className="text-link"
                  onClick={() => navigate("/login")}
                >
                  Sign in
                </button>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
