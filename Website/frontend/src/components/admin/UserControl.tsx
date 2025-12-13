import React, { useState } from "react";
import { PasswordField } from "../shared/PasswordField";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import ILoggerService from "../../app/core/interfaces/loggers/ILoggerService";
import { Services } from "../../app/core/utils/ServiceCollection";
import type IHttpService from "../../app/core/interfaces/http/IHttpService";

export function UserControl() {
  const logger = DependeciesInjector.services.getService<ILoggerService>(
    Services.LoggerService
  );

  const httpService = DependeciesInjector.services.getService<IHttpService>(
    Services.HttpService
  );

  // Security Settings State
  const [securitySettings, setSecuritySettings] = useState({
    twoFactorAuth: true,
    passwordExpiry: false,
    sessionTimeout: true,
    securityQuestion: false,
    emailVerification: true,
  });

  // Password Reset State
  const [passwordReset, setPasswordReset] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });
  const [passwordResetError, setPasswordResetError] = useState<string | null>(
    null
  );
  const [passwordResetSuccess, setPasswordResetSuccess] = useState(false);
  const [isResettingPassword, setIsResettingPassword] = useState(false);
  const [showPasswordResetForm, setShowPasswordResetForm] = useState(false);

  const handleSecuritySettingChange = async (
    setting: keyof typeof securitySettings
  ) => {
    const newValue = !securitySettings[setting];
    setSecuritySettings((prev) => ({
      ...prev,
      [setting]: newValue,
    }));

    try {
      // TODO: Call API to update security setting
      // await httpService.putData(`/api/admin/security/${setting}`, { enabled: newValue });
      logger.logInformation(`${setting} updated to ${newValue}`);
    } catch (error) {
      // Revert on error
      setSecuritySettings((prev) => ({
        ...prev,
        [setting]: !newValue,
      }));
      logger.logError(`Failed to update ${setting}: ${error}`);
    }
  };

  const handlePasswordReset = async (e: React.FormEvent) => {
    e.preventDefault();
    setPasswordResetError(null);
    setPasswordResetSuccess(false);

    // Validation
    if (!passwordReset.currentPassword) {
      setPasswordResetError("Current password is required");
      return;
    }

    if (!passwordReset.newPassword) {
      setPasswordResetError("New password is required");
      return;
    }

    if (passwordReset.newPassword.length < 8) {
      setPasswordResetError("New password must be at least 8 characters long");
      return;
    }

    if (passwordReset.newPassword !== passwordReset.confirmPassword) {
      setPasswordResetError("New passwords do not match");
      return;
    }

    if (passwordReset.currentPassword === passwordReset.newPassword) {
      setPasswordResetError(
        "New password must be different from current password"
      );
      return;
    }

    setIsResettingPassword(true);

    try {
      // TODO: Call API to reset password
      // await httpService.postData('/api/admin/reset-password', {
      //   currentPassword: passwordReset.currentPassword,
      //   newPassword: passwordReset.newPassword,
      // });

      logger.logInformation("Password reset successfully");
      setPasswordResetSuccess(true);
      setPasswordReset({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
      });

      // Clear success message after 3 seconds
      setTimeout(() => {
        setPasswordResetSuccess(false);
      }, 3000);
    } catch (error: any) {
      const errorMessage =
        error?.message ||
        "Failed to reset password. Please check your current password.";
      setPasswordResetError(errorMessage);
      logger.logError(`Password reset failed: ${errorMessage}`);
    } finally {
      setIsResettingPassword(false);
    }
  };

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Security Settings</h1>
          <p className="page-description">
            Manage your account security settings and password
          </p>
        </div>
      </div>

      <div className="admin-tools-grid">
        <div className="content-card">
          <h3 className="card-section-title">Security Settings</h3>
          <div className="toggle-group">
            <div className="toggle-item">
              <div className="toggle-info">
                <span className="toggle-label">Two-Factor Authentication</span>
                <span className="toggle-description">
                  Require 2FA for account access
                </span>
              </div>
              <label className="toggle-switch">
                <input
                  type="checkbox"
                  checked={securitySettings.twoFactorAuth}
                  onChange={() => handleSecuritySettingChange("twoFactorAuth")}
                />
                <span className="toggle-slider"></span>
              </label>
            </div>
            <div className="toggle-item">
              <div className="toggle-info">
                <span className="toggle-label">Password Expiry</span>
                <span className="toggle-description">
                  Passwords expire every 90 days
                </span>
              </div>
              <label className="toggle-switch">
                <input
                  type="checkbox"
                  checked={securitySettings.passwordExpiry}
                  onChange={() => handleSecuritySettingChange("passwordExpiry")}
                />
                <span className="toggle-slider"></span>
              </label>
            </div>
            <div className="toggle-item">
              <div className="toggle-info">
                <span className="toggle-label">Session Timeout</span>
                <span className="toggle-description">
                  Auto logout after 30 minutes of inactivity
                </span>
              </div>
              <label className="toggle-switch">
                <input
                  type="checkbox"
                  checked={securitySettings.sessionTimeout}
                  onChange={() => handleSecuritySettingChange("sessionTimeout")}
                />
                <span className="toggle-slider"></span>
              </label>
            </div>
            <div className="toggle-item">
              <div className="toggle-info">
                <span className="toggle-label">Security Question</span>
                <span className="toggle-description">
                  Require security question for password recovery
                </span>
              </div>
              <label className="toggle-switch">
                <input
                  type="checkbox"
                  checked={securitySettings.securityQuestion}
                  onChange={() =>
                    handleSecuritySettingChange("securityQuestion")
                  }
                />
                <span className="toggle-slider"></span>
              </label>
            </div>
            <div className="toggle-item">
              <div className="toggle-info">
                <span className="toggle-label">Email Verification</span>
                <span className="toggle-description">
                  Require email verification for account changes
                </span>
              </div>
              <label className="toggle-switch">
                <input
                  type="checkbox"
                  checked={securitySettings.emailVerification}
                  onChange={() =>
                    handleSecuritySettingChange("emailVerification")
                  }
                />
                <span className="toggle-slider"></span>
              </label>
            </div>
          </div>
        </div>

        <div className="content-card">
          <h3 className="card-section-title">Reset Admin Password</h3>

          {!showPasswordResetForm ? (
            <div
              style={{
                padding: "1.5rem",
                textAlign: "center",
                border: "1px solid var(--border-color)",
                borderRadius: "8px",
                backgroundColor: "var(--white)",
              }}
            >
              <p
                style={{
                  marginBottom: "1.5rem",
                  color: "var(--text-color)",
                  fontSize: "16px",
                  fontWeight: "500",
                }}
              >
                Did you lose your access to admin account?
              </p>
              <button
                type="button"
                className="btn btn-primary"
                onClick={() => setShowPasswordResetForm(true)}
              >
                Reset It
              </button>
            </div>
          ) : (
            <>
              <p
                style={{
                  marginBottom: "1.5rem",
                  color: "var(--medium-gray)",
                  fontSize: "14px",
                }}
              >
                To reset your password, please enter your current password to
                verify your identity.
              </p>

              <form onSubmit={handlePasswordReset}>
                <PasswordField
                  id="currentPassword"
                  name="currentPassword"
                  value={passwordReset.currentPassword}
                  onChange={(e) => {
                    setPasswordReset((prev) => ({
                      ...prev,
                      currentPassword: e.target.value,
                    }));
                    setPasswordResetError(null);
                  }}
                  placeholder="Enter your current password"
                  label="Current Password"
                  required
                />

                <PasswordField
                  id="newPassword"
                  name="newPassword"
                  value={passwordReset.newPassword}
                  onChange={(e) => {
                    setPasswordReset((prev) => ({
                      ...prev,
                      newPassword: e.target.value,
                    }));
                    setPasswordResetError(null);
                  }}
                  placeholder="Enter your new password"
                  label="New Password"
                  required
                />

                <PasswordField
                  id="confirmPassword"
                  name="confirmPassword"
                  value={passwordReset.confirmPassword}
                  onChange={(e) => {
                    setPasswordReset((prev) => ({
                      ...prev,
                      confirmPassword: e.target.value,
                    }));
                    setPasswordResetError(null);
                  }}
                  placeholder="Confirm your new password"
                  label="Confirm New Password"
                  required
                />

                {passwordResetError && (
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
                    {passwordResetError}
                  </div>
                )}

                {passwordResetSuccess && (
                  <div
                    style={{
                      color: "#10b981",
                      fontSize: "14px",
                      marginBottom: "16px",
                      padding: "12px",
                      backgroundColor: "#f0fdf4",
                      borderRadius: "6px",
                      border: "1px solid #bbf7d0",
                    }}
                  >
                    Password reset successfully!
                  </div>
                )}

                <button
                  type="submit"
                  className="btn btn-primary btn-block"
                  disabled={isResettingPassword}
                  style={{ marginTop: "1rem" }}
                >
                  {isResettingPassword
                    ? "Resetting Password..."
                    : "Reset Password"}
                </button>
              </form>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
