import React, { useState } from 'react';

interface DashboardProps {
  onNavigateToLanding: () => void;
}

export function Dashboard({ onNavigateToLanding }: DashboardProps) {
  const [activeSection, setActiveSection] = useState('overview');
  const [showPassword, setShowPassword] = useState(false);
  const [showApiKey, setShowApiKey] = useState(false);
  const [showChangePlanModal, setShowChangePlanModal] = useState(false);

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text);
    alert('Copied to clipboard!');
  };

  return (
    <div className="dashboard">
      {/* Navbar */}
      <nav className="dashboard-navbar">
        <div className="navbar-content">
          <div className="navbar-brand" onClick={onNavigateToLanding} style={{ cursor: 'pointer' }}>
            <i className="icon-fuel-pump">⛽</i>
            <span className="brand-name">FuelFlow</span>
          </div>
          <div className="navbar-actions">
            <button className="nav-icon-btn">
              🔔
              <span className="notification-badge">3</span>
            </button>
            <div className="user-menu">
              <div className="user-avatar">AC</div>
              <span>Acme Corp</span>
            </div>
          </div>
        </div>
      </nav>

      <div className="dashboard-layout">
        {/* Sidebar */}
        <aside className="dashboard-sidebar">
          <nav className="sidebar-nav">
            <button 
              className={`sidebar-item ${activeSection === 'overview' ? 'active' : ''}`}
              onClick={() => setActiveSection('overview')}
            >
              <span className="sidebar-icon">📊</span>
              <span>Overview</span>
            </button>
            <button 
              className={`sidebar-item ${activeSection === 'root-account' ? 'active' : ''}`}
              onClick={() => setActiveSection('root-account')}
            >
              <span className="sidebar-icon">🛡️</span>
              <span>Root Account</span>
            </button>
            <button 
              className={`sidebar-item ${activeSection === 'backup' ? 'active' : ''}`}
              onClick={() => setActiveSection('backup')}
            >
              <span className="sidebar-icon">💾</span>
              <span>Backup & Restore</span>
            </button>
            <button 
              className={`sidebar-item ${activeSection === 'billing' ? 'active' : ''}`}
              onClick={() => setActiveSection('billing')}
            >
              <span className="sidebar-icon">💳</span>
              <span>Billing</span>
            </button>
            <button 
              className={`sidebar-item ${activeSection === 'settings' ? 'active' : ''}`}
              onClick={() => setActiveSection('settings')}
            >
              <span className="sidebar-icon">⚙️</span>
              <span>Settings</span>
            </button>
          </nav>
        </aside>

        {/* Main Content */}
        <main className="dashboard-main">
          <div className="dashboard-header">
            <h1>Workspace Dashboard</h1>
            <p className="text-muted">Manage your FuelFlow workspace and subscription</p>
          </div>

          {/* Overview Section */}
          {activeSection === 'overview' && (
            <div className="dashboard-section">
              <h2>Workspace Overview</h2>
              <div className="stat-cards-grid">
                <div className="stat-card">
                  <div className="stat-card-icon blue">💾</div>
                  <div className="stat-card-content">
                    <div className="stat-card-label">Data Usage</div>
                    <div className="stat-card-value">2.4 GB</div>
                    <div className="stat-card-footer">
                      <span className="text-success">↑ 12%</span>
                      <span className="text-muted"> vs last month</span>
                    </div>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="stat-card-icon green">⛽</div>
                  <div className="stat-card-content">
                    <div className="stat-card-label">Active Stations</div>
                    <div className="stat-card-value">12</div>
                    <div className="stat-card-footer">
                      <span className="text-muted">of 15 allowed</span>
                    </div>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="stat-card-icon purple">🕐</div>
                  <div className="stat-card-content">
                    <div className="stat-card-label">Last Backup</div>
                    <div className="stat-card-value">2h ago</div>
                    <div className="stat-card-footer">
                      <span className="text-success">✓ Successful</span>
                    </div>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="stat-card-icon orange">📡</div>
                  <div className="stat-card-content">
                    <div className="stat-card-label">PTS Connection</div>
                    <div className="stat-card-value">
                      <span className="status-badge active">Active</span>
                    </div>
                    <div className="stat-card-footer">
                      <span className="text-muted">All systems online</span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="card">
                <div className="card-header">
                  <h3>Quick Actions</h3>
                </div>
                <div className="card-body">
                  <div className="quick-actions">
                    <button className="btn btn-primary">➕ Add Station</button>
                    <button className="btn btn-outline-primary">📥 Export Data</button>
                    <button className="btn btn-outline-primary">📄 Generate Report</button>
                    <button className="btn btn-outline-primary">👤 Invite User</button>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Root Account Section */}
          {activeSection === 'root-account' && (
            <div className="dashboard-section">
              <h2>Root Account Management</h2>
              <div className="card">
                <div className="card-body">
                  <div className="alert alert-info">
                    <span className="alert-icon">ℹ️</span>
                    <div>
                      <strong>Root Account Access</strong>
                      <p>The root account has full administrative privileges. Store credentials securely.</p>
                    </div>
                  </div>

                  <div className="two-column-layout">
                    <div>
                      <h4>Create Root Admin User</h4>
                      <form onSubmit={(e) => { e.preventDefault(); alert('Root user created!'); }}>
                        <div className="form-group">
                          <label>Username</label>
                          <input type="text" value="admin" readOnly className="form-input" />
                        </div>
                        <div className="form-group">
                          <label>Email</label>
                          <input type="email" placeholder="admin@acmecorp.com" className="form-input" />
                        </div>
                        <div className="form-group">
                          <label>Password</label>
                          <div className="input-with-icon">
                            <input 
                              type={showPassword ? 'text' : 'password'} 
                              placeholder="Enter secure password" 
                              className="form-input" 
                            />
                            <button 
                              type="button" 
                              className="input-icon-btn"
                              onClick={() => setShowPassword(!showPassword)}
                            >
                              {showPassword ? '🙈' : '👁️'}
                            </button>
                          </div>
                        </div>
                        <button type="submit" className="btn btn-primary">🛡️ Create Root User</button>
                      </form>
                    </div>

                    <div>
                      <h4>Workspace Credentials</h4>
                      <div className="credential-box">
                        <div className="credential-item">
                          <span className="credential-label">Workspace ID:</span>
                          <div className="credential-value">
                            <code>ws_ac1m3c0rp2024xyz</code>
                            <button 
                              className="icon-btn" 
                              onClick={() => copyToClipboard('ws_ac1m3c0rp2024xyz')}
                            >
                              📋
                            </button>
                          </div>
                        </div>
                        <div className="credential-item">
                          <span className="credential-label">API Key:</span>
                          <div className="credential-value">
                            <code>{showApiKey ? 'sk_live_1234567890abcdefghijklmnop' : 'sk_live_••••••••••••••••'}</code>
                            <button 
                              className="icon-btn" 
                              onClick={() => setShowApiKey(!showApiKey)}
                            >
                              {showApiKey ? '🙈' : '👁️'}
                            </button>
                          </div>
                        </div>
                        <div className="credential-item">
                          <span className="credential-label">Webhook URL:</span>
                          <div className="credential-value">
                            <code>https://api.fuelflow.io/webhook/acmecorp</code>
                            <button 
                              className="icon-btn" 
                              onClick={() => copyToClipboard('https://api.fuelflow.io/webhook/acmecorp')}
                            >
                              📋
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Backup Section */}
          {activeSection === 'backup' && (
            <div className="dashboard-section">
              <h2>Backup & Restore</h2>
              <div className="card">
                <div className="card-body">
                  <div className="two-column-layout">
                    <div>
                      <h4>Backup Options</h4>
                      <p className="text-muted">Create and download workspace backups</p>
                      <div className="button-group">
                        <button className="btn btn-primary" onClick={() => alert('Backup created!')}>📥 Download Backup</button>
                        <button className="btn btn-outline-primary">📅 Schedule Auto Backup</button>
                      </div>
                    </div>
                    <div>
                      <h4>Restore from Backup</h4>
                      <p className="text-muted">Upload and restore a previous backup</p>
                      <input type="file" className="form-input" accept=".zip,.tar.gz" />
                      <button className="btn btn-warning" style={{ marginTop: '1rem' }}>📤 Upload & Restore</button>
                    </div>
                  </div>

                  <hr className="divider" />

                  <h4>Backup History</h4>
                  <div className="table-container">
                    <table className="data-table">
                      <thead>
                        <tr>
                          <th>Date</th>
                          <th>Type</th>
                          <th>Size</th>
                          <th>Status</th>
                          <th>Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td>Nov 26, 2025 10:30 AM</td>
                          <td><span className="badge badge-primary">Automatic</span></td>
                          <td>1.2 GB</td>
                          <td><span className="status-badge active">Success</span></td>
                          <td>
                            <button className="btn-icon">📥</button>
                            <button className="btn-icon danger">🗑️</button>
                          </td>
                        </tr>
                        <tr>
                          <td>Nov 25, 2025 10:30 AM</td>
                          <td><span className="badge badge-primary">Automatic</span></td>
                          <td>1.1 GB</td>
                          <td><span className="status-badge active">Success</span></td>
                          <td>
                            <button className="btn-icon">📥</button>
                            <button className="btn-icon danger">🗑️</button>
                          </td>
                        </tr>
                        <tr>
                          <td>Nov 24, 2025 03:15 PM</td>
                          <td><span className="badge badge-secondary">Manual</span></td>
                          <td>1.1 GB</td>
                          <td><span className="status-badge active">Success</span></td>
                          <td>
                            <button className="btn-icon">📥</button>
                            <button className="btn-icon danger">🗑️</button>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Billing Section */}
          {activeSection === 'billing' && (
            <div className="dashboard-section">
              <h2>Billing & Subscription</h2>
              <div className="billing-layout">
                <div className="billing-main">
                  <div className="card">
                    <div className="card-body">
                      <div className="plan-header">
                        <div>
                          <h4>Current Plan: Pro</h4>
                          <p className="text-muted">Billed monthly • Next billing date: Dec 26, 2025</p>
                        </div>
                        <span className="status-badge active">Active</span>
                      </div>

                      <div className="plan-features-grid">
                        <div className="plan-feature">✓ 15 Stations</div>
                        <div className="plan-feature">✓ 25 Employee Accounts</div>
                        <div className="plan-feature">✓ Advanced Analytics</div>
                        <div className="plan-feature">✓ Zone Pricing</div>
                        <div className="plan-feature">✓ Automated Reporting</div>
                        <div className="plan-feature">✓ API Access</div>
                        <div className="plan-feature">✓ Priority Support</div>
                      </div>

                      <div className="button-group">
                        <button className="btn btn-primary" onClick={() => setShowChangePlanModal(true)}>🔄 Change Plan</button>
                        <button className="btn btn-outline-danger">❌ Cancel Subscription</button>
                      </div>
                    </div>
                  </div>

                  <div className="card">
                    <div className="card-header">
                      <h4>Invoice History</h4>
                    </div>
                    <div className="card-body">
                      <div className="table-container">
                        <table className="data-table">
                          <thead>
                            <tr>
                              <th>Date</th>
                              <th>Description</th>
                              <th>Amount</th>
                              <th>Status</th>
                              <th>Invoice</th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr>
                              <td>Nov 26, 2025</td>
                              <td>Pro Plan - Monthly</td>
                              <td>$149.00</td>
                              <td><span className="badge badge-success">Paid</span></td>
                              <td><button className="btn-sm btn-outline-primary">📥 Download</button></td>
                            </tr>
                            <tr>
                              <td>Oct 26, 2025</td>
                              <td>Pro Plan - Monthly</td>
                              <td>$149.00</td>
                              <td><span className="badge badge-success">Paid</span></td>
                              <td><button className="btn-sm btn-outline-primary">📥 Download</button></td>
                            </tr>
                            <tr>
                              <td>Sep 26, 2025</td>
                              <td>Pro Plan - Monthly</td>
                              <td>$149.00</td>
                              <td><span className="badge badge-success">Paid</span></td>
                              <td><button className="btn-sm btn-outline-primary">📥 Download</button></td>
                            </tr>
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="billing-sidebar">
                  <div className="card">
                    <div className="card-header">
                      <h4>Payment Method</h4>
                    </div>
                    <div className="card-body">
                      <div className="payment-method">
                        <div className="payment-icon">💳</div>
                        <div>
                          <div>Visa •••• 4242</div>
                          <div className="text-muted small">Expires 12/2026</div>
                        </div>
                      </div>
                      <button className="btn btn-outline-primary btn-block">✏️ Update Payment</button>
                    </div>
                  </div>

                  <div className="card">
                    <div className="card-header">
                      <h4>Usage This Month</h4>
                    </div>
                    <div className="card-body">
                      <div className="usage-item">
                        <div className="usage-header">
                          <span>Stations</span>
                          <span>12 / 15</span>
                        </div>
                        <div className="progress-bar">
                          <div className="progress-fill" style={{ width: '80%' }}></div>
                        </div>
                      </div>
                      <div className="usage-item">
                        <div className="usage-header">
                          <span>Employees</span>
                          <span>18 / 25</span>
                        </div>
                        <div className="progress-bar">
                          <div className="progress-fill" style={{ width: '72%' }}></div>
                        </div>
                      </div>
                      <div className="usage-item">
                        <div className="usage-header">
                          <span>API Calls</span>
                          <span>142K / 200K</span>
                        </div>
                        <div className="progress-bar">
                          <div className="progress-fill" style={{ width: '71%' }}></div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Settings Section */}
          {activeSection === 'settings' && (
            <div className="dashboard-section">
              <h2>Settings</h2>
              <div className="card">
                <div className="card-body">
                  <p className="text-muted">Settings page coming soon...</p>
                </div>
              </div>
            </div>
          )}
        </main>
      </div>

      {/* Change Plan Modal */}
      {showChangePlanModal && (
        <div className="modal-overlay" onClick={() => setShowChangePlanModal(false)}>
          <div className="modal-content large" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Change Plan</h3>
              <button className="modal-close" onClick={() => setShowChangePlanModal(false)}>×</button>
            </div>
            <div className="modal-body">
              <div className="modal-pricing-grid">
                <div className="modal-plan-card">
                  <h4>Basic</h4>
                  <div className="modal-plan-price">$49<span>/mo</span></div>
                  <ul className="modal-plan-features">
                    <li>Up to 3 stations</li>
                    <li>Real-time monitoring</li>
                    <li>Basic reporting</li>
                    <li>Email support</li>
                  </ul>
                  <button className="btn btn-outline-primary btn-block">Select Plan</button>
                </div>
                <div className="modal-plan-card active">
                  <div className="modal-plan-badge">Current Plan</div>
                  <h4>Pro</h4>
                  <div className="modal-plan-price">$149<span>/mo</span></div>
                  <ul className="modal-plan-features">
                    <li>Up to 15 stations</li>
                    <li>Advanced analytics</li>
                    <li>Automated reporting</li>
                    <li>Priority support</li>
                  </ul>
                  <button className="btn btn-primary btn-block" disabled>Current Plan</button>
                </div>
                <div className="modal-plan-card">
                  <h4>Enterprise</h4>
                  <div className="modal-plan-price">Custom</div>
                  <ul className="modal-plan-features">
                    <li>Unlimited stations</li>
                    <li>Custom integrations</li>
                    <li>White-label options</li>
                    <li>24/7 phone support</li>
                  </ul>
                  <button className="btn btn-outline-primary btn-block">Contact Sales</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
