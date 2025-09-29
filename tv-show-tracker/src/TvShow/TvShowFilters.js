import React from "react";
import { Form, Button, DropdownButton, Badge , InputGroup } from "react-bootstrap";

const GENRES = [
  "International TV Shows",
  "TV Dramas",
  "TV Sci-Fi & Fantasy",
  "TV Mysteries",
  "Crime TV Shows",
  "Docuseries",
  "Anime Series",
  "Reality TV",
  "TV Comedies",
  "Romantic TV Shows",
  "Science & Nature TV",
  "British TV Shows",
  "Korean TV Shows",
  "Kids' TV",
  "TV Action & Adventure",
  "Spanish-Language TV Shows",
  "TV Shows",
  "TV Horror",
  "Stand-Up Comedy & Talk Shows",
  "Teen TV Shows",
  "TV Thrillers",
  "Classic & Cult TV"
];

const RATING_ENUMS = {
  "TV-MA": "TV_MA",
  "TV-14": "TV_14",
  "TV-Y7": "TV_Y7",
  "TV-PG": "TV_PG",
  "TV-G": "TV_G",
  "TV-Y": "TV_Y",
};

function TvShowFilters({
  search,
  setSearch,
  searchTerm,
  setSearchTerm,
  loading,
  selectedGenres,
  setSelectedGenres,
  selectedRating,
  setSelectedRating,
  orderDirection,
  setOrderDirection,
}) {
  const handleSearchSubmit = (e) => {
    e.preventDefault();
    setSearchTerm(search);
  };

  const toggleGenre = (genre) => {
    setSelectedGenres((prev) =>
      prev.includes(genre) ? prev.filter((g) => g !== genre) : [...prev, genre]
    );
  };

  const selectRating = (rating) => {
    setSelectedRating((prev) => (prev === rating ? null : rating));
  };

  const removeGenre = (genre) => {
    setSelectedGenres((prev) => prev.filter((g) => g !== genre));
  };

  const clearRating = () => {
    setSelectedRating(null);
  };

  return (
    <>
      {/* Search + Filters */}
      <Form
        onSubmit={handleSearchSubmit}
        className="mb-3 d-flex gap-2 flex-wrap align-items-center"
      ><InputGroup>
        <Form.Control
          type="text"
          placeholder="Search TV Shows..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ flexGrow: 1 }}
        />
        <Button type="submit" variant="dark" disabled={loading}>
          Search
        </Button>
        </InputGroup>

        {/* Genres Dropdown */}
        <DropdownButton
          id="genre-dropdown"
          title="Genres"
          className="ms-2"
          variant="success"
        >
          <div style={{ maxHeight: "300px", overflowY: "auto", padding: "0.5rem" }}>
            {GENRES.map((genre) => (
              <Form.Check
                key={genre}
                type="checkbox"
                label={genre}
                checked={selectedGenres.includes(genre)}
                onChange={() => toggleGenre(genre)}
              />
            ))}
          </div>
        </DropdownButton>

        {/* Rating Dropdown */}
        <DropdownButton
          id="rating-dropdown"
          title={selectedRating || "Rating"}
          className="ms-2"
          variant="primary"
        >
          <div style={{ maxHeight: "200px", overflowY: "auto", padding: "0.5rem" }}>
            {Object.keys(RATING_ENUMS).map((label) => (
              <Form.Check
                key={label}
                type="radio"
                name="rating-options"
                label={label}
                checked={selectedRating === label}
                onChange={() => selectRating(label)}
              />
            ))}
          </div>
        </DropdownButton>

        {/* Order Dropdown */}
        <DropdownButton
          id="order-dropdown"
          title={orderDirection === "DESC" ? "Newest First" : "Oldest First"}
          className="ms-2"
          variant="warning"
        >
          <div style={{ padding: "0.5rem" }}>
            <Form.Check
              type="radio"
              name="order-options"
              label="Newest First"
              checked={orderDirection === "DESC"}
              onChange={() => setOrderDirection("DESC")}
            />
            <Form.Check
              type="radio"
              name="order-options"
              label="Oldest First"
              checked={orderDirection === "ASC"}
              onChange={() => setOrderDirection("ASC")}
            />
          </div>
        </DropdownButton>
      </Form>

      {/* Selected Filters */}
      <div className="mb-3">
        {selectedGenres.map((g) => (
          <Badge
            key={g}
            bg="success"
            className="me-1"
            style={{ cursor: "pointer" }}
            onClick={() => removeGenre(g)}
          >
            {g} ✕
          </Badge>
        ))}
        {selectedRating && (
          <Badge
            bg="primary"
            className="me-1"
            style={{ cursor: "pointer" }}
            onClick={clearRating}
          >
            {selectedRating} ✕
          </Badge>
        )}
      </div>
    </>
  );
}

export default TvShowFilters;
