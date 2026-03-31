import axiosClient from './axiosClient';

export const customerApi = {
  getAll: (activeOnly = false) =>
    axiosClient.get(`/Customers?activeOnly=${activeOnly}`),

  getById: (id) =>
    axiosClient.get(`/Customers/${id}`),

  create: (data) =>
    axiosClient.post('/Customers', data),

  update: (id, data) =>
    axiosClient.put(`/Customers/${id}`, data),

  delete: (id) =>
    axiosClient.delete(`/Customers/${id}`),
};