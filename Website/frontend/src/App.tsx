import React, { useEffect, useState } from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  useNavigate,
  useLocation,
} from "react-router-dom";
import { AuthProvider, useAuth } from "./components/AuthContext";
import { LandingPage } from "./components/LandingPage";
import { Login } from "./components/auth/Login";
import { Register } from "./components/auth/Register";
import { ForgotPassword } from "./components/auth/ForgotPassword";
import { ResetPassword } from "./components/auth/ResetPassword";
import { DashboardLayout } from "./components/dashboard/DashboardLayout";
import { OverviewDashboard } from "./components/dashboard/OverviewDashboard";
import { EmployeesList } from "./components/employees/EmployeesList";
import { CreateEmployee } from "./components/employees/CreateEmployee";
import { HierarchyChart } from "./components/employees/HierarchyChart";
import { RolesList } from "./components/roles/RolesList";
import { CreateRole } from "./components/roles/CreateRole";
import { UserControl } from "./components/admin/UserControl";
import { BackupRestore } from "./components/admin/BackupRestore";
import { AuditLogs } from "./components/admin/AuditLogs";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { RequiresSubscriptionAndTenant } from "./components/RequiresSubscriptionAndTenant";
import { ScrollToTop } from "./components/ScrollToTop";
import DependeciesInjector from "./app/core/utils/DependeciesInjector";
import IIdentityService from "./app/core/interfaces/identity/IIdentityService";
import { Services } from "./app/core/utils/ServiceCollection";
import RequiredUnAuthorizedLayout from "./components/layouts/RequiredUnAuthorizedLayout";

function AppContent() {
  const [isCheckingAuth, setIsCheckingAuth] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    checkAuthenticationAsync();
  }, []);

  const checkAuthenticationAsync = async () => {
    try {
      const identityService =
        DependeciesInjector.services.getService<IIdentityService>(
          Services.IdentityService
        );

      setIsCheckingAuth(true);
      // Initialize and check authentication via API
      await identityService.initialize();

      // Check if user is authenticated
      const isAuthenticated = identityService.isAuthenticated();

      if (isAuthenticated) {
        // User is authenticated, navigate to dashboard if on landing/login page
        if (location.pathname === "/" || location.pathname === "/login") {
          navigate("/dashboard", { replace: true });
        }
      } else {
        // User is not authenticated, navigate to landing page if on protected route
        const protectedPaths = ["/dashboard"];
        if (protectedPaths.some((path) => location.pathname.startsWith(path))) {
          navigate("/", { replace: true });
        }
      }
    } catch (error) {
      console.error("Error checking authentication:", error);
      // On error, navigate to landing page
      if (location.pathname.startsWith("/dashboard")) {
        navigate("/", { replace: true });
      }
    } finally {
      setIsCheckingAuth(false);
    }
  };

  // Show loading screen while checking authentication
  if (isCheckingAuth) {
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
        <p style={{ color: "#666", fontSize: "14px" }}>
          Checking authentication...
        </p>
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
    <>
      <ScrollToTop />
      <Routes>
        <Route element={<RequiredUnAuthorizedLayout />}>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<LandingPage />} />
          <Route path="/register" element={<Register />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<ResetPassword />} />
        </Route>

        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <RequiresSubscriptionAndTenant>
                <DashboardLayout />
              </RequiresSubscriptionAndTenant>
            </ProtectedRoute>
          }
        >
          <Route index element={<OverviewDashboard />} />
          <Route path="employees" element={<EmployeesList />} />
          <Route path="employees/create" element={<CreateEmployee />} />
          <Route path="employees/hierarchy" element={<HierarchyChart />} />
          <Route path="roles" element={<RolesList />} />
          <Route path="roles/create" element={<CreateRole />} />
          <Route path="admin/user-control" element={<UserControl />} />
          <Route path="admin/backup-restore" element={<BackupRestore />} />
          <Route path="admin/audit-logs" element={<AuditLogs />} />
        </Route>
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <AppContent />
      </BrowserRouter>
    </AuthProvider>
  );
}
