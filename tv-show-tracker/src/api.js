import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7211/api', // Replace with your API URL
  withCredentials: true, // Important for cookies
});

export default api;
