import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';

const CLASSIFICATION = { 1: 'Bronze', 2: 'Silver', 3: 'Gold', 4: 'Platinum' };
const SEGMENT = { 1: 'Retail', 2: 'Corporate', 3: 'Enterprise', 4: 'Partner' };
const CLASS_BADGE = { 1: 'badge-bronze', 2: 'badge-silver', 3: 'badge-gold', 4: 'badge-platinum' };

export default function CustomerList() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    customerApi.getAll()
      .then(res => {
        console.log('API Response:', res.data);
        setCustomers(res.data);
      })
      .catch(err => {
        console.error(err);
        setError('Failed to load customers. Please try again.');
      })
      .finally(() => setLoading(false));
  }, []);

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this customer?')) return;
    try {
      await customerApi.delete(id);
      setCustomers(prev => prev.filter(c => c.id !== id));
    } catch (err) {
      alert(err.response?.data?.error || 'Error deleting customer');
    }
  };

  return (
    <div className="page-wrap">
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1 className="section-title">Customers</h1>
        <button className="btn btn-primary btn-primary-cm" onClick={() => navigate('/customers/new')}>
          + Add Customer
        </button>
      </div>

      {/* Error */}
      {error && (
        <div className="alert alert-danger d-flex align-items-center" role="alert">
          <svg className="bi flex-shrink-0 me-2" width="16" height="16" role="img"><path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/></svg>
          {error}
        </div>
      )}

      {/* Table */}
      <div className="card border-0 shadow-sm" style={{ borderRadius: '14px', overflow: 'hidden' }}>
        <div className="card-body p-0">
          {loading ? (
            <div className="d-flex justify-content-center align-items-center py-5">
              <div className="spinner-border text-primary me-2" role="status" />
              <span className="text-muted">Loading customers...</span>
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover mb-0">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Industry</th>
                    <th>Classification</th>
                    <th>Segment</th>
                    <th>Account Value</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {customers.length === 0 ? (
                    <tr>
                      <td colSpan={9} className="text-center text-muted py-5">
                        No customers found. <Link to="/customers/new">Add your first customer →</Link>
                      </td>
                    </tr>
                  ) : customers.map(c => (
                    <tr key={c.id}>
                      <td className="fw-semibold">{c.customerName}</td>
                      <td style={{ color: '#4f46e5' }}>{c.email}</td>
                      <td>{c.phone || <span className="text-muted">—</span>}</td>
                      <td>{c.industry || <span className="text-muted">—</span>}</td>
                      <td>
                        <span className={`badge ${CLASS_BADGE[c.classification] || 'bg-secondary'}`}>
                          {CLASSIFICATION[c.classification] || c.classification}
                        </span>
                      </td>
                      <td>
                        <span className="badge bg-info text-dark">
                          {SEGMENT[c.segment] || c.segment}
                        </span>
                      </td>
                      <td className="fw-semibold">
                        {c.accountValue != null
                          ? `$${Number(c.accountValue).toLocaleString()}`
                          : <span className="text-muted">—</span>}
                      </td>
                      <td>
                        {c.isActive
                          ? <span className="badge badge-active">Active</span>
                          : <span className="badge badge-inactive">Inactive</span>}
                      </td>
                      <td>
                        <div className="d-flex gap-1">
                          <Link to={`/customers/${c.id}`} className="btn btn-sm btn-outline-primary">View</Link>
                          <Link to={`/customers/${c.id}/edit`} className="btn btn-sm btn-warning">Edit</Link>
                          <button onClick={() => handleDelete(c.id)} className="btn btn-sm btn-danger">Delete</button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {customers.length > 0 && (
        <p className="text-muted mt-2" style={{ fontSize: '0.82rem' }}>
          Showing {customers.length} customer{customers.length !== 1 ? 's' : ''}
        </p>
      )}
    </div>
  );
}