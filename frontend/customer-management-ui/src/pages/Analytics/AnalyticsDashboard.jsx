import { useEffect, useState } from 'react';
import { analyticsApi } from '../../api/analyticsApi';

const STAT_COLORS = [
  { border: '#4f46e5', bg: '#ede9fe', icon: '👥', label: 'Total Customers', key: 'totalCustomers' },
  { border: '#10b981', bg: '#d1fae5', icon: '✅', label: 'Active', key: 'activeCustomers' },
  { border: '#ef4444', bg: '#fee2e2', icon: '❌', label: 'Inactive', key: 'inactiveCustomers' },
  { border: '#f59e0b', bg: '#fef3c7', icon: '💬', label: 'Interactions', key: 'totalInteractions' },
];

export default function AnalyticsDashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    analyticsApi.get()
      .then(res => setData(res.data))
      .catch(() => setError('Failed to load analytics data.'))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return (
    <div className="page-wrap d-flex justify-content-center align-items-center py-5">
      <div className="spinner-border text-primary me-2" role="status" />
      <span className="text-muted">Loading analytics...</span>
    </div>
  );

  if (error) return (
    <div className="page-wrap">
      <div className="alert alert-danger">{error}</div>
    </div>
  );

  if (!data) return (
    <div className="page-wrap">
      <div className="alert alert-warning">No analytics data available.</div>
    </div>
  );

  return (
    <div className="page-wrap">
      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="section-title">📊 Analytics</h1>
          <p className="text-muted mb-0" style={{ fontSize: '0.85rem' }}>
            Cached via Redis · refreshes every 10 minutes
          </p>
        </div>
      </div>

      {/* KPI Cards */}
      <div className="row g-3 mb-4">
        {STAT_COLORS.map(({ border, bg, icon, label, key }) => (
          <div key={key} className="col-6 col-md-3">
            <div
              className="card border-0 shadow-sm h-100"
              style={{ borderRadius: '14px', borderLeft: `4px solid ${border}`, background: bg }}
            >
              <div className="card-body d-flex align-items-center gap-3 py-3">
                <div style={{ fontSize: '2rem' }}>{icon}</div>
                <div>
                  <div className="fw-bold" style={{ fontSize: '1.6rem', color: border, lineHeight: 1 }}>
                    {data[key] ?? '—'}
                  </div>
                  <div className="text-muted" style={{ fontSize: '0.82rem' }}>{label}</div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Breakdown Cards */}
      <div className="row g-3">
        <div className="col-12 col-md-6">
          <BreakdownCard
            title="👥 By Segment"
            data={data.customersBySegment}
            color="#4f46e5"
          />
        </div>
        <div className="col-12 col-md-6">
          <BreakdownCard
            title="🏅 By Classification"
            data={data.customersByClassification}
            color="#f59e0b"
          />
        </div>
      </div>
    </div>
  );
}

function BreakdownCard({ title, data, color }) {
  const total = Object.values(data).reduce((a, b) => a + b, 0);
  return (
    <div className="card border-0 shadow-sm h-100" style={{ borderRadius: '14px' }}>
      <div className="card-body">
        <h6 className="fw-bold mb-3" style={{ color: '#475569' }}>{title}</h6>
        {Object.entries(data).map(([key, val]) => {
          const pct = total > 0 ? Math.round((val / total) * 100) : 0;
          return (
            <div key={key} className="mb-3">
              <div className="d-flex justify-content-between mb-1">
                <span style={{ fontSize: '0.88rem', fontWeight: 500 }}>{key}</span>
                <span style={{ fontSize: '0.88rem', color: '#64748b' }}>{val} ({pct}%)</span>
              </div>
              <div className="progress" style={{ height: '6px', borderRadius: '99px' }}>
                <div
                  className="progress-bar"
                  role="progressbar"
                  style={{ width: `${pct}%`, background: color, borderRadius: '99px' }}
                />
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}