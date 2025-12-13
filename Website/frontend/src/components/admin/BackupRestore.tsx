import React from "react";

export function BackupRestore() {
  const backups = [
    {
      id: 1,
      name: "Full System Backup",
      date: "2024-11-28 10:30 AM",
      size: "2.4 GB",
      status: "Completed",
    },
    {
      id: 2,
      name: "Database Backup",
      date: "2024-11-27 10:30 AM",
      size: "1.8 GB",
      status: "Completed",
    },
    {
      id: 3,
      name: "Full System Backup",
      date: "2024-11-26 10:30 AM",
      size: "2.3 GB",
      status: "Completed",
    },
    {
      id: 4,
      name: "Manual Backup",
      date: "2024-11-25 03:15 PM",
      size: "2.2 GB",
      status: "Completed",
    },
  ];

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Backup & Restore</h1>
          <p className="page-description">
            Manage system backups and restore operations
          </p>
        </div>
        <div className="page-actions">
          <button className="btn btn-primary">
            <span className="btn-icon">💾</span>
            Create Backup
          </button>
        </div>
      </div>

      <div className="backup-grid">
        <div className="content-card">
          <h3 className="card-section-title">Backup Options</h3>
          <div className="backup-options">
            <button className="backup-option-card">
              <span className="backup-option-icon">⚡</span>
              <div className="backup-option-info">
                <h4>Quick Backup</h4>
                <p>Create a quick backup of critical data</p>
              </div>
            </button>
            <button className="backup-option-card">
              <span className="backup-option-icon">🗄️</span>
              <div className="backup-option-info">
                <h4>Full System Backup</h4>
                <p>Complete backup of all system data</p>
              </div>
            </button>
            <button className="backup-option-card">
              <span className="backup-option-icon">🗃️</span>
              <div className="backup-option-info">
                <h4>Database Only</h4>
                <p>Backup database files only</p>
              </div>
            </button>
          </div>

          <div className="backup-schedule">
            <h4>Automated Backups</h4>
            <div className="schedule-info">
              <div className="schedule-item">
                <span className="schedule-label">Frequency:</span>
                <span className="schedule-value">Daily at 10:30 AM</span>
              </div>
              <div className="schedule-item">
                <span className="schedule-label">Retention:</span>
                <span className="schedule-value">30 days</span>
              </div>
              <div className="schedule-item">
                <span className="schedule-label">Next Backup:</span>
                <span className="schedule-value">Tomorrow at 10:30 AM</span>
              </div>
            </div>
            <button className="btn btn-outline-primary btn-sm">
              Configure Schedule
            </button>
          </div>
        </div>

        <div className="content-card">
          <h3 className="card-section-title">Restore Options</h3>
          <div className="restore-info">
            <div className="info-icon warning">⚠️</div>
            <p className="text-center">
              Restoring from a backup will replace all current data with the
              backup data. This action cannot be undone.
            </p>
          </div>
          <div className="restore-options">
            <button className="btn btn-warning btn-block">
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  gap: "0.5rem",
                }}
              >
                <span className="">📤</span>
                Upload Backup File
              </div>
            </button>
            <p className="text-center text-muted small">
              Supported formats: .zip, .tar.gz, .backup
            </p>
          </div>
        </div>
      </div>

      <div className="content-card">
        <div className="card-header">
          <h3>Backup History</h3>
          <div className="card-actions">
            <button className="btn-text">Download All</button>
            <button className="btn-text danger">Clear History</button>
          </div>
        </div>
        <div className="table-wrapper">
          <table className="data-table">
            <thead>
              <tr>
                <th>Backup Name</th>
                <th>Date & Time</th>
                <th>Size</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {backups.map((backup) => (
                <tr key={backup.id}>
                  <td>
                    <div className="backup-name">
                      <span className="backup-icon">💾</span>
                      {backup.name}
                    </div>
                  </td>
                  <td>{backup.date}</td>
                  <td>{backup.size}</td>
                  <td>
                    <span className="status-badge active">{backup.status}</span>
                  </td>
                  <td>
                    <div className="action-buttons">
                      <button className="action-btn" title="Download">
                        📥
                      </button>
                      <button className="action-btn" title="Restore">
                        ↩️
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
