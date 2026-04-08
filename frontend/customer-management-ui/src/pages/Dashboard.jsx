import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';

export default function Dashboard() {
  const { user } = useAuth();

  return (
    <div className="page-wrap">
      {/* Hero Banner */}
      <div className="dashboard-hero">
        <h2>Welcome back, {user?.username}! 👋</h2>
        <p>Role: <strong>{user?.role}</strong> &nbsp;·&nbsp; Customer Management System</p>
      </div>

      {/* Stats Row */}
      <div className="row g-3 mb-4">
        <div className="col-6 col-md-3">
          <div className="card border-0 shadow-sm h-100" style={{ borderRadius: '12px' }}>
            <div className="card-body text-center py-3">
              <div style={{ fontSize: '1.8rem' }}>👥</div>
              <div className="fw-bold mt-1" style={{ fontSize: '0.9rem', color: '#475569' }}>Total Customers</div>
            </div>
          </div>
        </div>
        <div className="col-6 col-md-3">
          <div className="card border-0 shadow-sm h-100" style={{ borderRadius: '12px' }}>
            <div className="card-body text-center py-3">
              <div style={{ fontSize: '1.8rem' }}>📈</div>
              <div className="fw-bold mt-1" style={{ fontSize: '0.9rem', color: '#475569' }}>Active Deals</div>
            </div>
          </div>
        </div>
        <div className="col-6 col-md-3">
          <div className="card border-0 shadow-sm h-100" style={{ borderRadius: '12px' }}>
            <div className="card-body text-center py-3">
              <div style={{ fontSize: '1.8rem' }}>💰</div>
              <div className="fw-bold mt-1" style={{ fontSize: '0.9rem', color: '#475569' }}>Revenue</div>
            </div>
          </div>
        </div>
        <div className="col-6 col-md-3">
          <div className="card border-0 shadow-sm h-100" style={{ borderRadius: '12px' }}>
            <div className="card-body text-center py-3">
              <div style={{ fontSize: '1.8rem' }}>🎯</div>
              <div className="fw-bold mt-1" style={{ fontSize: '0.9rem', color: '#475569' }}>Goals</div>
            </div>
          </div>
        </div>
      </div>

      {/* Navigation Cards */}
      <h5 className="fw-bold mb-3" style={{ color: '#475569', fontSize: '0.9rem', textTransform: 'uppercase', letterSpacing: '0.06em' }}>Quick Access</h5>
      <div className="row g-3">
        <div className="col-12 col-sm-6 col-md-4">
          <Link to="/customers" className="dash-card">
            <div className="dc-icon">👥</div>
            <div className="dc-title">Customers</div>
            <div className="dc-desc">View, add, edit and manage all your customers</div>
          </Link>
        </div>
        {(user?.role === 'Admin' || user?.role === 'SalesManager') && (
          <div className="col-12 col-sm-6 col-md-4">
            <Link to="/analytics" className="dash-card">
              <div className="dc-icon">📊</div>
              <div className="dc-title">Analytics</div>
              <div className="dc-desc">Revenue insights, segments and churn risk</div>
            </Link>
          </div>
        )}
        <div className="col-12 col-sm-6 col-md-4">
          <Link to="/customers/new" className="dash-card">
            <div className="dc-icon">➕</div>
            <div className="dc-title">Add Customer</div>
            <div className="dc-desc">Create a new customer record</div>
          </Link>
        </div>
      </div>
    </div>
  );
}