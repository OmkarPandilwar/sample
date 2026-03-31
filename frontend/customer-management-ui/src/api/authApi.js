import axiosClient from './axiosClient';

export const authApi = {
  login: (username, password) =>
    axiosClient.post('/Auth/login', { username, password }),
};