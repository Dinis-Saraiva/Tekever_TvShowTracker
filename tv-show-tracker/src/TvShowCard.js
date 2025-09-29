import React from 'react';
import { Card, Badge } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import utils from './Utils';

const TvShowCard = ({ show }) => {
  const navigate = useNavigate();

  const getRandomPlaceholder = () => {
    const placeholders = [
      "https://picsum.photos/300/450?random=1",
      "https://picsum.photos/300/450?random=2",
      "https://picsum.photos/300/450?random=3",
      "https://picsum.photos/300/450?random=4",
    ];
    return placeholders[Math.floor(Math.random() * placeholders.length)];
  };

  const handleClick = () => {
    navigate(`/show/${show.id}`); // navigate to dynamic route
  };

  return (
    <Card
      style={{ width: "18rem", margin: "1rem", cursor: "pointer" }}
      className="shadow-sm"
      onClick={handleClick}
    >
      <Card.Img
        variant="top"
        src={utils.getImageUrl(show.imageUrl) || getRandomPlaceholder()}
        alt={show.name || "TV Show"}
        style={{ height: "300px", objectFit: "cover" }}
      />
      <Card.Body>
        <Card.Title>{show.name || "Unknown"}</Card.Title>
        <Card.Text>{show.description || "No description available."}</Card.Text>
        <ul className="list-unstyled mb-2">
          <li><strong>Release:</strong> {utils.formatDate(show.releaseDate) || "N/A"}</li>
          <li><strong>Seasons:</strong> {show.seasons || "N/A"}</li>
          <li><strong>Rating:</strong> {show.rating || "N/A"}</li>
          <li><strong>Origin:</strong> {show.origin || "N/A"}</li>
        </ul>
        <div>
          {show.genres?.map((genre) => (
            <Badge key={genre.name} bg="secondary" className="me-1">
              {genre.name}
            </Badge>
          ))}
        </div>
      </Card.Body>
    </Card>
  );
};

export default TvShowCard;
