import axiosClient from './axiosClient';

export const contactApi = {
  getByCustomer: (customerId) =>
    axiosClient.get(`/Contacts/customer/${customerId}`),

  create: (data) =>
    axiosClient.post('/Contacts', data),
};