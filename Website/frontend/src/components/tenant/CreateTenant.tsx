import React, { useState } from "react";
import type ITenantService from "../../app/core/interfaces/apis/tenant/ITenantService";
import type CreateTenantDto from "../../app/core/interfaces/apis/tenant/dtos/CreateTenantDto";
import type IIdentityService from "../../app/core/interfaces/identity/IIdentityService";
import type ILoggerService from "../../app/core/interfaces/loggers/ILoggerService";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import { Services } from "../../app/core/utils/ServiceCollection";

export function CreateTenant() {
  const [formData, setFormData] = useState({
    name: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    // Validation
    if (!formData.name.trim()) {
      setError("Tenant name is required");
      return;
    }

    // Validate tenant name format: lowercase alphanumeric with hyphens only
    const trimmedName = formData.name.trim();
    const nameRegex = /^[a-z0-9-]+$/;

    if (trimmedName.length < 3 || trimmedName.length > 100) {
      setError("Tenant name must be between 3 and 100 characters long");
      return;
    }

    if (!nameRegex.test(trimmedName)) {
      setError(
        "Tenant name must be lowercase alphanumeric with hyphens only (e.g., 'my-tenant')"
      );
      return;
    }

    setIsLoading(true);

    try {
      const tenantService =
        DependeciesInjector.services.getService<ITenantService>(
          Services.TenantService
        );
      const logger = DependeciesInjector.services.getService<ILoggerService>(
        Services.LoggerService
      );

      const createTenantDto: CreateTenantDto = {
        name: formData.name.trim().toLowerCase(),
      };

      const result = await tenantService.createTenant(createTenantDto);

      if (result.success) {
        logger.logInformation("Tenant created successfully");

        // Re-check authentication to get updated status
        const identityService =
          DependeciesInjector.services.getService<IIdentityService>(
            Services.IdentityService
          );
        await identityService.checkAuthentication();

        // The app will automatically redirect based on updated auth status
        window.location.reload();
      } else {
        setError(result.message || "Tenant creation failed. Please try again.");
        logger.logError(`Tenant creation failed: ${result.message}`);
      }
    } catch (err) {
      const errorMessage =
        err instanceof Error
          ? err.message
          : "An unexpected error occurred. Please try again.";
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: "#f5f5f5",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: "2rem",
      }}
    >
      <div
        style={{
          backgroundColor: "#fff",
          borderRadius: "12px",
          padding: "2.5rem",
          boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
          maxWidth: "500px",
          width: "100%",
        }}
      >
        <div style={{ textAlign: "center", marginBottom: "2rem" }}>
          <h1
            style={{
              fontSize: "1.75rem",
              fontWeight: "bold",
              marginBottom: "0.5rem",
            }}
          >
            Create Your Tenant
          </h1>
          <p style={{ color: "#666", fontSize: "0.95rem" }}>
            Set up your tenant to get started with FuelMaster
          </p>
        </div>

        {error && (
          <div
            style={{
              backgroundColor: "#fef2f2",
              border: "1px solid #fecaca",
              color: "#ef4444",
              padding: "1rem",
              borderRadius: "8px",
              marginBottom: "1.5rem",
              fontSize: "0.9rem",
            }}
          >
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: "1.5rem" }}>
            <label
              htmlFor="name"
              style={{
                display: "block",
                marginBottom: "0.5rem",
                fontWeight: "600",
                fontSize: "0.9rem",
                color: "#333",
              }}
            >
              Tenant Name <span style={{ color: "#ef4444" }}>*</span>
            </label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              placeholder="my-tenant"
              required
              style={{
                width: "100%",
                padding: "0.875rem",
                border: "1.5px solid #e5e7eb",
                borderRadius: "8px",
                fontSize: "0.95rem",
                transition: "border-color 0.2s",
              }}
              onFocus={(e) => {
                e.currentTarget.style.borderColor = "#2563eb";
              }}
              onBlur={(e) => {
                e.currentTarget.style.borderColor = "#e5e7eb";
              }}
            />
            <p
              style={{
                fontSize: "0.8rem",
                color: "#666",
                marginTop: "0.25rem",
              }}
            >
              Lowercase alphanumeric with hyphens only
            </p>
          </div>

          <button
            type="submit"
            disabled={isLoading}
            style={{
              width: "100%",
              padding: "0.875rem",
              backgroundColor: "#2563eb",
              color: "#fff",
              border: "none",
              borderRadius: "8px",
              fontSize: "1rem",
              fontWeight: "600",
              cursor: isLoading ? "not-allowed" : "pointer",
              opacity: isLoading ? 0.7 : 1,
              transition: "opacity 0.2s",
            }}
          >
            {isLoading ? "Creating Tenant..." : "Create Tenant"}
          </button>
        </form>
      </div>
    </div>
  );
}
