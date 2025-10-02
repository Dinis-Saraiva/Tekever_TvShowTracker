import { createContext, useState, useEffect } from 'react';
import { login, logout, register, getCurrentUser, deleteCurrentUser } from '../Enpoints/Authentication';

export const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    (async () => {
      const data = await getCurrentUser();
      if (data.success && data.user) setUser(data.user);
      setLoading(false);
    })();
  }, []);

  const handleLogin = async (username, password) => {
    const data = await login(username, password);
    if (data.success && data.user) setUser(data.user);
    return data;
  };

  const handleRegister = async (username, email, password) => {
    const data = await register(username, email, password);
    if (data.success && data.user) setUser(data.user);
    return data;
  };

  const handleLogout = async () => {
    const data = await logout();
    if (data.success) setUser(null);
    return data;
  };

  const handleUserDelete= async () => {
    const data = await deleteCurrentUser();
    if (data.success) setUser(null);
    return data;
  };

  return (
    <UserContext.Provider value={{ user, loading, handleLogin, handleRegister, handleLogout, handleUserDelete }}>
      {children}
    </UserContext.Provider>
  );
};
