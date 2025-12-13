import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import IIdentityService from "../app/core/interfaces/identity/IIdentityService";
import DependeciesInjector from "../app/core/utils/DependeciesInjector";
import { Services } from "../app/core/utils/ServiceCollection";

interface ProtectedRouteProps {
  children: React.ReactNode;
}

export function ProtectedRoute({ children }: ProtectedRouteProps) {
  const identityService =
    DependeciesInjector.services.getService<IIdentityService>(
      Services.IdentityService
    );

  if (!identityService.isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
