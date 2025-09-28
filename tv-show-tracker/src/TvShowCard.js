import { Card, Badge, Button } from 'react-bootstrap';
import { useState } from 'react';

function TvShowCard({ show }) {
  const [isFavourite, setIsFavourite] = useState(show.isFavourite || false);

  // Group people by role
  const rolesMap = {};
  if (show.workedOn) {
    show.workedOn.forEach((person) => {
      const role = person.role || 'Unknown';
      if (!rolesMap[role]) {
        rolesMap[role] = [];
      }
      if (!rolesMap[role].includes(person.name.name)) {
        rolesMap[role].push(person.name.name);
      }
    });
  }

  const handleFavouriteToggle = () => {
    setIsFavourite(!isFavourite);
    console.log(`${show.name} favourite status is now: ${!isFavourite}`);
  };

  return (
    <Card className="h-100 shadow-sm position-relative">
      {/* Favourite toggle button */}
      <Button
        onClick={handleFavouriteToggle}
        variant={isFavourite ? "danger" : "outline-secondary"}
        size="sm"
        className="position-absolute top-2 end-2"
        style={{ zIndex: 10 }}
      >
        {isFavourite ? '♥' : '♡'}
      </Button>

      {show.imageUrl && <Card.Img variant="top" src={show.imageUrl} alt={show.name} />}
      <Card.Body>
        <Card.Title>{show.name}</Card.Title>

        <Card.Subtitle className="mb-2 text-muted">
          {show.origin && <Badge bg="info" className="me-1">{show.origin}</Badge>}

          {show.tvShowGenres && show.tvShowGenres.length > 0 &&
            show.tvShowGenres.map((g) => (
              <Badge bg="secondary" key={g.genre.id} className="me-1">
                {g.genre.name}
              </Badge>
            ))
          }

          | Seasons: {show.seasons} | Rating: {show.rating}
        </Card.Subtitle>

        <Card.Text>{show.description}</Card.Text>

        <Card.Text>
          <small className="text-muted">
            Released: {new Date(show.releaseDate).toLocaleDateString()}
          </small>
        </Card.Text>

        {Object.keys(rolesMap).length > 0 && (
          <Card.Text>
            {Object.entries(rolesMap).map(([role, names]) => (
              <div key={role}>
                <strong>{role}:</strong> {names.join(', ')}
              </div>
            ))}
          </Card.Text>
        )}
      </Card.Body>
    </Card>
  );
}

export default TvShowCard;
