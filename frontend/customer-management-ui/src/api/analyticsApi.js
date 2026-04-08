import axiosClient from './axiosClient';

export const analyticsApi = {
  get: () => axiosClient.get('/customers/analytics'),
};