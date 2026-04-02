import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';
import { interactionApi } from '../../api/interactionApi';

export default function CustomerDetail() {
  const { id } = useParams();
  const [customer, setCustomer] = useState(null);
  const [interactions, setInteractions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  const [newInteraction, setNewInteraction] = useState({
    type: 1,
    subject: '',
    details: '',
    interactionDate: new Date().toISOString().substring(0, 10),
  });

  const fetchData = async () => {
    try {
      const [custRes, intRes] = await Promise.all([
        customerApi.getById(id),
        interactionApi.getByCustomer(id).catch(() => ({ data: [] }))
      ]);
      setCustomer(custRes.data);
      setInteractions(intRes.data);
    } catch (err) {
      setError('Could not load customer details.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line
  }, [id]);

  const handleAddInteraction = async (e) => {
    e.preventDefault();
    if (!newInteraction.subject || !newInteraction.details) return;
    try {
      await interactionApi.create(id, {
        ...newInteraction,
        type: parseInt(newInteraction.type)
      });
      setNewInteraction(prev => ({ ...prev, subject: '', details: '' }));
      fetchData(); // refresh list
    } catch (err) {
      alert('Failed to add interaction');
    }
  };

  if (loading) return <div className="content-area">Loading...</div>;
  if (error) return <div className="content-area" style={{ color: 'red' }}>{error}</div>;
  if (!customer) return <div className="content-area">Customer not found</div>;

  return (
    <div className="content-area">
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '2rem' }}>
        <h2>{customer.customerName}</h2>
        <Link to="/customers" className="btn-primary" style={{ textDecoration: 'none' }}>Back to Customers</Link>
      </div>

      <div className="glass-panel" style={{ padding: '2rem', marginBottom: '2rem' }}>
        <h3>Details</h3>
        <p><strong>Email:</strong> {customer.email}</p>
        <p><strong>Phone:</strong> {customer.phone || 'N/A'}</p>
        <p><strong>Website:</strong> {customer.website || 'N/A'}</p>
        <p><strong>Status:</strong> <span className={`badge ${customer.isActive ? 'badge-active' : 'badge-inactive'}`}>{customer.isActive ? 'Active' : 'Inactive'}</span></p>
      </div>

      <div className="glass-panel" style={{ padding: '2rem' }}>
        <h3>Interactions</h3>
        {interactions.length === 0 ? (
          <p style={{ color: 'var(--text-muted)' }}>No interactions logged yet.</p>
        ) : (
          <div style={{ marginBottom: '2rem' }}>
            {interactions.map(int => (
              <div key={int.id} style={{ borderBottom: '1px solid var(--border)', padding: '1rem 0' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                  <strong>{int.subject}</strong>
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>{new Date(int.interactionDate).toLocaleDateString()}</span>
                </div>
                <p style={{ fontSize: '0.9rem', marginTop: '0.5rem' }}>{int.details}</p>
                <small style={{ color: 'var(--primary)' }}>Type: {['Unknown', 'Email', 'Call', 'Meeting', 'SupportTicket'][int.type] || int.type} • By: {int.createdBy}</small>
              </div>
            ))}
          </div>
        )}

        <div style={{ marginTop: '2rem', background: 'rgba(0,0,0,0.2)', padding: '1.5rem', borderRadius: '8px' }}>
          <h4>Log New Interaction</h4>
          <form onSubmit={handleAddInteraction} style={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginTop: '1rem' }}>
            <div style={{ display: 'flex', gap: '1rem' }}>
              <select 
                value={newInteraction.type} 
                onChange={(e) => setNewInteraction(v => ({...v, type: e.target.value}))}
                style={{ flex: 1 }}
              >
                <option value={1}>Email</option>
                <option value={2}>Phone Call</option>
                <option value={3}>Meeting</option>
                <option value={4}>Support Ticket</option>
              </select>
              <input 
                type="date" 
                value={newInteraction.interactionDate} 
                onChange={(e) => setNewInteraction(v => ({...v, interactionDate: e.target.value}))}
                style={{ flex: 1 }}
                required 
              />
            </div>
            <input 
              type="text" 
              placeholder="Subject" 
              value={newInteraction.subject}
              onChange={(e) => setNewInteraction(v => ({...v, subject: e.target.value}))}
              required 
            />
            <textarea 
              placeholder="Details..." 
              value={newInteraction.details}
              onChange={(e) => setNewInteraction(v => ({...v, details: e.target.value}))}
              rows={3} 
              required 
            />
            <button type="submit" className="btn-primary" style={{ alignSelf: 'flex-start' }}>Log Interaction</button>
          </form>
        </div>
      </div>
    </div>
  );
}
