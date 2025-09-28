import axios from 'axios';

// REST API client
const api = axios.create({
  baseURL: 'https://localhost:7211/api', // Replace with your API URL
  withCredentials: true, // Important for cookies
});

// GraphQL endpoint URL
const endpoint = 'https://localhost:7211/graphql';

// Function to make GraphQL requests
const graphql = async (query, variables = {}) => {
  const response = await axios.post(endpoint, {
    query,
    variables,
  }, {
    withCredentials: true,
  });
  return response.data;
};

export { api, graphql };