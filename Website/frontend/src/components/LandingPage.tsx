import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export function LandingPage() {
  const navigate = useNavigate();
  const [isYearly, setIsYearly] = useState(false);
  const [showContactModal, setShowContactModal] = useState(false);

  return (
    <div className="landing-page">
      {/* Navbar */}
      <nav className="navbar">
        <div className="container">
          <div className="navbar-brand">
            <i className="icon-fuel-pump">⛽</i>
            <span className="brand-name">FuelFlow</span>
          </div>
          <div className="navbar-menu">
            <a href="#features" className="nav-link">
              Features
            </a>
            <a href="#how-it-works" className="nav-link">
              How It Works
            </a>
            <a href="#pricing" className="nav-link">
              Pricing
            </a>
            <a href="#use-cases" className="nav-link">
              Use Cases
            </a>
            <button
              onClick={() => navigate("/login")}
              className="nav-link btn-link"
            >
              Login
            </button>
            <button
              onClick={() => navigate("/register")}
              className="btn btn-primary"
            >
              Register
            </button>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section className="hero-section">
        <div className="container">
          <div className="hero-content">
            <div className="hero-text">
              <h1 className="hero-title">
                Next-Generation Fuel Management System
              </h1>
              <p className="hero-subtitle">
                Manage stations, prices, tanks, pumps, employees, and fuel data
                with full automation.
              </p>
              <div className="hero-cta">
                <button
                  onClick={() => navigate("/register")}
                  className="btn btn-primary"
                >
                  Start Free Trial
                </button>
                <a href="#pricing" className="btn btn-outline-primary">
                  View Pricing
                </a>
              </div>
              <div className="hero-stats">
                <div className="stat-item">
                  <div className="stat-number">500+</div>
                  <div className="stat-label">Active Stations</div>
                </div>
                <div className="stat-item">
                  <div className="stat-number">50M+</div>
                  <div className="stat-label">Transactions/Month</div>
                </div>
                <div className="stat-item">
                  <div className="stat-number">99.9%</div>
                  <div className="stat-label">Uptime</div>
                </div>
              </div>
            </div>
            <div className="hero-illustration">
              <div className="dashboard-mockup">
                <div className="mockup-header">
                  <div className="mockup-dots">
                    <span className="dot-red"></span>
                    <span className="dot-yellow"></span>
                    <span className="dot-green"></span>
                  </div>
                </div>
                <div className="mockup-content">
                  <div className="mockup-sidebar"></div>
                  <div className="mockup-main">
                    <div className="mockup-card"></div>
                    <div className="mockup-card"></div>
                    <div className="mockup-chart"></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* How It Works Section */}
      <section id="how-it-works" className="section-padding bg-light">
        <div className="container">
          <div className="section-header">
            <h2>How It Works</h2>
            <p className="section-subtitle">
              Simple, powerful, and automated fuel management in 4 steps
            </p>
          </div>
          <div className="steps-grid">
            <div className="step-card">
              <div className="step-number">01</div>
              <div className="step-icon">📍</div>
              <h3>Add Stations</h3>
              <p>
                Set up fuel stations across multiple cities with complete
                location and zone management.
              </p>
            </div>
            <div className="step-card">
              <div className="step-number">02</div>
              <div className="step-icon">💰</div>
              <h3>Create Zones & Prices</h3>
              <p>
                Define pricing zones and update prices for multiple stations
                simultaneously.
              </p>
            </div>
            <div className="step-card">
              <div className="step-number">03</div>
              <div className="step-icon">⚙️</div>
              <h3>Configure Infrastructure</h3>
              <p>
                Add tanks, nozzles, pumps, and employee permissions for complete
                control.
              </p>
            </div>
            <div className="step-card">
              <div className="step-number">04</div>
              <div className="step-icon">⚡</div>
              <h3>Real-Time Integration</h3>
              <p>
                Connect with PTS-Controller for automated, real-time fuel data
                tracking.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Integration Section */}
      <section className="section-padding">
        <div className="container">
          <div className="section-header">
            <h2>Seamless PTS-Controller Integration</h2>
            <p className="section-subtitle">
              Real-time data flow from station to cloud
            </p>
          </div>
          <div className="integration-pipeline">
            <div className="pipeline-step">
              <div className="pipeline-icon">⛽</div>
              <div className="pipeline-label">Fuel Station</div>
            </div>
            <div className="pipeline-arrow">→</div>
            <div className="pipeline-step">
              <div className="pipeline-icon">🖥️</div>
              <div className="pipeline-label">PTS Controller</div>
            </div>
            <div className="pipeline-arrow">→</div>
            <div className="pipeline-step">
              <div className="pipeline-icon">☁️</div>
              <div className="pipeline-label">Cloud Platform</div>
            </div>
            <div className="pipeline-arrow">→</div>
            <div className="pipeline-step">
              <div className="pipeline-icon">📊</div>
              <div className="pipeline-label">Dashboard</div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section id="features" className="section-padding bg-light">
        <div className="container">
          <div className="section-header">
            <h2>Powerful Features for Complete Control</h2>
            <p className="section-subtitle">
              Everything you need to manage your fuel operations efficiently
            </p>
          </div>
          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-icon">📈</div>
              <h3>Real-time Station Monitoring</h3>
              <p>
                Monitor all your fuel stations in real-time with live data
                updates and instant alerts.
              </p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">📄</div>
              <h3>Automated Reporting</h3>
              <p>
                Generate comprehensive reports automatically with customizable
                templates and schedules.
              </p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">💵</div>
              <h3>Centralized Pricing</h3>
              <p>
                Update fuel prices across multiple stations instantly with
                zone-based pricing control.
              </p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">👥</div>
              <h3>Employee & Permissions Management</h3>
              <p>
                Manage staff access with role-based permissions and detailed
                activity tracking.
              </p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">🔔</div>
              <h3>Alerts & Notifications</h3>
              <p>
                Receive instant notifications for critical events, anomalies,
                and system updates.
              </p>
            </div>
            <div className="feature-card">
              <div className="feature-icon">🏢</div>
              <h3>Multi-tenant Workspace</h3>
              <p>
                Manage multiple organizations and stations from a single unified
                platform.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Use Cases Section */}
      <section id="use-cases" className="section-padding">
        <div className="container">
          <div className="section-header">
            <h2>Built for Every Fuel Business</h2>
            <p className="section-subtitle">
              Trusted by industry leaders across the fuel management sector
            </p>
          </div>
          <div className="use-cases-grid">
            <div className="use-case-card">
              <div className="use-case-icon">💼</div>
              <h3>Fuel Company Owners</h3>
              <p>
                Get complete oversight of your entire fuel distribution network
                with centralized management and real-time analytics.
              </p>
              <ul className="use-case-benefits">
                <li>✓ Multi-station oversight</li>
                <li>✓ Profit tracking & analytics</li>
                <li>✓ Strategic decision support</li>
              </ul>
            </div>
            <div className="use-case-card">
              <div className="use-case-icon">👨‍💼</div>
              <h3>Station Managers</h3>
              <p>
                Streamline daily operations with automated inventory tracking,
                employee management, and real-time reporting.
              </p>
              <ul className="use-case-benefits">
                <li>✓ Inventory automation</li>
                <li>✓ Staff scheduling</li>
                <li>✓ Operational efficiency</li>
              </ul>
            </div>
            <div className="use-case-card">
              <div className="use-case-icon">🌍</div>
              <h3>Multi-City Distributors</h3>
              <p>
                Scale your operations across cities with zone-based pricing,
                regional analytics, and unified management.
              </p>
              <ul className="use-case-benefits">
                <li>✓ Regional price control</li>
                <li>✓ Cross-city analytics</li>
                <li>✓ Scalable infrastructure</li>
              </ul>
            </div>
          </div>
        </div>
      </section>

      {/* Pricing Section */}
      <section id="pricing" className="section-padding bg-light">
        <div className="container">
          <div className="section-header">
            <h2>Simple, Transparent Pricing</h2>
            <p className="section-subtitle">
              Choose the plan that fits your business needs
            </p>
          </div>
          <div className="pricing-toggle">
            <span className={!isYearly ? "active" : ""}>Monthly</span>
            <label className="switch">
              <input
                type="checkbox"
                checked={isYearly}
                onChange={() => setIsYearly(!isYearly)}
              />
              <span className="slider"></span>
            </label>
            <span className={isYearly ? "active" : ""}>
              Yearly <span className="badge">Save 20%</span>
            </span>
          </div>
          <div className="pricing-grid">
            <div className="pricing-card">
              <h3>Basic</h3>
              <div className="pricing-subtitle">For small operations</div>
              <div className="pricing-price">
                <span className="currency">$</span>
                <span className="amount">{isYearly ? "39" : "49"}</span>
                <span className="period">/month</span>
              </div>
              <ul className="pricing-features">
                <li>✓ Up to 3 stations</li>
                <li>✓ Real-time monitoring</li>
                <li>✓ Basic reporting</li>
                <li>✓ Email support</li>
                <li>✓ 5 employee accounts</li>
              </ul>
              <button
                onClick={() => navigate("/register")}
                className="btn btn-outline-primary btn-block"
              >
                Start Free Trial
              </button>
            </div>
            <div className="pricing-card featured">
              <div className="pricing-badge">Most Popular</div>
              <h3>Pro</h3>
              <div className="pricing-subtitle">For growing businesses</div>
              <div className="pricing-price">
                <span className="currency">$</span>
                <span className="amount">{isYearly ? "119" : "149"}</span>
                <span className="period">/month</span>
              </div>
              <ul className="pricing-features">
                <li>✓ Up to 15 stations</li>
                <li>✓ Advanced analytics</li>
                <li>✓ Automated reporting</li>
                <li>✓ Priority support</li>
                <li>✓ 25 employee accounts</li>
                <li>✓ Zone pricing</li>
                <li>✓ API access</li>
              </ul>
              <button
                onClick={() => navigate("/register")}
                className="btn btn-primary btn-block"
              >
                Start Free Trial
              </button>
            </div>
            <div className="pricing-card">
              <h3>Enterprise</h3>
              <div className="pricing-subtitle">For large operations</div>
              <div className="pricing-price">
                <span className="custom-price">Custom</span>
              </div>
              <ul className="pricing-features">
                <li>✓ Unlimited stations</li>
                <li>✓ Custom integrations</li>
                <li>✓ White-label options</li>
                <li>✓ 24/7 phone support</li>
                <li>✓ Unlimited employees</li>
                <li>✓ Advanced security</li>
                <li>✓ Dedicated account manager</li>
                <li>✓ Custom SLA</li>
              </ul>
              <button
                onClick={() => setShowContactModal(true)}
                className="btn btn-outline-primary btn-block"
              >
                Contact Sales
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-section">
        <div className="container">
          <h2>Ready to Transform Your Fuel Management?</h2>
          <p className="cta-subtitle">
            Join hundreds of fuel companies already using FuelFlow
          </p>
          <button
            onClick={() => navigate("/register")}
            className="btn btn-light btn-lg"
          >
            Start Your Free Trial Today
          </button>
        </div>
      </section>

      {/* Footer */}
      <footer className="footer">
        <div className="container">
          <div className="footer-grid">
            <div className="footer-column">
              <div className="footer-brand">
                <i className="icon-fuel-pump">⛽</i>
                <span>FuelFlow</span>
              </div>
              <p className="footer-description">
                Next-generation fuel management system for modern businesses.
                Automate, monitor, and scale your fuel operations with
                confidence.
              </p>
              <div className="footer-social">
                <a href="#" aria-label="Twitter">
                  𝕏
                </a>
                <a href="#" aria-label="LinkedIn">
                  in
                </a>
                <a href="#" aria-label="GitHub">
                  ⚡
                </a>
              </div>
            </div>
            <div className="footer-column">
              <h4>Product</h4>
              <ul>
                <li>
                  <a href="#features">Features</a>
                </li>
                <li>
                  <a href="#pricing">Pricing</a>
                </li>
                <li>
                  <a href="#how-it-works">How It Works</a>
                </li>
                <li>
                  <a href="#use-cases">Use Cases</a>
                </li>
              </ul>
            </div>
            <div className="footer-column">
              <h4>Company</h4>
              <ul>
                <li>
                  <a href="#">About Us</a>
                </li>
                <li>
                  <a href="#">Careers</a>
                </li>
                <li>
                  <a href="#">Blog</a>
                </li>
                <li>
                  <a href="#">Press</a>
                </li>
              </ul>
            </div>
            <div className="footer-column">
              <h4>Support</h4>
              <ul>
                <li>
                  <a
                    href="#"
                    onClick={(e) => {
                      e.preventDefault();
                      setShowContactModal(true);
                    }}
                  >
                    Contact
                  </a>
                </li>
                <li>
                  <a href="#">Documentation</a>
                </li>
                <li>
                  <a href="#">Help Center</a>
                </li>
                <li>
                  <a href="#">Status</a>
                </li>
              </ul>
            </div>
            <div className="footer-column">
              <h4>Legal</h4>
              <ul>
                <li>
                  <a href="#">Privacy Policy</a>
                </li>
                <li>
                  <a href="#">Terms of Service</a>
                </li>
                <li>
                  <a href="#">Security</a>
                </li>
                <li>
                  <a href="#">Compliance</a>
                </li>
              </ul>
            </div>
          </div>
          <div className="footer-bottom">
            <p>&copy; 2025 FuelFlow. All rights reserved.</p>
          </div>
        </div>
      </footer>

      {/* Contact Modal */}
      {showContactModal && (
        <div
          className="modal-overlay"
          onClick={() => setShowContactModal(false)}
        >
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Contact Us</h3>
              <button
                className="modal-close"
                onClick={() => setShowContactModal(false)}
              >
                ×
              </button>
            </div>
            <div className="modal-body">
              <form
                onSubmit={(e) => {
                  e.preventDefault();
                  alert("Message sent!");
                  setShowContactModal(false);
                }}
              >
                <div className="form-group">
                  <label>Name</label>
                  <input type="text" required className="form-input" />
                </div>
                <div className="form-group">
                  <label>Email</label>
                  <input type="email" required className="form-input" />
                </div>
                <div className="form-group">
                  <label>Company</label>
                  <input type="text" className="form-input" />
                </div>
                <div className="form-group">
                  <label>Message</label>
                  <textarea required className="form-input" rows={4}></textarea>
                </div>
                <button type="submit" className="btn btn-primary btn-block">
                  Send Message
                </button>
              </form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
