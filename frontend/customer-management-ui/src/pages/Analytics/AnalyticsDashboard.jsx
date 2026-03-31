import { useEffect, useState } from 'react';
import { analyticsApi } from '../../api/analyticsApi';

export default function AnalyticsDashboard() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    analyticsApi.get()
      .then(res => setData(res.data))
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div style={{ padding: '32px' }}>Loading analytics...</div>;
  if (!data) return <div style={{ padding: '32px' }}>No data available</div>;

  return (
    <div style={styles.page}>
      <h1 style={styles.title}>Customer Analytics</h1>
      <p style={styles.cached}>Cached in Redis — refreshes every 10 minutes</p>
      <div style={styles.grid}>
        <StatCard label="Total Customers" value={data.totalCustomers} color="#2563eb" />
        <StatCard label="Active Customers" value={data.activeCustomers} color="#059669" />
        <StatCard label="Inactive Customers" value={data.inactiveCustomers} color="#dc2626" />
        <StatCard label="Total Interactions" value={data.totalInteractions} color="#7c3aed" />
      </div>
      <div style={styles.row}>
        <BreakdownCard title="By Segment" data={data.customersBySegment} />
        <BreakdownCard title="By Classification" data={data.customersByClassification} />
      </div>
    </div>
  );
}

function StatCard({ label, value, color }) {
  return (
    <div style={{ ...styles.statCard, borderTop: `4px solid ${color}` }}>
      <div style={{ ...styles.statValue, color }}>{value}</div>
      <div style={styles.statLabel}>{label}</div>
    </div>
  );
}

function BreakdownCard({ title, data }) {
  return (
    <div style={styles.breakCard}>
      <h3 style={styles.breakTitle}>{title}</h3>
      {Object.entries(data).map(([key, val]) => (
        <div key={key} style={styles.breakRow}>
          <span>{key}</span>
          <span style={styles.breakVal}>{val}</span>
        </div>
      ))}
    </div>
  );
}

const styles = {
  page: { padding: '32px' },
  title: { fontSize: '24px', color: '#1e293b', margin: '0 0 4px' },
  cached: { color: '#64748b', fontSize: '13px', marginBottom: '24px' },
  grid: { display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '16px', marginBottom: '24px' },
  statCard: { background: 'white', padding: '24px', borderRadius: '12px', boxShadow: '0 2px 8px rgba(0,0,0,0.06)' },
  statValue: { fontSize: '36px', fontWeight: '700', marginBottom: '4px' },
  statLabel: { fontSize: '13px', color: '#64748b' },
  row: { display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' },
  breakCard: { background: 'white', padding: '24px', borderRadius: '12px', boxShadow: '0 2px 8px rgba(0,0,0,0.06)' },
  breakTitle: { fontSize: '16px', color: '#1e293b', margin: '0 0 16px' },
  breakRow: { display: 'flex', justifyContent: 'space-between', padding: '8px 0', borderBottom: '1px solid #f1f5f9', fontSize: '14px' },
  breakVal: { fontWeight: '600', color: '#2563eb' },
};