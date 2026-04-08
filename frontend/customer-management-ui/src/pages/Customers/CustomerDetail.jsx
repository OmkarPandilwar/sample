import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';
import { interactionApi } from '../../api/interactionApi';

const CLASSIFICATION = { 1: 'Bronze', 2: 'Silver', 3: 'Gold', 4: 'Platinum' };
const CLASS_BADGE = { 1: 'badge-bronze', 2: 'badge-silver', 3: 'badge-gold', 4: 'badge-platinum' };
const SEGMENT = { 1: 'Retail', 2: 'Corporate', 3: 'Enterprise', 4: 'Partner' };
const INT_TYPE = { 1: 'Email', 2: 'Phone Call', 3: 'Meeting', 4: 'Support Ticket' };

export default function CustomerDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [customer, setCustomer] = useState(null);
  const [interactions, setInteractions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [submitting, setSubmitting] = useState(false);

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
      console.log('Customer detail:', custRes.data);
      setCustomer(custRes.data);
      setInteractions(intRes.data);
    } catch {
      setError('Could not load customer details.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, [id]);

  const handleAddInteraction = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await interactionApi.create(id, {
        ...newInteraction,
        type: parseInt(newInteraction.type)
      });
      setNewInteraction(prev => ({ ...prev, subject: '', details: '' }));
      fetchData();
    } catch {
      alert('Failed to add interaction');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return (
    <div className="page-wrap d-flex justify-content-center align-items-center py-5">
      <div className="spinner-border text-primary me-2" role="status" />
      <span className="text-muted">Loading customer...</span>
    </div>
  );

  if (error) return (
    <div className="page-wrap">
      <div className="alert alert-danger">{error}</div>
    </div>
  );

  if (!customer) return (
    <div className="page-wrap">
      <div className="alert alert-warning">Customer not found.</div>
    </div>
  );

  return (
    <div className="page-wrap">
      {/* Header */}
      <div className="detail-header d-flex justify-content-between align-items-start flex-wrap gap-3">
        <div>
          <h2>{customer.customerName}</h2>
          <p>{customer.email} {customer.phone && `· ${customer.phone}`}</p>
        </div>
        <div className="d-flex gap-2">
          <Link to={`/customers/${id}/edit`} className="btn btn-warning btn-sm fw-semibold">✏️ Edit</Link>
          <button onClick={() => navigate('/customers')} className="btn btn-outline-light btn-sm">← Back</button>
        </div>
      </div>

      <div className="row g-3">
        {/* Left: Details */}
        <div className="col-12 col-lg-5">
          <div className="info-panel">
            <h5 className="fw-bold mb-3" style={{ color: '#475569', fontSize: '0.85rem', textTransform: 'uppercase', letterSpacing: '0.06em' }}>Customer Details</h5>
            <div className="info-row">
              <span className="info-label">Name</span>
              <span className="fw-semibold">{customer.customerName}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Email</span>
              <span style={{ color: '#4f46e5' }}>{customer.email}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Phone</span>
              <span>{customer.phone || '—'}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Website</span>
              <span>{customer.website
                ? <a href={customer.website} target="_blank" rel="noreferrer">{customer.website}</a>
                : '—'}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Industry</span>
              <span>{customer.industry || '—'}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Company Size</span>
              <span>{customer.companySize || '—'}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Classification</span>
              <span className={`badge ${CLASS_BADGE[customer.classification] || 'bg-secondary'}`}>
                {CLASSIFICATION[customer.classification] || customer.classification}
              </span>
            </div>
            <div className="info-row">
              <span className="info-label">Segment</span>
              <span className="badge bg-info text-dark">{SEGMENT[customer.segment] || customer.segment}</span>
            </div>
            <div className="info-row">
              <span className="info-label">Account Value</span>
              <span className="fw-bold text-success">
                {customer.accountValue != null ? `$${Number(customer.accountValue).toLocaleString()}` : '—'}
              </span>
            </div>
            <div className="info-row">
              <span className="info-label">Status</span>
              <span className={`badge ${customer.isActive ? 'badge-active' : 'badge-inactive'}`}>
                {customer.isActive ? 'Active' : 'Inactive'}
              </span>
            </div>
            <div className="info-row">
              <span className="info-label">Created</span>
              <span>{new Date(customer.createdDate).toLocaleDateString()}</span>
            </div>
          </div>
        </div>

        {/* Right: Interactions */}
        <div className="col-12 col-lg-7">
          <div className="info-panel">
            <h5 className="fw-bold mb-3" style={{ color: '#475569', fontSize: '0.85rem', textTransform: 'uppercase', letterSpacing: '0.06em' }}>
              Interactions ({interactions.length})
            </h5>

            {interactions.length === 0 ? (
              <p className="text-muted fst-italic">No interactions yet.</p>
            ) : interactions.map(int => (
              <div key={int.id} className="interaction-item">
                <div className="d-flex justify-content-between">
                  <span className="int-subject">{int.subject}</span>
                  <small className="text-muted">{new Date(int.interactionDate).toLocaleDateString()}</small>
                </div>
                <p className="int-meta mb-1">{int.details}</p>
                <small className="text-primary fw-semibold">
                  {INT_TYPE[int.type] || int.type} · {int.createdBy}
                </small>
              </div>
            ))}

            {/* Log Interaction Form */}
            <div className="mt-4 pt-3 border-top">
              <h6 className="fw-bold mb-3">📝 Log New Interaction</h6>
              <form onSubmit={handleAddInteraction}>
                <div className="row g-2 mb-2">
                  <div className="col-6">
                    <select
                      className="form-select form-select-sm"
                      value={newInteraction.type}
                      onChange={e => setNewInteraction(v => ({ ...v, type: e.target.value }))}
                    >
                      <option value={1}>Email</option>
                      <option value={2}>Phone Call</option>
                      <option value={3}>Meeting</option>
                      <option value={4}>Support Ticket</option>
                    </select>
                  </div>
                  <div className="col-6">
                    <input
                      type="date"
                      className="form-control form-control-sm"
                      value={newInteraction.interactionDate}
                      onChange={e => setNewInteraction(v => ({ ...v, interactionDate: e.target.value }))}
                      required
                    />
                  </div>
                </div>
                <div className="mb-2">
                  <input
                    className="form-control form-control-sm"
                    placeholder="Subject"
                    value={newInteraction.subject}
                    onChange={e => setNewInteraction(v => ({ ...v, subject: e.target.value }))}
                    required
                  />
                </div>
                <div className="mb-2">
                  <textarea
                    className="form-control form-control-sm"
                    placeholder="Details..."
                    value={newInteraction.details}
                    onChange={e => setNewInteraction(v => ({ ...v, details: e.target.value }))}
                    rows={3}
                    required
                  />
                </div>
                <button
                  type="submit"
                  className="btn btn-primary btn-sm"
                  disabled={submitting}
                  style={{ background: 'linear-gradient(135deg,#4f46e5,#4338ca)', border: 'none', borderRadius: '8px' }}
                >
                  {submitting ? <><span className="spinner-border spinner-border-sm me-1" />Saving...</> : '+ Log Interaction'}
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
