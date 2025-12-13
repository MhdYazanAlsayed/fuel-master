import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export function ForgotPassword() {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [sent, setSent] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSent(true);
  };

  if (sent) {
    return (
      <div className="auth-container">
        <div className="auth-center-layout">
          <div className="auth-card">
            <div className="success-icon-large">
              <svg width="64" height="64" viewBox="0 0 64 64" fill="none">
                <circle cx="32" cy="32" r="32" fill="#10b981" opacity="0.1"/>
                <path d="M20 32l8 8 16-16" stroke="#10b981" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"/>
              </svg>
            </div>
            <h2>Check your email</h2>
            <p className="text-center text-muted">
              We've sent a password reset link to <strong>{email}</strong>
            </p>
            <p className="text-center text-muted small">
              Didn't receive the email? Check your spam folder or{' '}
              <button className="text-link" onClick={() => setSent(false)}>
                try another email
              </button>
            </p>
            <button 
              className="btn btn-primary btn-block"
              onClick={() => navigate('/login')}
            >
              Back to Login
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="auth-container">
      <div className="auth-center-layout">
        <div className="auth-card">
          <div className="auth-card-header">
            <h2>Forgot password?</h2>
            <p>No worries, we'll send you reset instructions</p>
          </div>

          <form onSubmit={handleSubmit} className="auth-form">
            <div className="form-group">
              <label htmlFor="email">Email address</label>
              <input
                type="email"
                id="email"
                className="form-input"
                placeholder="you@company.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <button type="submit" className="btn btn-primary btn-block btn-lg">
              Send Reset Link
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
