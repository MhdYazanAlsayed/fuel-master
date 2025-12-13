import React, { useState } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import type IIdentityService from "../../app/core/interfaces/identity/IIdentityService";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import { Services } from "../../app/core/utils/ServiceCollection";

export function DashboardLayout() {
  const location = useLocation();
  const navigate = useNavigate();
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

  const identityService =
    DependeciesInjector.services.getService<IIdentityService>(
      Services.IdentityService
    );

  const handleLogout = async () => {
    try {
      await identityService.logout();
      navigate("/login", { replace: true });
    } catch (error) {
      console.error("Error during logout:", error);
      // Navigate to login even if logout API call fails
      navigate("/login", { replace: true });
    }
  };

  const isActive = (path: string, exact: boolean = false) => {
    if (exact) {
      return location.pathname === path;
    }
    return (
      location.pathname === path || location.pathname.startsWith(path + "/")
    );
  };

  return (
    <div className="admin-dashboard" dir="ltr">
      {/* Top Header */}
      <header className="admin-header">
        <div className="admin-header-left">
          <button
            className="sidebar-toggle"
            onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
            aria-label="Toggle sidebar"
          >
            <svg
              width="20"
              height="20"
              viewBox="0 0 20 20"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M2.5 5H17.5M2.5 10H17.5M2.5 15H17.5"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
              />
            </svg>
          </button>
          <div className="admin-logo">
            <div className="logo-icon">⛽</div>
            <span className="logo-text">FuelStation</span>
          </div>
        </div>

        <div className="admin-header-right">
          <div className="header-search">
            <button className="header-icon-btn search-btn" title="Search">
              <svg
                width="18"
                height="18"
                viewBox="0 0 18 18"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M8.25 14.25C11.5637 14.25 14.25 11.5637 14.25 8.25C14.25 4.93629 11.5637 2.25 8.25 2.25C4.93629 2.25 2.25 4.93629 2.25 8.25C2.25 11.5637 4.93629 14.25 8.25 14.25Z"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M15.75 15.75L12.4875 12.4875"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
            </button>
          </div>
          <button
            className="header-icon-btn notification-btn"
            title="Notifications"
          >
            <svg
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M9 2.25C6.92893 2.25 5.25 3.92893 5.25 6V9.75C5.25 10.1642 5.08579 10.5628 4.79289 10.8558C4.5 11.1487 4.10137 11.3125 3.6875 11.3125H14.3125C13.8986 11.3125 13.5 11.1487 13.2071 10.8558C12.9142 10.5628 12.75 10.1642 12.75 9.75V6C12.75 3.92893 11.0711 2.25 9 2.25Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <path
                d="M6.75 11.3125V12.1875C6.75 13.125 7.3125 14.0625 9 14.0625C10.6875 14.0625 11.25 13.125 11.25 12.1875V11.3125"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
            <span className="notification-badge">3</span>
          </button>
          <button className="header-icon-btn settings-btn" title="Settings">
            <svg
              width="18"
              height="18"
              viewBox="0 0 18 18"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M9 11.25C10.2426 11.25 11.25 10.2426 11.25 9C11.25 7.75736 10.2426 6.75 9 6.75C7.75736 6.75 6.75 7.75736 6.75 9C6.75 10.2426 7.75736 11.25 9 11.25Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <path
                d="M14.0625 9.75C14.0625 9.9375 14.0625 10.125 14.0625 10.3125C14.0625 11.0625 13.875 11.8125 13.5 12.375L12.75 13.5C12.375 14.0625 11.8125 14.4375 11.25 14.625C10.6875 14.8125 10.125 14.8125 9.5625 14.625C9 14.4375 8.4375 14.0625 8.0625 13.5L7.3125 12.375C6.9375 11.8125 6.75 11.0625 6.75 10.3125C6.75 10.125 6.75 9.9375 6.75 9.75C6.75 9.5625 6.75 9.375 6.75 9.1875C6.75 8.4375 6.9375 7.6875 7.3125 7.125L8.0625 6C8.4375 5.4375 9 5.0625 9.5625 4.875C10.125 4.6875 10.6875 4.6875 11.25 4.875C11.8125 5.0625 12.375 5.4375 12.75 6L13.5 7.125C13.875 7.6875 14.0625 8.4375 14.0625 9.1875C14.0625 9.375 14.0625 9.5625 14.0625 9.75Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
          </button>
          <div className="header-divider"></div>
          <div className="header-user-menu">
            <div className="user-avatar">
              <img
                src="https://ui-avatars.com/api/?name=Admin+User&background=2563eb&color=fff"
                alt="User"
              />
            </div>
            <div className="user-info">
              <div className="user-name">Admin User</div>
              <div className="user-role">Super Admin</div>
            </div>
            <button
              className="logout-btn"
              onClick={handleLogout}
              title="Logout"
            >
              <svg
                width="18"
                height="18"
                viewBox="0 0 18 18"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M6.75 15.75H3.75C3.35218 15.75 2.97064 15.592 2.68934 15.3107C2.40804 15.0294 2.25 14.6478 2.25 14.25V3.75C2.25 3.35218 2.40804 2.97064 2.68934 2.68934C2.97064 2.40804 3.35218 2.25 3.75 2.25H6.75"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M12 12.75L15.75 9L12 5.25"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M15.75 9H6.75"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
            </button>
          </div>
        </div>
      </header>

      <div
        className={`admin-layout ${
          sidebarCollapsed ? "sidebar-collapsed" : ""
        }`}
      >
        {/* Sidebar */}
        <aside
          className={`admin-sidebar ${sidebarCollapsed ? "collapsed" : ""}`}
        >
          <nav className="sidebar-nav">
            {/* Overview */}
            <button
              className={`sidebar-nav-item ${
                isActive("/dashboard", true) ? "active" : ""
              }`}
              onClick={() => navigate("/dashboard")}
              data-tooltip={sidebarCollapsed ? "Overview" : undefined}
            >
              <span className="nav-icon">📊</span>
              <span className="nav-label">Overview</span>
            </button>

            {/* Admin Tools */}
            <div className="sidebar-section">
              {!sidebarCollapsed && (
                <div className="sidebar-section-title">
                  <span className="nav-icon">🛠️</span>
                  <span className="nav-label">Admin Tools</span>
                </div>
              )}
              <button
                className={`sidebar-nav-item sub-item ${
                  isActive("/dashboard/admin/user-control") ? "active" : ""
                }`}
                onClick={() => navigate("/dashboard/admin/user-control")}
                data-tooltip={sidebarCollapsed ? "User Control" : undefined}
              >
                <span className="nav-icon">👤</span>
                <span className="nav-label">User Control</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${
                  isActive("/dashboard/admin/backup-restore") ? "active" : ""
                }`}
                onClick={() => navigate("/dashboard/admin/backup-restore")}
                data-tooltip={sidebarCollapsed ? "Backup & Restore" : undefined}
              >
                <span className="nav-icon">💾</span>
                <span className="nav-label">Backup & Restore</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${
                  isActive("/dashboard/admin/audit-logs") ? "active" : ""
                }`}
                onClick={() => navigate("/dashboard/admin/audit-logs")}
                data-tooltip={sidebarCollapsed ? "Audit Logs" : undefined}
              >
                <span className="nav-icon">📝</span>
                <span className="nav-label">Audit Logs</span>
              </button>
            </div>
          </nav>
        </aside>

        {/* Main Content */}
        <main className="admin-main">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
