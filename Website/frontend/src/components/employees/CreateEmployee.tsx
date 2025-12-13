import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export function CreateEmployee() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phone: '',
    role: '',
    station: '',
    isActive: true
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert('Employee created successfully!');
    navigate('/dashboard/employees');
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  return (
    <div className="admin-content">
      <div className="page-header">
        <div>
          <button className="back-btn" onClick={() => navigate('/dashboard/employees')}>
            ← Back
          </button>
          <h1 className="page-title">Create New Employee</h1>
          <p className="page-description">Add a new team member to your organization</p>
        </div>
      </div>

      <div className="form-layout">
        <div className="form-main">
          <div className="content-card">
            <h3 className="card-section-title">Basic Information</h3>
            
            <form onSubmit={handleSubmit}>
              <div className="form-grid">
                <div className="form-group">
                  <label htmlFor="name">Full Name *</label>
                  <input
                    type="text"
                    id="name"
                    name="name"
                    className="form-input"
                    placeholder="John Doe"
                    value={formData.name}
                    onChange={handleChange}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="email">Email Address *</label>
                  <input
                    type="email"
                    id="email"
                    name="email"
                    className="form-input"
                    placeholder="john@company.com"
                    value={formData.email}
                    onChange={handleChange}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="phone">Phone Number</label>
                  <input
                    type="tel"
                    id="phone"
                    name="phone"
                    className="form-input"
                    placeholder="+1 (555) 000-0000"
                    value={formData.phone}
                    onChange={handleChange}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="role">Job Role *</label>
                  <select
                    id="role"
                    name="role"
                    className="form-input"
                    value={formData.role}
                    onChange={handleChange}
                    required
                  >
                    <option value="">Select a role</option>
                    <option value="manager">Station Manager</option>
                    <option value="supervisor">Supervisor</option>
                    <option value="cashier">Cashier</option>
                    <option value="attendant">Attendant</option>
                    <option value="maintenance">Maintenance</option>
                  </select>
                </div>

                <div className="form-group full-width">
                  <label htmlFor="station">Assigned Station(s)</label>
                  <select
                    id="station"
                    name="station"
                    className="form-input"
                    value={formData.station}
                    onChange={handleChange}
                  >
                    <option value="">Select station</option>
                    <option value="station-1">Station 1 - Main Street</option>
                    <option value="station-2">Station 2 - Downtown</option>
                    <option value="station-3">Station 3 - Highway Exit</option>
                    <option value="all">All Stations</option>
                  </select>
                </div>
              </div>

              <div className="form-actions">
                <button type="button" className="btn btn-outline-primary" onClick={() => navigate('/dashboard/employees')}>
                  Cancel
                </button>
                <button type="submit" className="btn btn-primary">
                  Create Employee
                </button>
              </div>
            </form>
          </div>
        </div>

        <div className="form-sidebar">
          <div className="content-card">
            <h3 className="card-section-title">Profile Photo</h3>
            <div className="upload-avatar">
              <div className="avatar-preview">
                <img src="https://ui-avatars.com/api/?name=New+User&background=e2e8f0&color=475569" alt="Avatar" />
              </div>
              <button type="button" className="btn btn-outline-primary btn-block">
                Upload Photo
              </button>
              <p className="upload-hint">JPG, PNG or GIF. Max size 2MB</p>
            </div>
          </div>

          <div className="content-card">
            <h3 className="card-section-title">Account Status</h3>
            <div className="toggle-group">
              <div className="toggle-item">
                <div className="toggle-info">
                  <span className="toggle-label">Active</span>
                  <span className="toggle-description">Employee can access the system</span>
                </div>
                <label className="toggle-switch">
                  <input
                    type="checkbox"
                    checked={formData.isActive}
                    onChange={(e) => setFormData(prev => ({ ...prev, isActive: e.target.checked }))}
                  />
                  <span className="toggle-slider"></span>
                </label>
              </div>
            </div>
          </div>

          <div className="content-card">
            <h3 className="card-section-title">Permissions</h3>
            <div className="permissions-preview">
              <p className="text-muted small">
                Permissions will be based on the selected role. You can customize them after creating the employee.
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
