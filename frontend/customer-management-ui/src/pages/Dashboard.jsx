import { useAuth } from '../context/AuthContext';
import { Link } from 'react-router-dom';

export default function Dashboard() {
  const { user } = useAuth();

  return (
    <div style={styles.page}>
      <h1 style={styles.title}>Welcome, {user?.username}!</h1>
      <p style={styles.role}>Role: {user?.role}</p>
      <div style={styles.grid}>
        <Link to="/customers" style={styles.card}>
          <div style={styles.icon}>👥</div>
          <div style={styles.cardTitle}>Customers</div>
          <div style={styles.cardDesc}>Manage all customers</div>
        </Link>
        {(user?.role === 'Admin' || user?.role === 'SalesManager') && (
          <Link to="/analytics" style={styles.card}>
            <div style={styles.icon}>📊</div>
            <div style={styles.cardTitle}>Analytics</div>
            <div style={styles.cardDesc}>View customer analytics</div>
          </Link>
        )}
      </div>
    </div>
  );
}

const styles = {
  page: { padding:'32px' },
  title: { fontSize:'24px', color:'#1e293b', margin:'0 0 4px' },
  role: { color:'#64748b', marginBottom:'32px' },
  grid: { display:'grid', gridTemplateColumns:'repeat(auto-fill, minmax(200px, 1fr))', gap:'16px', maxWidth:'600px' },
  card: { background:'white', padding:'24px', borderRadius:'12px',
    boxShadow:'0 2px 8px rgba(0,0,0,0.06)', textDecoration:'none',
    color:'#1e293b', display:'block', transition:'box-shadow 0.2s' },
  icon: { fontSize:'32px', marginBottom:'12px' },
  cardTitle: { fontWeight:'600', fontSize:'16px', marginBottom:'4px' },
  cardDesc: { fontSize:'13px', color:'#64748b' },
};