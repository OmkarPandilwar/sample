import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="cm-navbar">
      <Link to="/dashboard" className="brand">⚡ CRM Pro</Link>
      <div className="nav-links">
        <Link to="/dashboard">Dashboard</Link>
        <Link to="/customers">Customers</Link>
        {(user?.role === 'Admin' || user?.role === 'SalesManager') && (
          <Link to="/analytics">Analytics</Link>
        )}
      </div>
      <div className="user-area">
        <span className="user-badge">👤 {user?.username} · {user?.role}</span>
        <button className="btn-logout" onClick={handleLogout}>Logout</button>
      </div>
    </nav>
  );
}