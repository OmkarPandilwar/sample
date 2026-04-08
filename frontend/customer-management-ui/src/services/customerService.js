// customerService.js — clean API service layer
import axiosClient from '../api/axiosClient';

const BASE = '/customers';

export const customerService = {
  /** GET /api/customers */
  getCustomers: () => axiosClient.get(BASE),

  /** GET /api/customers/:id */
  getCustomerById: (id) => axiosClient.get(`${BASE}/${id}`),

  /** POST /api/customers */
  createCustomer: (data) => axiosClient.post(BASE, data),

  /** PUT /api/customers/:id */
  updateCustomer: (id, data) => axiosClient.put(`${BASE}/${id}`, data),

  /** DELETE /api/customers/:id */
  deleteCustomer: (id) => axiosClient.delete(`${BASE}/${id}`),
};

export default customerService;
