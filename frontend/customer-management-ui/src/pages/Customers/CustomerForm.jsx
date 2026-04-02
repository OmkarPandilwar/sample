import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';

export default function CustomerForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    customerName: '', email: '', phone: '', website: '', industry: '', companySize: '',
    accountValue: 0, segment: 1, classification: 1, type: 1
  });

  useEffect(() => {
    if (isEdit) {
      customerApi.getById(id).then(res => setForm(res.data)).catch(console.error);
    }
  }, [id]);

  
  const handleChange = (e) => {
    const { name, value } = e.target;
    const numericFields = ['segment', 'classification', 'type', 'accountValue'];
    setForm(prev => ({
      ...prev,
      [name]: numericFields.includes(name) ? parseInt(value) : value
    }));
  };

    const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const payload = {
        ...form,
        segment: parseInt(form.segment),
        classification: parseInt(form.classification),
      };
      if (isEdit) {
        await customerApi.update(id, payload);
      } else {
        await customerApi.create(payload);
      }
      navigate('/customers');
    } catch (err) {
      const msg = err.response?.data?.error
        || err.response?.data?.title
        || err.response?.data
        || 'Something went wrong';
      setError(typeof msg === 'string' ? msg : JSON.stringify(msg));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.page}>
      <h1 style={styles.title}>{isEdit ? 'Edit Customer' : 'New Customer'}</h1>
      {error && <div style={styles.error}>{error}</div>}
      <div style={styles.card}>
        <form onSubmit={handleSubmit}>
          <div style={styles.grid}>
            <Field label="Customer Name" name="customerName" value={form.customerName} onChange={handleChange} required />
            <Field label="Email" name="email" type="email" value={form.email} onChange={handleChange} required />
            <Field label="Phone" name="phone" value={form.phone} onChange={handleChange} />
            <Field label="Website" name="website" value={form.website} onChange={handleChange} />
            <Field label="Industry" name="industry" value={form.industry} onChange={handleChange} />
            <Field label="Company Size" name="companySize" value={form.companySize} onChange={handleChange} />
            <Field label="Account Value" name="accountValue" type="number" value={form.accountValue} onChange={handleChange} />
          </div>
          <div style={styles.grid}>
            <div style={styles.field}>
              <label style={styles.label}>Segment</label>
              <select name="segment" value={form.segment} onChange={handleChange} style={styles.input}>
                <option value={1}>Retail</option>
                <option value={2}>Corporate</option>
                <option value={3}>Enterprise</option>
                <option value={4}>Partner</option>
              </select>
            </div>
            <div style={styles.field}>
              <label style={styles.label}>Classification</label>
              <select name="classification" value={form.classification} onChange={handleChange} style={styles.input}>
                <option value={1}>Bronze</option>
                <option value={2}>Silver</option>
                <option value={3}>Gold</option>
                <option value={4}>Platinum</option>
              </select>
            </div>
          </div>
          <div style={styles.actions}>
            <button type="button" onClick={() => navigate('/customers')} style={styles.cancelBtn}>Cancel</button>
            <button type="submit" style={styles.saveBtn} disabled={loading}>
              {loading ? 'Saving...' : 'Save Customer'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

function Field({ label, name, value, onChange, type = 'text', required }) {
  return (
    <div style={{ marginBottom: '16px' }}>
      <label style={{ display:'block', fontSize:'13px', color:'#374151', marginBottom:'6px' }}>{label}</label>
      <input type={type} name={name} value={value || ''} onChange={onChange} required={required}
        style={{ width:'100%', padding:'10px 12px', border:'1px solid #d1d5db',
          borderRadius:'6px', fontSize:'14px', boxSizing:'border-box' }} />
    </div>
  );
}

const styles = {
  page: { padding:'32px' },
  title: { fontSize:'24px', color:'#1e293b', marginBottom:'24px' },
  error: { background:'#fef2f2', color:'#dc2626', padding:'12px', borderRadius:'8px', marginBottom:'16px' },
  card: { background:'white', padding:'32px', borderRadius:'12px', boxShadow:'0 2px 8px rgba(0,0,0,0.06)', maxWidth:'700px' },
  grid: { display:'grid', gridTemplateColumns:'1fr 1fr', gap:'16px' },
  field: { marginBottom:'16px' },
  label: { display:'block', fontSize:'13px', color:'#374151', marginBottom:'6px' },
  input: { width:'100%', padding:'10px 12px', border:'1px solid #d1d5db', borderRadius:'6px', fontSize:'14px', boxSizing:'border-box' },
  actions: { display:'flex', gap:'12px', marginTop:'24px' },
  cancelBtn: { padding:'10px 20px', background:'#f1f5f9', color:'#374151', border:'none', borderRadius:'8px', cursor:'pointer' },
  saveBtn: { padding:'10px 24px', background:'#2563eb', color:'white', border:'none', borderRadius:'8px', cursor:'pointer' },
};