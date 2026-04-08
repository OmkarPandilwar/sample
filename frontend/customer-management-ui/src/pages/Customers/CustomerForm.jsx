import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { customerApi } from '../../api/customerApi';

const SEGMENTS = [
  { value: 1, label: 'Retail' },
  { value: 2, label: 'Corporate' },
  { value: 3, label: 'Enterprise' },
  { value: 4, label: 'Partner' },
];
const CLASSIFICATIONS = [
  { value: 1, label: 'Bronze' },
  { value: 2, label: 'Silver' },
  { value: 3, label: 'Gold' },
  { value: 4, label: 'Platinum' },
];
const TYPES = [
  { value: 1, label: 'Direct' },
  { value: 2, label: 'Reseller' },
  { value: 3, label: 'Partner' },
];

export default function CustomerForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(isEdit);
  const [error, setError] = useState('');
  const [form, setForm] = useState({
    customerName: '', email: '', phone: '', website: '',
    industry: '', companySize: '', accountValue: 0,
    segment: 1, classification: 1, type: 1
  });

  useEffect(() => {
    if (isEdit) {
      customerApi.getById(id)
        .then(res => {
          console.log('Edit load:', res.data);
          setForm(res.data);
        })
        .catch(console.error)
        .finally(() => setFetching(false));
    }
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    const numericFields = ['segment', 'classification', 'type', 'accountValue'];
    setForm(prev => ({
      ...prev,
      [name]: numericFields.includes(name) ? parseFloat(value) : value
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
        type: parseInt(form.type),
        accountValue: parseFloat(form.accountValue) || 0,
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

  if (fetching) {
    return (
      <div className="page-wrap d-flex justify-content-center align-items-center py-5">
        <div className="spinner-border text-primary" role="status" />
      </div>
    );
  }

  return (
    <div className="page-wrap">
      <div className="d-flex align-items-center gap-3 mb-4">
        <button className="btn btn-sm btn-outline-secondary" onClick={() => navigate('/customers')}>
          ← Back
        </button>
        <h1 className="section-title mb-0">{isEdit ? '✏️ Edit Customer' : '➕ New Customer'}</h1>
      </div>

      {error && (
        <div className="alert alert-danger mb-3">{error}</div>
      )}

      <div className="form-card">
        <form onSubmit={handleSubmit}>
          <div className="row g-3">
            {/* Customer Name */}
            <div className="col-12 col-md-6">
              <label className="form-label">Customer Name <span className="text-danger">*</span></label>
              <input
                className="form-control"
                name="customerName"
                value={form.customerName || ''}
                onChange={handleChange}
                placeholder="e.g. Acme Corporation"
                required
              />
            </div>

            {/* Email */}
            <div className="col-12 col-md-6">
              <label className="form-label">Email <span className="text-danger">*</span></label>
              <input
                className="form-control"
                type="email"
                name="email"
                value={form.email || ''}
                onChange={handleChange}
                placeholder="e.g. contact@acme.com"
                required
              />
            </div>

            {/* Phone */}
            <div className="col-12 col-md-6">
              <label className="form-label">Phone</label>
              <input
                className="form-control"
                name="phone"
                value={form.phone || ''}
                onChange={handleChange}
                placeholder="e.g. +1 555 000 1234"
              />
            </div>

            {/* Website */}
            <div className="col-12 col-md-6">
              <label className="form-label">Website</label>
              <input
                className="form-control"
                name="website"
                value={form.website || ''}
                onChange={handleChange}
                placeholder="e.g. https://acme.com"
              />
            </div>

            {/* Industry */}
            <div className="col-12 col-md-6">
              <label className="form-label">Industry</label>
              <input
                className="form-control"
                name="industry"
                value={form.industry || ''}
                onChange={handleChange}
                placeholder="e.g. Technology"
              />
            </div>

            {/* Company Size */}
            <div className="col-12 col-md-6">
              <label className="form-label">Company Size</label>
              <input
                className="form-control"
                name="companySize"
                value={form.companySize || ''}
                onChange={handleChange}
                placeholder="e.g. 50-200"
              />
            </div>

            {/* Account Value */}
            <div className="col-12 col-md-4">
              <label className="form-label">Account Value ($)</label>
              <input
                className="form-control"
                type="number"
                name="accountValue"
                value={form.accountValue ?? 0}
                onChange={handleChange}
                min={0}
                step="0.01"
              />
            </div>

            {/* Classification */}
            <div className="col-12 col-md-4">
              <label className="form-label">Classification</label>
              <select className="form-select" name="classification" value={form.classification} onChange={handleChange}>
                {CLASSIFICATIONS.map(c => (
                  <option key={c.value} value={c.value}>{c.label}</option>
                ))}
              </select>
            </div>

            {/* Segment */}
            <div className="col-12 col-md-4">
              <label className="form-label">Segment</label>
              <select className="form-select" name="segment" value={form.segment} onChange={handleChange}>
                {SEGMENTS.map(s => (
                  <option key={s.value} value={s.value}>{s.label}</option>
                ))}
              </select>
            </div>

            {/* Type */}
            <div className="col-12 col-md-4">
              <label className="form-label">Type</label>
              <select className="form-select" name="type" value={form.type} onChange={handleChange}>
                {TYPES.map(t => (
                  <option key={t.value} value={t.value}>{t.label}</option>
                ))}
              </select>
            </div>
          </div>

          {/* Actions */}
          <div className="d-flex gap-2 mt-4 pt-3 border-top">
            <button
              type="submit"
              className="btn btn-primary px-4"
              disabled={loading}
              style={{ background: 'linear-gradient(135deg,#4f46e5,#4338ca)', border: 'none', borderRadius: '8px' }}
            >
              {loading
                ? <><span className="spinner-border spinner-border-sm me-2" />Saving...</>
                : isEdit ? '💾 Save Changes' : '✅ Create Customer'}
            </button>
            <button
              type="button"
              className="btn btn-outline-secondary px-4"
              onClick={() => navigate('/customers')}
              style={{ borderRadius: '8px' }}
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}