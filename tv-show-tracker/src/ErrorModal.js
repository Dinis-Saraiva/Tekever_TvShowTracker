import React from 'react';
import { Modal, Button } from 'react-bootstrap';

const ErrorModal = ({ show, onClose, title = 'Error', message }) => {
  return (
    <Modal show={show} onHide={onClose} centered>
      <Modal.Header closeButton style={{ backgroundColor: '#dc3545', color: 'white' }}>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <pre style={{ whiteSpace: 'pre-wrap', margin: 0 }}>{message}</pre>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>Close</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ErrorModal;
