import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../api/authApi';
import { useAuth } from '../context/AuthContext';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
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
      setError('Invalid username or password');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.page}>
      <div style={styles.card}>
        <h2 style={styles.title}>Customer Management</h2>
        <p style={styles.subtitle}>Sign in to your account</p>
        {error && <div style={styles.error}>{error}</div>}
        <form onSubmit={handleSubmit}>
          <div style={styles.field}>
            <label style={styles.label}>Username</label>
            <input style={styles.input} value={username}
              onChange={e => setUsername(e.target.value)} required />
          </div>
          <div style={styles.field}>
            <label style={styles.label}>Password</label>
            <input style={styles.input} type="password" value={password}
              onChange={e => setPassword(e.target.value)} required />
          </div>
          <button style={styles.btn} type="submit" disabled={loading}>
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>
        <div style={styles.hint}>
          <p>admin / Admin@123</p>
          <p>manager / Manager@123</p>
          <p>salesrep / Sales@123</p>
        </div>
      </div>
    </div>
  );
}

const styles = {
  page: { minHeight:'100vh', display:'flex', alignItems:'center',
    justifyContent:'center', background:'#f1f5f9' },
  card: { background:'white', padding:'40px', borderRadius:'12px',
    width:'360px', boxShadow:'0 4px 24px rgba(0,0,0,0.08)' },
  title: { margin:'0 0 4px', fontSize:'22px', color:'#1e293b' },
  subtitle: { margin:'0 0 24px', color:'#64748b', fontSize:'14px' },
  error: { background:'#fef2f2', color:'#dc2626', padding:'10px',
    borderRadius:'6px', marginBottom:'16px', fontSize:'13px' },
  field: { marginBottom:'16px' },
  label: { display:'block', fontSize:'13px', color:'#374151', marginBottom:'6px' },
  input: { width:'100%', padding:'10px 12px', border:'1px solid #d1d5db',
    borderRadius:'6px', fontSize:'14px', boxSizing:'border-box' },
  btn: { width:'100%', padding:'11px', background:'#2563eb', color:'white',
    border:'none', borderRadius:'6px', fontSize:'15px', cursor:'pointer', marginTop:'8px' },
  hint: { marginTop:'20px', padding:'12px', background:'#f8fafc',
    borderRadius:'6px', fontSize:'12px', color:'#64748b', lineHeight:'1.8' },
};