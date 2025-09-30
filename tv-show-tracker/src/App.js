import React, { useState } from 'react';
import { Routes, Route, useNavigate } from 'react-router-dom';
import Header from './Header';
import HomePage from './HomePage';
import LoginPage from './UserManagement/LoginPage';
import RegisterPage from './UserManagement/RegisterPage';
import TvShowPage from './TvShow/TvShowPage';
import TvShowDetail from './TvShow/TvShowDetail';
import PersonPage from './People/PersonPage';
import PeoplePage from './People/PeoplePage';
import Favorites from './Favorites';

const App = () => {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  const handleLogin = (username) => {
    setUser({ name: username });
    navigate('/'); // go to home after login
  };

  const handleLogout = () => {
    setUser(null);
    navigate('/'); // go to home after logout
  };

  return (
    <>
      <Header user={user} onLoginClick={() => navigate('/login')} onRegisterClick={() => navigate('/register')} onLogoutClick={handleLogout} />
      <div className="container mt-4">
        <Routes>
          <Route path="/" element={<HomePage user={user} />} />
          <Route path="/tvshows" element={<TvShowPage user={user} />} />
          <Route path="/show/:id" element={<TvShowDetail />} />
          <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/people" element={<PeoplePage />}/>
          <Route path="/person/:id" element={<PersonPage/>} />
          <Route path="/favorites" element={ <Favorites />}/>
          <Route path="*" element={<h2>Page Not Found</h2>} />

        </Routes>
      </div>
    </>
  );
};

export default App;
