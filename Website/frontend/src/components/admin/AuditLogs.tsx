import React, { useState } from 'react';

export function AuditLogs() {
  const [filterUser, setFilterUser] = useState('all');
  const [filterAction, setFilterAction] = useState('all');
  const [filterDate, setFilterDate] = useState('all');

  const logs = [
    { id: 1, user: 'Admin User', action: 'Created employee', area: 'Employees', details: 'John Smith added as Station Manager', timestamp: '2024-11-28 10:30 AM', severity: 'info' },
    { id: 2, user: 'Sarah Johnson', action: 'Updated permissions', area: 'Roles', details: 'Modified Supervisor role permissions', timestamp: '2024-11-28 09:15 AM', severity: 'warning' },
    { id: 3, user: 'Admin User', action: 'System backup', area: 'Backup', details: 'Full system backup completed', timestamp: '2024-11-28 08:00 AM', severity: 'success' },
    { id: 4, user: 'Mike Davis', action: 'Failed login', area: 'Security', details: 'Multiple failed login attempts', timestamp: '2024-11-28 07:45 AM', severity: 'error' },
    { id: 5, user: 'Admin User', action: 'Deactivated user', area: 'Users', details: 'User account temporarily disabled', timestamp: '2024-11-27 04:20 PM', severity: 'warning' },
    { id: 6, user: 'Emily Brown', action: 'Updated role', area: 'Roles', details: 'Changed Cashier role permissions', timestamp: '2024-11-27 02:10 PM', severity: 'info' },
    { id: 7, user: 'System', action: 'Auto backup', area: 'Backup', details: 'Scheduled backup executed', timestamp: '2024-11-27 10:30 AM', severity: 'success' },
    { id: 8, user: 'David Wilson', action: 'Password reset', area: 'Security', details: 'Password reset requested', timestamp: '2024-11-27 09:00 AM', severity: 'info' },
  ];

  const getSeverityIcon = (severity: string) => {
    switch (severity) {
      case 'success': return '✅';
      case 'error': return '❌';
      case 'warning': return '⚠️';
      default: return 'ℹ️';
    }
  };

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Audit Logs</h1>
          <p className="page-description">Track and monitor all system activities</p>
        </div>
        <div className="page-actions">
          <button className="btn btn-outline-primary">
            <span className="btn-icon">📥</span>
            Export Logs
          </button>
        </div>
      </div>

      <div className="content-card">
        <div className="table-controls">
          <div className="filter-group">
            <select 
              className="filter-select"
              value={filterUser}
              onChange={(e) => setFilterUser(e.target.value)}
            >
              <option value="all">All Users</option>
              <option value="admin">Admin User</option>
              <option value="system">System</option>
            </select>
            
            <select 
              className="filter-select"
              value={filterAction}
              onChange={(e) => setFilterAction(e.target.value)}
            >
              <option value="all">All Actions</option>
              <option value="created">Created</option>
              <option value="updated">Updated</option>
              <option value="deleted">Deleted</option>
              <option value="login">Login</option>
              <option value="backup">Backup</option>
            </select>
            
            <select 
              className="filter-select"
              value={filterDate}
              onChange={(e) => setFilterDate(e.target.value)}
            >
              <option value="all">All Time</option>
              <option value="today">Today</option>
              <option value="week">This Week</option>
              <option value="month">This Month</option>
            </select>

            <input
              type="date"
              className="filter-select"
              placeholder="Custom Date"
            />
          </div>
        </div>

        <div className="table-wrapper">
          <table className="data-table audit-table">
            <thead>
              <tr>
                <th>Timestamp</th>
                <th>User</th>
                <th>Action</th>
                <th>System Area</th>
                <th>Details</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {logs.map((log) => (
                <tr key={log.id} className={`log-row log-${log.severity}`}>
                  <td className="log-timestamp">{log.timestamp}</td>
                  <td>
                    <div className="user-cell">
                      <img 
                        src={`https://ui-avatars.com/api/?name=${log.user}&background=2563eb&color=fff`} 
                        alt={log.user}
                        className="user-avatar-small"
                      />
                      <span>{log.user}</span>
                    </div>
                  </td>
                  <td>
                    <span className="log-action">{log.action}</span>
                  </td>
                  <td>
                    <span className="log-area-badge">{log.area}</span>
                  </td>
                  <td className="log-details">{log.details}</td>
                  <td>
                    <span className="log-severity-icon">
                      {getSeverityIcon(log.severity)}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className="table-pagination">
          <div className="pagination-info">
            Showing <strong>1-8</strong> of <strong>247</strong> logs
          </div>
          <div className="pagination-controls">
            <button className="pagination-btn" disabled>Previous</button>
            <button className="pagination-btn active">1</button>
            <button className="pagination-btn">2</button>
            <button className="pagination-btn">3</button>
            <button className="pagination-btn">Next</button>
          </div>
        </div>
      </div>

      <div className="logs-stats-grid">
        <div className="stat-card-compact">
          <div className="stat-icon success">✅</div>
          <div className="stat-compact-info">
            <div className="stat-compact-value">1,247</div>
            <div className="stat-compact-label">Successful Actions</div>
          </div>
        </div>
        <div className="stat-card-compact">
          <div className="stat-icon error">❌</div>
          <div className="stat-compact-info">
            <div className="stat-compact-value">23</div>
            <div className="stat-compact-label">Failed Actions</div>
          </div>
        </div>
        <div className="stat-card-compact">
          <div className="stat-icon warning">⚠️</div>
          <div className="stat-compact-info">
            <div className="stat-compact-value">156</div>
            <div className="stat-compact-label">Warnings</div>
          </div>
        </div>
        <div className="stat-card-compact">
          <div className="stat-icon info">ℹ️</div>
          <div className="stat-compact-info">
            <div className="stat-compact-value">2,431</div>
            <div className="stat-compact-label">Info Logs</div>
          </div>
        </div>
      </div>
    </div>
  );
}
