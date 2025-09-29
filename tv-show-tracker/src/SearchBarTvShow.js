import { useState } from "react";
import { Form, Button, Row, Col } from "react-bootstrap";

export default function TvShowSearchBar() {
  const [search, setSearch] = useState("");
  const [genre, setGenre] = useState("");
  const [rating, setRating] = useState("");
  const [year, setYear] = useState("");

  const handleSearch = () => {
    console.log("Search clicked", { search, genre, rating, year });
  };

  return (
    <Form className="p-4 border rounded shadow-sm bg-light">
      <Row className="align-items-end g-3">
        <Col xs={12} md={4}>
          <Form.Group controlId="search">
            <Form.Label>Search</Form.Label>
            <Form.Control
              type="text"
              placeholder="Search TV Shows..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </Form.Group>
        </Col>

        <Col xs={12} md={2}>
          <Form.Group controlId="genre">
            <Form.Label>Genre</Form.Label>
            <Form.Select
              value={genre}
              onChange={(e) => setGenre(e.target.value)}
            >
              <option value="">All Genres</option>
              <option value="Drama">Drama</option>
              <option value="Comedy">Comedy</option>
              <option value="Sci-Fi">Sci-Fi</option>
            </Form.Select>
          </Form.Group>
        </Col>

        <Col xs={12} md={2}>
          <Form.Group controlId="rating">
            <Form.Label>Rating</Form.Label>
            <Form.Select
              value={rating}
              onChange={(e) => setRating(e.target.value)}
            >
              <option value="">Any</option>
              <option value="G">G</option>
              <option value="PG">PG</option>
              <option value="PG-13">PG-13</option>
              <option value="R">R</option>
            </Form.Select>
          </Form.Group>
        </Col>

        <Col xs={12} md={2}>
          <Form.Group controlId="year">
            <Form.Label>Year</Form.Label>
            <Form.Control
              type="number"
              placeholder="e.g. 2023"
              value={year}
              onChange={(e) => setYear(e.target.value)}
            />
          </Form.Group>
        </Col>

        <Col xs={12} md={2}>
          <Button
            variant="primary"
            className="w-100"
            onClick={handleSearch}
          >
            Search
          </Button>
        </Col>
      </Row>
    </Form>
  );
}
