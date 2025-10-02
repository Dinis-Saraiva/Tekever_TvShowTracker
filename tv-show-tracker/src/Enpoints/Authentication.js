import {api} from './api';

// Login
export const login = async (username, password) => {
  try {
    const response = await api.post('/auth/login', { username, password });
    return { success: true, user: response.data.user };
  } catch (error) {
    console.error('Login error:', error);
    return { success: false, message: error.response?.data?.message || 'Login failed' };
  }
};

// Register
export const register = async (username, email, password) => {
  try {
    const response = await api.post('/auth/register', { username, email, password });
    return { success: true, user: response.data.user };
  } catch (error) {
    console.error('Register error:', error);
    return { success: false, message: error.response?.data?.message || 'Registration failed' };
  }
};

// Logout
export const logout = async () => {
  try {
    await api.post('/auth/logout');
    return { success: true };
  } catch (error) {
    console.error('Logout error:', error);
    return { success: false, message: error.response?.data?.message || 'Logout failed' };
  }
};

// Get current user
export const getCurrentUser = async () => {
  try {
    const response = await api.get('/auth/current-user');
    return { success: true, user: response.data.user };
  } catch (error) {
    console.error('Get current user error:', error);
    return { success: false, user: null };
  }
};
//Delete User
export const deleteCurrentUser = async() =>{
  try{
    const response = await api.delete('auth/delete');
    return {success:true};
  }catch(error){
    console.error('Error Deleting User');
    return{success:false};
  }
};

