import React, { useState, useEffect } from "react";
import type ISubscriptionService from "../../app/core/interfaces/apis/subscription/ISubscriptionService";
import type SubscriptionPlan from "../../app/core/interfaces/apis/subscription/dtos/SubscriptionPlan";
import type SubscribeDto from "../../app/core/interfaces/apis/subscription/dtos/SubscribeDto";
import type IIdentityService from "../../app/core/interfaces/identity/IIdentityService";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import { Services } from "../../app/core/utils/ServiceCollection";

export function SubscriptionSelection() {
  const [plans, setPlans] = useState<SubscriptionPlan[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSubscribing, setIsSubscribing] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedPlanId, setSelectedPlanId] = useState<string | null>(null);

  useEffect(() => {
    loadPlans();
  }, []);

  const loadPlans = async () => {
    setIsLoading(true);
    setError(null);

    try {
      const subscriptionService =
        DependeciesInjector.services.getService<ISubscriptionService>(
          Services.SubscriptionService
        );

      const fetchedPlans = await subscriptionService.getPlans(true);
      setPlans(fetchedPlans);
    } catch (err) {
      setError("Failed to load subscription plans. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleSubscribe = async (planId: string) => {
    setIsSubscribing(true);
    setError(null);
    setSelectedPlanId(planId);

    try {
      const subscriptionService =
        DependeciesInjector.services.getService<ISubscriptionService>(
          Services.SubscriptionService
        );

      const subscribeDto: SubscribeDto = { planId };
      const result = await subscriptionService.subscribe(subscribeDto);

      if (result.success) {
        // Re-check authentication to get updated status
        const identityService =
          DependeciesInjector.services.getService<IIdentityService>(
            Services.IdentityService
          );
        await identityService.checkAuthentication();

        // The app will automatically redirect based on updated auth status
        window.location.reload();
      } else {
        setError(result.message || "Subscription failed. Please try again.");
      }
    } catch (err) {
      setError("Subscription failed. Please try again.");
    } finally {
      setIsSubscribing(false);
      setSelectedPlanId(null);
    }
  };

  const parseFeatures = (featuresJson: string) => {
    try {
      return JSON.parse(featuresJson);
    } catch {
      return {};
    }
  };

  if (isLoading) {
    return (
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100vh",
          flexDirection: "column",
          gap: "16px",
        }}
      >
        <div
          style={{
            width: "40px",
            height: "40px",
            border: "4px solid #f3f3f3",
            borderTop: "4px solid #3498db",
            borderRadius: "50%",
            animation: "spin 1s linear infinite",
          }}
        />
        <p style={{ color: "#666", fontSize: "14px" }}>Loading plans...</p>
        <style>
          {`
            @keyframes spin {
              0% { transform: rotate(0deg); }
              100% { transform: rotate(360deg); }
            }
          `}
        </style>
      </div>
    );
  }

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: "#f5f5f5",
        padding: "2rem",
      }}
    >
      <div style={{ maxWidth: "1200px", margin: "0 auto" }}>
        <div style={{ textAlign: "center", marginBottom: "3rem" }}>
          <h1 style={{ fontSize: "2rem", fontWeight: "bold", marginBottom: "0.5rem" }}>
            Choose Your Subscription Plan
          </h1>
          <p style={{ color: "#666", fontSize: "1rem" }}>
            Select a plan to continue using FuelMaster
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
              marginBottom: "2rem",
              textAlign: "center",
            }}
          >
            {error}
          </div>
        )}

        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(300px, 1fr))",
            gap: "2rem",
          }}
        >
          {plans.map((plan) => {
            const features = parseFeatures(plan.features);
            const isSelected = selectedPlanId === plan.id;
            const isProcessing = isSubscribing && isSelected;

            return (
              <div
                key={plan.id}
                style={{
                  backgroundColor: "#fff",
                  borderRadius: "12px",
                  padding: "2rem",
                  boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
                  border: plan.isFree ? "2px solid #10b981" : "2px solid #e5e7eb",
                  display: "flex",
                  flexDirection: "column",
                  transition: "transform 0.2s, box-shadow 0.2s",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.transform = "translateY(-4px)";
                  e.currentTarget.style.boxShadow = "0 4px 16px rgba(0,0,0,0.15)";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.transform = "translateY(0)";
                  e.currentTarget.style.boxShadow = "0 2px 8px rgba(0,0,0,0.1)";
                }}
              >
                {plan.isFree && (
                  <div
                    style={{
                      backgroundColor: "#10b981",
                      color: "#fff",
                      padding: "0.5rem 1rem",
                      borderRadius: "6px",
                      fontSize: "0.875rem",
                      fontWeight: "600",
                      marginBottom: "1rem",
                      textAlign: "center",
                    }}
                  >
                    FREE PLAN
                  </div>
                )}

                <h3
                  style={{
                    fontSize: "1.5rem",
                    fontWeight: "bold",
                    marginBottom: "0.5rem",
                  }}
                >
                  {plan.name}
                </h3>

                <p
                  style={{
                    color: "#666",
                    marginBottom: "1.5rem",
                    fontSize: "0.95rem",
                  }}
                >
                  {plan.description}
                </p>

                <div style={{ marginBottom: "1.5rem" }}>
                  <div style={{ display: "flex", alignItems: "baseline", gap: "0.5rem" }}>
                    <span style={{ fontSize: "2.5rem", fontWeight: "bold" }}>
                      ${plan.price.toFixed(2)}
                    </span>
                    <span style={{ color: "#666", fontSize: "1rem" }}>
                      /{plan.billingCycle}
                    </span>
                  </div>
                </div>

                <ul
                  style={{
                    listStyle: "none",
                    padding: 0,
                    margin: "0 0 2rem 0",
                    flex: 1,
                  }}
                >
                  {Object.entries(features).map(([key, value]) => (
                    <li
                      key={key}
                      style={{
                        padding: "0.5rem 0",
                        borderBottom: "1px solid #f3f4f6",
                        display: "flex",
                        justifyContent: "space-between",
                      }}
                    >
                      <span style={{ textTransform: "capitalize" }}>
                        {key.replace(/([A-Z])/g, " $1").trim()}:
                      </span>
                      <span style={{ fontWeight: "600" }}>{String(value)}</span>
                    </li>
                  ))}
                </ul>

                <button
                  onClick={() => handleSubscribe(plan.id)}
                  disabled={isSubscribing}
                  style={{
                    width: "100%",
                    padding: "0.875rem",
                    backgroundColor: plan.isFree ? "#10b981" : "#2563eb",
                    color: "#fff",
                    border: "none",
                    borderRadius: "8px",
                    fontSize: "1rem",
                    fontWeight: "600",
                    cursor: isSubscribing ? "not-allowed" : "pointer",
                    opacity: isSubscribing ? 0.7 : 1,
                    transition: "opacity 0.2s",
                  }}
                >
                  {isProcessing ? "Processing..." : "Subscribe"}
                </button>
              </div>
            );
          })}
        </div>

        {plans.length === 0 && !isLoading && (
          <div
            style={{
              textAlign: "center",
              padding: "3rem",
              backgroundColor: "#fff",
              borderRadius: "12px",
            }}
          >
            <p style={{ color: "#666", fontSize: "1rem" }}>
              No subscription plans available at the moment.
            </p>
          </div>
        )}
      </div>
    </div>
  );
}

