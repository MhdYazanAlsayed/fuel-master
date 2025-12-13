import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export function CreateRole() {
  const navigate = useNavigate();
  const [roleName, setRoleName] = useState('');
  const [permissions, setPermissions] = useState({
    usersManagement: { read: false, write: false, delete: false, manage: false },
    employees: { read: false, write: false, delete: false, manage: false },
    paymentCards: { read: false, write: false, delete: false, manage: false },
    backups: { read: false, write: false, delete: false, manage: false },
    stations: { read: false, write: false, delete: false, manage: false },
  });

  const handlePermissionChange = (category: string, permission: string) => {
    setPermissions(prev => ({
      ...prev,
      [category]: {
        ...prev[category],
        [permission]: !prev[category][permission]
      }
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert('Role created successfully!');
    navigate('/dashboard/roles');
  };

  const permissionCategories = [
    { key: 'usersManagement', label: 'Users Management', icon: '👥', description: 'Control user accounts and access' },
    { key: 'employees', label: 'Employees', icon: '💼', description: 'Manage employee records and information' },
    { key: 'paymentCards', label: 'Payment Cards', icon: '💳', description: 'Handle payment card operations' },
    { key: 'backups', label: 'Backups', icon: '💾', description: 'System backup and restore operations' },
    { key: 'stations', label: 'Stations', icon: '⛽', description: 'Fuel station management' },
  ];

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <button className="back-btn" onClick={() => navigate('/dashboard/roles')}>
            ← Back
          </button>
          <h1 className="page-title">Create New Role</h1>
          <p className="page-description">Define a new role with specific permissions</p>
        </div>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="content-card">
          <h3 className="card-section-title">Role Information</h3>
          <div className="form-group">
            <label htmlFor="roleName">Role Name *</label>
            <input
              type="text"
              id="roleName"
              className="form-input"
              placeholder="e.g., Senior Manager"
              value={roleName}
              onChange={(e) => setRoleName(e.target.value)}
              required
            />
            <p className="form-hint">Choose a descriptive name for this role</p>
          </div>
        </div>

        <div className="content-card">
          <h3 className="card-section-title">Permissions</h3>
          <p className="card-description">Select the permissions for this role. Toggle switches to grant or revoke access.</p>
          
          <div className="permissions-grid">
            {permissionCategories.map((category) => (
              <div key={category.key} className="permission-category">
                <div className="permission-category-header">
                  <div className="permission-category-icon">{category.icon}</div>
                  <div className="permission-category-info">
                    <h4>{category.label}</h4>
                    <p>{category.description}</p>
                  </div>
                </div>
                <div className="permission-toggles">
                  <label className="permission-toggle">
                    <span>Read</span>
                    <input
                      type="checkbox"
                      checked={permissions[category.key].read}
                      onChange={() => handlePermissionChange(category.key, 'read')}
                    />
                    <span className="checkmark">
                      {permissions[category.key].read ? '✓' : ''}
                    </span>
                  </label>
                  <label className="permission-toggle">
                    <span>Write</span>
                    <input
                      type="checkbox"
                      checked={permissions[category.key].write}
                      onChange={() => handlePermissionChange(category.key, 'write')}
                    />
                    <span className="checkmark">
                      {permissions[category.key].write ? '✓' : ''}
                    </span>
                  </label>
                  <label className="permission-toggle">
                    <span>Delete</span>
                    <input
                      type="checkbox"
                      checked={permissions[category.key].delete}
                      onChange={() => handlePermissionChange(category.key, 'delete')}
                    />
                    <span className="checkmark">
                      {permissions[category.key].delete ? '✓' : ''}
                    </span>
                  </label>
                  <label className="permission-toggle">
                    <span>Manage</span>
                    <input
                      type="checkbox"
                      checked={permissions[category.key].manage}
                      onChange={() => handlePermissionChange(category.key, 'manage')}
                    />
                    <span className="checkmark">
                      {permissions[category.key].manage ? '✓' : ''}
                    </span>
                  </label>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="form-actions sticky">
          <button type="button" className="btn btn-outline-primary" onClick={() => navigate('/dashboard/roles')}>
            Cancel
          </button>
          <button type="submit" className="btn btn-primary">
            Create Role
          </button>
        </div>
      </form>
    </div>
  );
}
