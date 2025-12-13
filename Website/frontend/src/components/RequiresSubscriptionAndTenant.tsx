import React from "react";
import type IIdentityService from "../app/core/interfaces/identity/IIdentityService";
import DependeciesInjector from "../app/core/utils/DependeciesInjector";
import { Services } from "../app/core/utils/ServiceCollection";
import { SubscriptionSelection } from "./subscription/SubscriptionSelection";
import { CreateTenant } from "./tenant/CreateTenant";

interface RequiresSubscriptionAndTenantProps {
  children: React.ReactNode;
}

export function RequiresSubscriptionAndTenant({
  children,
}: RequiresSubscriptionAndTenantProps) {
  const identityService =
    DependeciesInjector.services.getService<IIdentityService>(
      Services.IdentityService
    );

  // Check if user is authenticated
  if (!identityService.isAuthenticated()) {
    return null; // ProtectedRoute will handle redirect
  }

  // Check subscription status
  const hasSubscription = identityService.hasActiveSubscription();
  if (!hasSubscription) {
    return <SubscriptionSelection />;
  }

  // Check tenant status
  const hasTenant = identityService.hasActiveTenant();
  if (!hasTenant) {
    return <CreateTenant />;
  }

  // User has both subscription and tenant, allow access
  return <>{children}</>;
}
