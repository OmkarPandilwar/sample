import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../api/authApi';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const res = await authApi.login(username, password);
      login(res.data);
      navigate('/dashboard');
    } catch {
      setError('Invalid username or password. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-bg">
      <div className="login-card">
        <div className="login-logo">🏢</div>
        <h1 className="login-title">Customer Management</h1>
        <p className="login-subtitle">Sign in to your account to continue</p>

        {error && (
          <div className="alert alert-danger d-flex align-items-center py-2 px-3 mb-3" style={{ fontSize: '0.88rem' }}>
            <span>⚠️</span>&nbsp;{error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label fw-semibold" style={{ fontSize: '0.87rem' }}>Username</label>
            <div className="input-group">
              <span className="input-group-text bg-light border-end-0">
                <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16"><path d="M8 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6zm2-3a2 2 0 1 1-4 0 2 2 0 0 1 4 0zm4 8c0 1-1 1-1 1H3s-1 0-1-1 1-4 6-4 6 3 6 4z"/></svg>
              </span>
              <input
                className="form-control border-start-0"
                value={username}
                onChange={e => setUsername(e.target.value)}
                placeholder="Enter your username"
                required
                autoFocus
              />
            </div>
          </div>

          <div className="mb-3">
            <label className="form-label fw-semibold" style={{ fontSize: '0.87rem' }}>Password</label>
            <div className="input-group">
              <span className="input-group-text bg-light border-end-0">
                <svg width="16" height="16" fill="currentColor" viewBox="0 0 16 16"><path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2zm3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z"/></svg>
              </span>
              <input
                className="form-control border-start-0 border-end-0"
                type={showPassword ? 'text' : 'password'}
                value={password}
                onChange={e => setPassword(e.target.value)}
                placeholder="Enter your password"
                required
              />
              <button
                type="button"
                className="input-group-text bg-light"
                onClick={() => setShowPassword(v => !v)}
                style={{ cursor: 'pointer', border: '1.5px solid #dee2e6', borderLeft: 'none' }}
              >
                {showPassword ? '🙈' : '👁️'}
              </button>
            </div>
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100 py-2 fw-semibold mt-1"
            disabled={loading}
            style={{ background: 'linear-gradient(135deg,#4f46e5,#4338ca)', border: 'none', borderRadius: '10px', fontSize: '1rem' }}
          >
            {loading ? (
              <><span className="spinner-border spinner-border-sm me-2" role="status" />Signing in...</>
            ) : '🔐 Sign In'}
          </button>
        </form>

        <div className="login-hint mt-3">
          <strong>Demo Credentials</strong>
          <div className="d-flex flex-column gap-1">
            <span>👑 <strong>admin</strong> / admin</span>
            <span>📊 <strong>manager</strong> / Manager@123</span>
            <span>💼 <strong>salesrep</strong> / Sales@123</span>
          </div>
        </div>
      </div>
    </div>
  );
}