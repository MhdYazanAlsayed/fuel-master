import React, { useState, useEffect } from "react";
import DependeciesInjector from "../../app/core/utils/DependeciesInjector";
import type IHttpService from "../../app/core/interfaces/http/IHttpService";
import { Services } from "../../app/core/utils/ServiceCollection";
import ILoggerService from "../../app/core/interfaces/loggers/ILoggerService";

export function OverviewDashboard() {
  const httpService = DependeciesInjector.services.getService<IHttpService>(
    Services.HttpService
  );
  const logger = DependeciesInjector.services.getService<ILoggerService>(
    Services.LoggerService
  );

  const [databaseStatus, setDatabaseStatus] = useState({
    connection: "Healthy",
    status: "green",
    responseTime: "12ms",
    lastChecked: "Just now",
  });

  const [backupInfo, setBackupInfo] = useState({
    lastBackup: "2 hours ago",
    nextScheduledBackup: "Today at 2:00 AM",
    dailyBackupEnabled: true,
    backupStatus: "Completed",
    totalBackups: 45,
    backupSize: "12.5 GB",
  });

  const [storageInfo, setStorageInfo] = useState({
    used: 67.2,
    total: 100,
    unit: "GB",
    percentage: 67,
  });

  useEffect(() => {
    // TODO: Fetch real data from API
    // fetchDatabaseStatus();
    // fetchBackupInfo();
    // fetchStorageInfo();
  }, []);

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <h1 className="page-title">Dashboard Overview</h1>
          <p className="page-description">
            Monitor your database connection, backups, and system status
          </p>
        </div>
        {/* <div className="page-actions">
          <button className="btn btn-outline-primary">
            <span className="btn-icon">📥</span>
            Export Report
          </button>
          <button className="btn btn-primary">
            <span className="btn-icon">💾</span>
            Create Backup
          </button>
        </div> */}
      </div>

      {/* Database & Backup Overview Cards */}
      <div className="stats-grid">
        <div className="stats-card">
          <div className="stats-card-header">
            <span className="stats-label">Database Status</span>
            <span className="stats-icon blue">🗄️</span>
          </div>
          <div className="stats-value">{databaseStatus.connection}</div>
          <div className="stats-change positive">
            <span className="change-icon">✓</span>
            <span>Response time: {databaseStatus.responseTime}</span>
          </div>
        </div>

        <div className="stats-card">
          <div className="stats-card-header">
            <span className="stats-label">Last Backup</span>
            <span className="stats-icon teal">💾</span>
          </div>
          <div className="stats-value small">{backupInfo.lastBackup}</div>
          <div
            className={`stats-change ${
              backupInfo.backupStatus === "Completed" ? "positive" : "neutral"
            }`}
          >
            <span className="change-icon">
              {backupInfo.backupStatus === "Completed" ? "✓" : "→"}
            </span>
            <span>{backupInfo.backupStatus}</span>
          </div>
        </div>

        <div className="stats-card">
          <div className="stats-card-header">
            <span className="stats-label">Daily Backup</span>
            <span className="stats-icon green">🔄</span>
          </div>
          <div className="stats-value small">
            {backupInfo.dailyBackupEnabled ? "Enabled" : "Disabled"}
          </div>
          <div className="stats-change positive">
            <span className="change-icon">📅</span>
            <span>Next: {backupInfo.nextScheduledBackup}</span>
          </div>
        </div>

        <div className="stats-card">
          <div className="stats-card-header">
            <span className="stats-label">Total Backups</span>
            <span className="stats-icon purple">📦</span>
          </div>
          <div className="stats-value">{backupInfo.totalBackups}</div>
          <div className="stats-change neutral">
            <span className="change-icon">📊</span>
            <span>Total size: {backupInfo.backupSize}</span>
          </div>
        </div>

        <div className="stats-card">
          <div className="stats-card-header">
            <span className="stats-label">Storage Usage</span>
            <span className="stats-icon orange">💿</span>
          </div>
          <div className="stats-value">
            {storageInfo.used} {storageInfo.unit}
          </div>
          <div className="stats-change neutral">
            <span className="change-icon">→</span>
            <span>
              {storageInfo.used} / {storageInfo.total} {storageInfo.unit} (
              {storageInfo.percentage}%)
            </span>
          </div>
        </div>
      </div>

      {/* Database Connection & System Status - Top Section */}
      <div className="content-card" style={{ marginBottom: "1.5rem" }}>
        <div className="card-header">
          <h3>Database Connection & System Status</h3>
        </div>
        <div className="card-body">
          <div className="status-list">
            <div className="status-item">
              <div className="status-label">
                <span className={`status-dot ${databaseStatus.status}`}></span>
                Database Connection
              </div>
              <div
                style={{ display: "flex", alignItems: "center", gap: "1rem" }}
              >
                <span className="status-value">
                  {databaseStatus.connection}
                </span>
                <span
                  style={{
                    fontSize: "12px",
                    color: "var(--medium-gray)",
                  }}
                >
                  Response: {databaseStatus.responseTime}
                </span>
                <span
                  style={{
                    fontSize: "12px",
                    color: "var(--medium-gray)",
                  }}
                >
                  Last checked: {databaseStatus.lastChecked}
                </span>
              </div>
            </div>
            <div className="status-item">
              <div className="status-label">
                <span className="status-dot green"></span>
                API Services
              </div>
              <span className="status-value">Running</span>
            </div>
            <div className="status-item">
              <div className="status-label">
                <span className="status-dot green"></span>
                Backup Service
              </div>
              <span className="status-value">Active</span>
            </div>
            <div className="status-item">
              <div className="status-label">
                <span className="status-dot green"></span>
                Email Service
              </div>
              <span className="status-value">Active</span>
            </div>
          </div>
        </div>
      </div>

      {/* Storage Usage Details */}
      <div className="dashboard-grid">
        <div className="dashboard-card">
          <div className="card-header">
            <h3>Storage Usage Details</h3>
          </div>
          <div className="card-body">
            <div className="storage-usage">
              <div className="usage-header">
                <span>Total Storage</span>
                <span>
                  {storageInfo.used} {storageInfo.unit} / {storageInfo.total}{" "}
                  {storageInfo.unit}
                </span>
              </div>
              <div className="usage-bar">
                <div
                  className="usage-fill"
                  style={{ width: `${storageInfo.percentage}%` }}
                ></div>
              </div>
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  marginTop: "0.5rem",
                  fontSize: "12px",
                  color: "var(--medium-gray)",
                }}
              >
                <span>
                  Used: {storageInfo.used} {storageInfo.unit}
                </span>
                <span>
                  Available: {storageInfo.total - storageInfo.used}{" "}
                  {storageInfo.unit}
                </span>
              </div>
            </div>

            <div style={{ marginTop: "1.5rem" }}>
              <h4
                style={{
                  marginBottom: "0.75rem",
                  fontSize: "14px",
                  fontWeight: "600",
                }}
              >
                Storage Breakdown
              </h4>
              <div className="status-list">
                <div className="status-item">
                  <div className="status-label">Database Files</div>
                  <span className="status-value">45.2 GB</span>
                </div>
                <div className="status-item">
                  <div className="status-label">Backup Files</div>
                  <span className="status-value">18.5 GB</span>
                </div>
                <div className="status-item">
                  <div className="status-label">Logs & Temp</div>
                  <span className="status-value">3.5 GB</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="dashboard-card">
          <div className="card-header">
            <h3>Backup Overview</h3>
            <button className="text-link">View all</button>
          </div>
          <div className="card-body">
            <div className="status-list">
              <div className="status-item">
                <div className="status-label">
                  <span className="status-dot green"></span>
                  Daily Backup Schedule
                </div>
                <span className="status-value">
                  {backupInfo.dailyBackupEnabled ? "Enabled" : "Disabled"}
                </span>
              </div>
              <div className="status-item">
                <div className="status-label">
                  <span className="status-dot green"></span>
                  Last Backup
                </div>
                <span className="status-value">{backupInfo.lastBackup}</span>
              </div>
              <div className="status-item">
                <div className="status-label">
                  <span className="status-dot blue"></span>
                  Next Scheduled Backup
                </div>
                <span className="status-value">
                  {backupInfo.nextScheduledBackup}
                </span>
              </div>
              <div className="status-item">
                <div className="status-label">
                  <span className="status-dot purple"></span>
                  Total Backups
                </div>
                <span className="status-value">
                  {backupInfo.totalBackups} backups
                </span>
              </div>
            </div>

            <div
              style={{
                marginTop: "1.5rem",
                padding: "1rem",
                backgroundColor: "var(--bg-light)",
                borderRadius: "6px",
              }}
            >
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--medium-gray)",
                  marginBottom: "0.5rem",
                }}
              >
                Backup Storage Used
              </div>
              <div
                style={{
                  fontSize: "18px",
                  fontWeight: "600",
                  color: "var(--text-color)",
                }}
              >
                {backupInfo.backupSize}
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Recent Activity */}
      <div className="dashboard-card">
        <div className="card-header">
          <h3>Recent Activity</h3>
          <button className="text-link">View all</button>
        </div>
        <div className="card-body">
          <div className="activity-list">
            <div className="activity-item">
              <div className="activity-icon green">✓</div>
              <div className="activity-content">
                <div className="activity-title">Backup completed</div>
                <div className="activity-description">
                  Daily backup finished successfully
                </div>
                <div className="activity-time">{backupInfo.lastBackup}</div>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon blue">🗄️</div>
              <div className="activity-content">
                <div className="activity-title">
                  Database connection verified
                </div>
                <div className="activity-description">
                  All database connections healthy
                </div>
                <div className="activity-time">
                  {databaseStatus.lastChecked}
                </div>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon purple">💾</div>
              <div className="activity-content">
                <div className="activity-title">Backup scheduled</div>
                <div className="activity-description">
                  Next backup scheduled for {backupInfo.nextScheduledBackup}
                </div>
                <div className="activity-time">1 day ago</div>
              </div>
            </div>
            <div className="activity-item">
              <div className="activity-icon orange">📊</div>
              <div className="activity-content">
                <div className="activity-title">Storage usage updated</div>
                <div className="activity-description">
                  Storage usage at {storageInfo.percentage}%
                </div>
                <div className="activity-time">2 days ago</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
