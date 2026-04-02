import axiosClient from './axiosClient';

export const interactionApi = {
  getByCustomer: (customerId) =>
    axiosClient.get(`/customers/${customerId}/interactions`),

  create: (customerId, data) =>
    axiosClient.post(`/customers/${customerId}/interactions`, data),
};
