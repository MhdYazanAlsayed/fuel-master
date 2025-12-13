import React from 'react';
import { useNavigate } from 'react-router-dom';

export function RolesList() {
  const navigate = useNavigate();
  const roles = [
    { id: 1, name: 'Super Admin', users: 2, permissions: 'Full Access', color: '#1e40af' },
    { id: 2, name: 'Station Manager', users: 5, permissions: 'Manage Employees, View Reports', color: '#2563eb' },
    { id: 3, name: 'Supervisor', users: 12, permissions: 'View Employees, Manage Shifts', color: '#10b981' },
    { id: 4, name: 'Cashier', users: 45, permissions: 'Process Transactions', color: '#8b5cf6' },
    { id: 5, name: 'Attendant', users: 78, permissions: 'Basic Operations', color: '#f59e0b' },
  ];

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Roles & Permissions</h1>
          <p className="page-description">Manage user roles and their access levels</p>
        </div>
        <div className="page-actions">
          <button className="btn btn-primary" onClick={() => navigate('/dashboard/roles/create')}>
            <span className="btn-icon">✨</span>
            Create Role
          </button>
        </div>
      </div>

      <div className="roles-grid">
        {roles.map((role) => (
          <div key={role.id} className="role-card">
            <div className="role-card-header">
              <div className="role-icon" style={{ backgroundColor: role.color }}>
                🔐
              </div>
              <div className="role-menu">
                <button className="role-menu-btn">⋮</button>
              </div>
            </div>
            <div className="role-card-body">
              <h3 className="role-name">{role.name}</h3>
              <div className="role-stats">
                <div className="role-stat">
                  <span className="role-stat-value">{role.users}</span>
                  <span className="role-stat-label">Users</span>
                </div>
              </div>
              <div className="role-permissions">
                <span className="permissions-label">Permissions:</span>
                <p className="permissions-text">{role.permissions}</p>
              </div>
            </div>
            <div className="role-card-footer">
              <button className="btn-text">Edit Permissions</button>
              <button className="btn-text danger">Delete</button>
            </div>
          </div>
        ))}
      </div>

      <div className="content-card">
        <div className="card-header">
          <h3>Detailed Roles List</h3>
        </div>
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Role Name</th>
                <th>Users Assigned</th>
                <th>Key Permissions</th>
                <th>Created Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {roles.map((role) => (
                <tr key={role.id}>
                  <td>
                    <div className="role-cell">
                      <div className="role-icon small" style={{ backgroundColor: role.color }}>
                        🔐
                      </div>
                      <span>{role.name}</span>
                    </div>
                  </td>
                  <td>{role.users} users</td>
                  <td>{role.permissions}</td>
                  <td>Jan 15, 2024</td>
                  <td>
                    <div className="action-buttons">
                      <button className="action-btn" title="Edit">
                        ✏️
                      </button>
                      <button className="action-btn" title="View Details">
                        👁️
                      </button>
                      <button className="action-btn danger" title="Delete">
                        🗑️
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
