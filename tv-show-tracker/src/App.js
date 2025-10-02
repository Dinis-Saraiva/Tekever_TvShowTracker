import { Routes, Route} from 'react-router-dom';
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
 
  return (
    <>
      <Header/>
      <div className="container mt-4">
        <Routes>
          <Route path="/" element={<HomePage/>} />
          <Route path="/tvshows" element={<TvShowPage/>} />
          <Route path="/show/:id" element={<TvShowDetail />} />
          <Route path="/login" element={<LoginPage/>} />
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
