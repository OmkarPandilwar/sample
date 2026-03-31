import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';

export default function CustomerList() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    customerApi.getAll()
      .then(res => setCustomers(res.data))
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  const handleDelete = async (id) => {
    if (!confirm('Deactivate this customer?')) return;
    try {
      await customerApi.delete(id);
      setCustomers(prev => prev.filter(c => c.id !== id));
    } catch (err) {
      alert(err.response?.data?.error || 'Error deleting customer');
    }
  };

  if (loading) return <div style={styles.loading}>Loading...</div>;

  return (
    <div style={styles.page}>
      <div style={styles.header}>
        <h1 style={styles.title}>Customers</h1>
        <button onClick={() => navigate('/customers/new')} style={styles.addBtn}>
          + Add Customer
        </button>
      </div>
      <div style={styles.tableWrap}>
        <table style={styles.table}>
          <thead>
            <tr style={styles.thead}>
              <th style={styles.th}>Name</th>
              <th style={styles.th}>Email</th>
              <th style={styles.th}>Company</th>
              <th style={styles.th}>Segment</th>
              <th style={styles.th}>Status</th>
              <th style={styles.th}>Actions</th>
            </tr>
          </thead>
          <tbody>
            {customers.map(c => (
              <tr key={c.id} style={styles.tr}>
                <td style={styles.td}>{c.fullName}</td>
                <td style={styles.td}>{c.email}</td>
                <td style={styles.td}>{c.companyName || '-'}</td>
                <td style={styles.td}>{getSegmentName(c.segment)}</td>
                <td style={styles.td}>
                  <span style={c.isActive ? styles.active : styles.inactive}>
                    {c.isActive ? 'Active' : 'Inactive'}
                  </span>
                </td>
                <td style={styles.td}>
                  <Link to={`/customers/${c.id}`} style={styles.viewBtn}>View</Link>
                  <Link to={`/customers/${c.id}/edit`} style={styles.editBtn}>Edit</Link>
                  <button onClick={() => handleDelete(c.id)} style={styles.delBtn}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {customers.length === 0 && (
          <div style={styles.empty}>No customers found. Add your first customer!</div>
        )}
      </div>
    </div>
  );
}

const getSegmentName = (s) => ['','Retail','Corporate','Enterprise','Partner'][s] || s;

const styles = {
  page: { padding:'32px' },
  header: { display:'flex', justifyContent:'space-between', alignItems:'center', marginBottom:'24px' },
  title: { fontSize:'24px', color:'#1e293b', margin:0 },
  addBtn: { padding:'10px 20px', background:'#2563eb', color:'white',
    border:'none', borderRadius:'8px', cursor:'pointer', fontSize:'14px' },
  tableWrap: { background:'white', borderRadius:'12px', boxShadow:'0 2px 8px rgba(0,0,0,0.06)', overflow:'hidden' },
  table: { width:'100%', borderCollapse:'collapse' },
  thead: { background:'#f8fafc' },
  th: { padding:'12px 16px', textAlign:'left', fontSize:'13px',
    fontWeight:'600', color:'#374151', borderBottom:'1px solid #e5e7eb' },
  tr: { borderBottom:'1px solid #f1f5f9' },
  td: { padding:'12px 16px', fontSize:'14px', color:'#374151' },
  active: { background:'#dcfce7', color:'#166534', padding:'2px 10px', borderRadius:'99px', fontSize:'12px' },
  inactive: { background:'#fee2e2', color:'#991b1b', padding:'2px 10px', borderRadius:'99px', fontSize:'12px' },
  viewBtn: { marginRight:'8px', color:'#2563eb', fontSize:'13px', textDecoration:'none' },
  editBtn: { marginRight:'8px', color:'#059669', fontSize:'13px', textDecoration:'none' },
  delBtn: { color:'#dc2626', fontSize:'13px', background:'none', border:'none', cursor:'pointer' },
  loading: { padding:'32px', textAlign:'center', color:'#64748b' },
  empty: { padding:'32px', textAlign:'center', color:'#94a3b8' },
};