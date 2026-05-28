import axios from 'axios';

export const baseApi = axios.create({
  baseURL: 'http://localhost:5041/api',
});

baseApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});