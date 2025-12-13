import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export function EmployeesList() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');
  const [filterStatus, setFilterStatus] = useState('all');

  const employees = [
    { id: 1, name: 'John Smith', email: 'john.smith@fuel.com', role: 'Station Manager', status: 'Active', dateAdded: '2024-01-15', avatar: 'https://ui-avatars.com/api/?name=John+Smith&background=2563eb&color=fff' },
    { id: 2, name: 'Sarah Johnson', email: 'sarah.j@fuel.com', role: 'Supervisor', status: 'Active', dateAdded: '2024-02-20', avatar: 'https://ui-avatars.com/api/?name=Sarah+Johnson&background=10b981&color=fff' },
    { id: 3, name: 'Mike Davis', email: 'mike.d@fuel.com', role: 'Cashier', status: 'Active', dateAdded: '2024-03-10', avatar: 'https://ui-avatars.com/api/?name=Mike+Davis&background=8b5cf6&color=fff' },
    { id: 4, name: 'Emily Brown', email: 'emily.b@fuel.com', role: 'Attendant', status: 'Inactive', dateAdded: '2024-01-05', avatar: 'https://ui-avatars.com/api/?name=Emily+Brown&background=f59e0b&color=fff' },
    { id: 5, name: 'David Wilson', email: 'david.w@fuel.com', role: 'Station Manager', status: 'Active', dateAdded: '2023-12-12', avatar: 'https://ui-avatars.com/api/?name=David+Wilson&background=ef4444&color=fff' },
  ];

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Employees Management</h1>
          <p className="page-description">Manage your team members and their access</p>
        </div>
        <div className="page-actions">
          <button className="btn btn-outline-primary">
            <span className="btn-icon">📥</span>
            Export
          </button>
          <button className="btn btn-primary" onClick={() => navigate('/dashboard/employees/create')}>
            <span className="btn-icon">➕</span>
            Add Employee
          </button>
        </div>
      </div>

      <div className="content-card">
        <div className="table-controls">
          <div className="search-box">
            <span className="search-icon">🔍</span>
            <input
              type="text"
              placeholder="Search employees..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="search-input"
            />
          </div>
          <div className="filter-group">
            <select 
              className="filter-select"
              value={filterStatus}
              onChange={(e) => setFilterStatus(e.target.value)}
            >
              <option value="all">All Status</option>
              <option value="active">Active</option>
              <option value="inactive">Inactive</option>
            </select>
            <select className="filter-select">
              <option value="all">All Roles</option>
              <option value="manager">Station Manager</option>
              <option value="supervisor">Supervisor</option>
              <option value="cashier">Cashier</option>
              <option value="attendant">Attendant</option>
            </select>
          </div>
        </div>

        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Employee</th>
                <th>Email</th>
                <th>Role</th>
                <th>Status</th>
                <th>Date Added</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {employees.map((employee) => (
                <tr key={employee.id}>
                  <td>
                    <div className="employee-cell">
                      <img src={employee.avatar} alt={employee.name} className="employee-avatar" />
                      <span className="employee-name">{employee.name}</span>
                    </div>
                  </td>
                  <td>{employee.email}</td>
                  <td>
                    <span className="role-badge">{employee.role}</span>
                  </td>
                  <td>
                    <span className={`status-badge ${employee.status.toLowerCase()}`}>
                      {employee.status}
                    </span>
                  </td>
                  <td>{new Date(employee.dateAdded).toLocaleDateString()}</td>
                  <td>
                    <div className="action-buttons">
                      <button className="action-btn" title="Edit">
                        ✏️
                      </button>
                      <button className="action-btn" title="Reset Password">
                        🔑
                      </button>
                      <button className="action-btn danger" title="Disable">
                        🚫
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="table-pagination">
          <div className="pagination-info">
            Showing <strong>1-5</strong> of <strong>5</strong> employees
          </div>
          <div className="pagination-controls">
            <button className="pagination-btn" disabled>Previous</button>
            <button className="pagination-btn active">1</button>
            <button className="pagination-btn" disabled>Next</button>
          </div>
        </div>
      </div>
    </div>
  );
}
