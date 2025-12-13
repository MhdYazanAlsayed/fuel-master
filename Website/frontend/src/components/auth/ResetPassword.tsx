import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export function ResetPassword() {
  const navigate = useNavigate();
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const getPasswordStrength = (pwd: string) => {
    if (pwd.length === 0) return { strength: 0, label: '' };
    if (pwd.length < 6) return { strength: 1, label: 'Weak' };
    if (pwd.length < 10) return { strength: 2, label: 'Fair' };
    if (pwd.length >= 10 && /[A-Z]/.test(pwd) && /[0-9]/.test(pwd)) 
      return { strength: 3, label: 'Strong' };
    return { strength: 2, label: 'Fair' };
  };

  const passwordStrength = getPasswordStrength(password);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }
    alert('Password reset successful!');
    navigate('/login');
  };

  return (
    <div className="auth-container">
      <div className="auth-center-layout">
        <div className="auth-card">
          <div className="auth-card-header">
            <h2>Set new password</h2>
            <p>Your new password must be different from previously used passwords</p>
          </div>

          <form onSubmit={handleSubmit} className="auth-form">
            <div className="form-group">
              <label htmlFor="password">New Password</label>
              <input
                type="password"
                id="password"
                className="form-input"
                placeholder="Enter new password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              {password && (
                <div className="password-strength">
                  <div className="password-strength-bar">
                    <div 
                      className={`password-strength-fill strength-${passwordStrength.strength}`}
                      style={{ width: `${(passwordStrength.strength / 3) * 100}%` }}
                    />
                  </div>
                  <span className="password-strength-label">{passwordStrength.label}</span>
                </div>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="confirmPassword">Confirm Password</label>
              <input
                type="password"
                id="confirmPassword"
                className="form-input"
                placeholder="Re-enter new password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
              />
            </div>

            <button type="submit" className="btn btn-primary btn-block btn-lg">
              Reset Password
            </button>
          </form>

          <div className="auth-footer text-center">
            <button 
              type="button" 
              className="text-link"
              onClick={() => navigate('/login')}
            >
              ← Back to Login
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
