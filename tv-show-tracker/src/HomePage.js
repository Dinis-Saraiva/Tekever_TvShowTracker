import React from 'react';
import { Link } from 'react-router-dom';

const HomePage = ({ user }) => (
  <div>
    <h1>Welcome to MyApp</h1>
    {user ? <p>Hello, {user.name}!</p> : <p>Please <Link to="/login">log in</Link> to see your content.</p>}
  </div>
);

export default HomePage;
