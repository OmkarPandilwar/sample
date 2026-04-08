import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/Shared/ProtectedRoute';
import Navbar from './components/Layout/Navbar';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import CustomerList from './pages/Customers/CustomerList';
import CustomerForm from './pages/Customers/CustomerForm';
import CustomerDetail from './pages/Customers/CustomerDetail';
import AnalyticsDashboard from './pages/Analytics/AnalyticsDashboard';

function Layout({ children }) {
  return (
    <div style={{ minHeight: '100vh', background: '#f8fafc' }}>
      <Navbar />
      <main>{children}</main>
    </div>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/dashboard" element={
            <ProtectedRoute><Layout><Dashboard /></Layout></ProtectedRoute>
          } />
          <Route path="/customers" element={
            <ProtectedRoute><Layout><CustomerList /></Layout></ProtectedRoute>
          } />
          <Route path="/customers/new" element={
            <ProtectedRoute><Layout><CustomerForm /></Layout></ProtectedRoute>
          } />
          <Route path="/customers/:id/edit" element={
            <ProtectedRoute><Layout><CustomerForm /></Layout></ProtectedRoute>
          } />
          <Route path="/customers/:id" element={
            <ProtectedRoute><Layout><CustomerDetail /></Layout></ProtectedRoute>
          } />
          <Route path="/analytics" element={
            <ProtectedRoute><Layout><AnalyticsDashboard /></Layout></ProtectedRoute>
          } />
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}