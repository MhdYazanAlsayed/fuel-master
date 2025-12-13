import React, { useState } from 'react';
import { useAuth } from '../AuthContext';
import { OverviewDashboard } from './OverviewDashboard';
import { EmployeesList } from '../employees/EmployeesList';
import { CreateEmployee } from '../employees/CreateEmployee';
import { HierarchyChart } from '../employees/HierarchyChart';
import { RolesList } from '../roles/RolesList';
import { CreateRole } from '../roles/CreateRole';
import { UserControl } from '../admin/UserControl';
import { BackupRestore } from '../admin/BackupRestore';
import { AuditLogs } from '../admin/AuditLogs';

type Screen = 'overview' | 'employees-list' | 'create-employee' | 'hierarchy' | 'roles-list' | 'create-role' | 'user-control' | 'backup-restore' | 'audit-logs';

export function MainDashboard() {
  const { logout } = useAuth();
  const [currentScreen, setCurrentScreen] = useState<Screen>('overview');
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

  const renderScreen = () => {
    switch (currentScreen) {
      case 'overview':
        return <OverviewDashboard />;
      case 'employees-list':
        return <EmployeesList onCreateNew={() => setCurrentScreen('create-employee')} />;
      case 'create-employee':
        return <CreateEmployee onBack={() => setCurrentScreen('employees-list')} />;
      case 'hierarchy':
        return <HierarchyChart />;
      case 'roles-list':
        return <RolesList onCreateNew={() => setCurrentScreen('create-role')} />;
      case 'create-role':
        return <CreateRole onBack={() => setCurrentScreen('roles-list')} />;
      case 'user-control':
        return <UserControl />;
      case 'backup-restore':
        return <BackupRestore />;
      case 'audit-logs':
        return <AuditLogs />;
      default:
        return <OverviewDashboard />;
    }
  };

  return (
    <div className="admin-dashboard" dir="ltr">
      {/* Top Header */}
      <header className="admin-header">
        <div className="admin-header-left">
          <button 
            className="sidebar-toggle"
            onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
          >
            ☰
          </button>
          <div className="admin-logo">
            <div className="logo-icon">⛽</div>
            <span className="logo-text">FuelStation</span>
          </div>
        </div>
        
        <div className="admin-header-right">
          <button className="header-icon-btn">
            <span className="icon">🔍</span>
          </button>
          <button className="header-icon-btn">
            <span className="icon">🔔</span>
            <span className="notification-dot"></span>
          </button>
          <button className="header-icon-btn">
            <span className="icon">⚙️</span>
          </button>
          <div className="header-user-menu">
            <div className="user-avatar">
              <img src="https://ui-avatars.com/api/?name=Admin+User&background=2563eb&color=fff" alt="User" />
            </div>
            <div className="user-info">
              <div className="user-name">Admin User</div>
              <div className="user-role">Super Admin</div>
            </div>
            <button className="logout-btn" onClick={logout} title="Logout">
              <span className="icon">🚪</span>
            </button>
          </div>
        </div>
      </header>

      <div className="admin-layout">
        {/* Sidebar */}
        <aside className={`admin-sidebar ${sidebarCollapsed ? 'collapsed' : ''}`}>
          <nav className="sidebar-nav">
            {/* Overview */}
            <button
              className={`sidebar-nav-item ${currentScreen === 'overview' ? 'active' : ''}`}
              onClick={() => setCurrentScreen('overview')}
            >
              <span className="nav-icon">📊</span>
              <span className="nav-label">Overview</span>
            </button>

            {/* Employees & Accounts */}
            <div className="sidebar-section">
              <div className="sidebar-section-title">
                <span className="nav-icon">👥</span>
                <span className="nav-label">Employees & Accounts</span>
              </div>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'employees-list' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('employees-list')}
              >
                <span className="nav-icon">📋</span>
                <span className="nav-label">Employees List</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'create-employee' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('create-employee')}
              >
                <span className="nav-icon">➕</span>
                <span className="nav-label">Create Employee</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'hierarchy' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('hierarchy')}
              >
                <span className="nav-icon">🌳</span>
                <span className="nav-label">Hierarchy Chart</span>
              </button>
            </div>

            {/* Roles & Permissions */}
            <div className="sidebar-section">
              <div className="sidebar-section-title">
                <span className="nav-icon">🔐</span>
                <span className="nav-label">Roles & Permissions</span>
              </div>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'roles-list' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('roles-list')}
              >
                <span className="nav-icon">📜</span>
                <span className="nav-label">Roles List</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'create-role' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('create-role')}
              >
                <span className="nav-icon">✨</span>
                <span className="nav-label">Create Role</span>
              </button>
            </div>

            {/* Admin Tools */}
            <div className="sidebar-section">
              <div className="sidebar-section-title">
                <span className="nav-icon">🛠️</span>
                <span className="nav-label">Admin Tools</span>
              </div>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'user-control' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('user-control')}
              >
                <span className="nav-icon">👤</span>
                <span className="nav-label">User Control</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'backup-restore' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('backup-restore')}
              >
                <span className="nav-icon">💾</span>
                <span className="nav-label">Backup & Restore</span>
              </button>
              <button
                className={`sidebar-nav-item sub-item ${currentScreen === 'audit-logs' ? 'active' : ''}`}
                onClick={() => setCurrentScreen('audit-logs')}
              >
                <span className="nav-icon">📝</span>
                <span className="nav-label">Audit Logs</span>
              </button>
            </div>
          </nav>
        </aside>

        {/* Main Content */}
        <main className="admin-main">
          {renderScreen()}
        </main>
      </div>
    </div>
  );
}
