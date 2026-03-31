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
    <nav style={styles.nav}>
      <div style={styles.brand}>Customer Management</div>
      <div style={styles.links}>
        <Link to="/dashboard" style={styles.link}>Dashboard</Link>
        <Link to="/customers" style={styles.link}>Customers</Link>
        {(user?.role === 'Admin' || user?.role === 'SalesManager') && (
          <Link to="/analytics" style={styles.link}>Analytics</Link>
        )}
      </div>
      <div style={styles.user}>
        <span style={styles.username}>{user?.username} ({user?.role})</span>
        <button onClick={handleLogout} style={styles.logoutBtn}>Logout</button>
      </div>
    </nav>
  );
}

const styles = {
  nav: { display:'flex', alignItems:'center', justifyContent:'space-between',
    padding:'0 24px', height:'56px', background:'#1e293b', color:'white' },
  brand: { fontWeight:'700', fontSize:'18px', color:'#38bdf8' },
  links: { display:'flex', gap:'24px' },
  link: { color:'#cbd5e1', textDecoration:'none', fontSize:'14px' },
  user: { display:'flex', alignItems:'center', gap:'12px' },
  username: { fontSize:'13px', color:'#94a3b8' },
  logoutBtn: { padding:'6px 14px', background:'#ef4444', color:'white',
    border:'none', borderRadius:'6px', cursor:'pointer', fontSize:'13px' },
};