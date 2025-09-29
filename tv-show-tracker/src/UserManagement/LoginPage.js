import React, { useState, useContext } from 'react';
import { UserContext } from './UserContext';
import { useNavigate } from 'react-router-dom';
import { Form, Button } from 'react-bootstrap';
import ErrorModal from '../ErrorModal';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);

  const { handleLogin } = useContext(UserContext);
  const navigate = useNavigate();

  const onSubmit = async (e) => {
    e.preventDefault();
    const data = await handleLogin(username, password);
    if (data.success && data.user) {
      navigate('/');
    } else {
      setError(data.message);
      setShowModal(true);
    }
  };

  return (
    <div className="container mt-4" style={{ maxWidth: '400px' }}>
      <h2>Login</h2>
      <Form onSubmit={onSubmit}>
        <Form.Group className="mb-3">
          <Form.Label>Username</Form.Label>
          <Form.Control type="text" value={username} onChange={e => setUsername(e.target.value)} required />
        </Form.Group>
        <Form.Group className="mb-3">
          <Form.Label>Password</Form.Label>
          <Form.Control type="password" value={password} onChange={e => setPassword(e.target.value)} required />
        </Form.Group>
        <Button variant="primary" type="submit">Login</Button>
      </Form>

      <ErrorModal
        show={showModal}
        onClose={() => setShowModal(false)}
        title="Login Error"
        message={error}
      />
    </div>
  );
};

export default LoginPage;
