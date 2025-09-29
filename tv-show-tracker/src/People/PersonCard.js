import React from 'react';
import { Card } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Utils from '../Utils';

function PersonCard({ person }) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/person/${person.id}`);
  };

  return (
    <Card
      className="h-100 shadow-sm"
      onClick={handleClick}
      style={{ cursor: 'pointer' }}
    >
      {person.profileImageUrl && (
        <Card.Img
          variant="top"
          src={Utils.getImageUrl(person.profileImageUrl)}
          alt={person.name}
          style={{ objectFit: 'cover', height: '250px' }}
        />
      )}
      <Card.Body>
        <Card.Title>{person.name}</Card.Title>
        {person.birthDate && (
          <Card.Subtitle className="mb-2 text-muted">
            Born: {new Date(person.birthDate).toLocaleDateString()}
          </Card.Subtitle>
        )}
        {person.bio && (
          <Card.Text
            style={{
              maxHeight: '100px',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
            }}
          >
            {person.bio}
          </Card.Text>
        )}
      </Card.Body>
    </Card>
  );
}

export default PersonCard;
