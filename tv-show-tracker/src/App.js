import React, { useState, useEffect } from 'react';
import { Container, Form, Button, Alert, Card } from 'react-bootstrap';
import api from './api';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');
  const [currentUser, setCurrentUser] = useState(null);
  const [showLogin, setShowLogin] = useState(true); // toggle between login/register

  const register = async () => {
    try {
      const res = await api.post('/auth/register', { username, email, password });
      setMessage(res.data.message);
      setShowLogin(true); // after register, go to login
    } catch (err) {
      setMessage(err.response?.data[0]?.description || err.message);
    }
  };

  const login = async () => {
    try {
      const res = await api.post('/auth/login', { username, password });
      setMessage(res.data.message);
      fetchCurrentUser();
    } catch (err) {
      setMessage(err.response?.data || err.message);
    }
  };

  const logout = async () => {
    try {
      const res = await api.post('/auth/logout');
      setMessage(res.data.message);
      setCurrentUser(null);
    } catch (err) {
      setMessage(err.response?.data || err.message);
    }
  };

  const fetchCurrentUser = async () => {
    try {
      const res = await api.get('/auth/me');
      setCurrentUser(res.data.user);
    } catch {
      setCurrentUser(null);
    }
  };

  useEffect(() => {
    fetchCurrentUser();
  }, []);

  return (
    <Container className="mt-5">
      <Card className="p-4 shadow-sm">
        <h1 className="mb-4 text-center">TV Show Tracker Auth</h1>

        {currentUser ? (
          <div className="text-center">
            <h2>Welcome, {currentUser}</h2>
            <Button variant="danger" onClick={logout} className="mt-3">
              Logout
            </Button>
          </div>
        ) : (
          <div>
            {showLogin ? (
              <>
                <h2>Login</h2>
                <Form>
                  <Form.Group className="mb-3">
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                      type="text"
                      placeholder="Enter username"
                      value={username}
                      onChange={e => setUsername(e.target.value)}
                    />
                  </Form.Group>
                  <Form.Group className="mb-3">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                      type="password"
                      placeholder="Password"
                      value={password}
                      onChange={e => setPassword(e.target.value)}
                    />
                  </Form.Group>
                  <Button variant="primary" onClick={login} className="me-2">
                    Login
                  </Button>
                  <Button variant="secondary" onClick={() => setShowLogin(false)}>
                    Go to Register
                  </Button>
                </Form>
              </>
            ) : (
              <>
                <h2>Register</h2>
                <Form>
                  <Form.Group className="mb-3">
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                      type="text"
                      placeholder="Enter username"
                      value={username}
                      onChange={e => setUsername(e.target.value)}
                    />
                  </Form.Group>
                  <Form.Group className="mb-3">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                      type="email"
                      placeholder="Enter email"
                      value={email}
                      onChange={e => setEmail(e.target.value)}
                    />
                  </Form.Group>
                  <Form.Group className="mb-3">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                      type="password"
                      placeholder="Password"
                      value={password}
                      onChange={e => setPassword(e.target.value)}
                    />
                  </Form.Group>
                  <Button variant="success" onClick={register} className="me-2">
                    Register
                  </Button>
                  <Button variant="secondary" onClick={() => setShowLogin(true)}>
                    Go to Login
                  </Button>
                </Form>
              </>
            )}
          </div>
        )}

        {message && <Alert className="mt-3" variant="info">{message}</Alert>}
      </Card>
    </Container>
  );
}

export default App;
