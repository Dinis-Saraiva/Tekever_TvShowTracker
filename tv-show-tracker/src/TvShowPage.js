import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Button, Form, Dropdown, DropdownButton, Badge } from 'react-bootstrap';
import { graphql } from './Enpoints/api';
import TvShowCard from './TvShowCard';
import { GET_TVSHOWS_PAGINATED } from './queries';

const PAGE_SIZE = 15;

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

function TvShowPage() {
  const [tvShows, setTvShows] = useState([]);
  const [pageInfo, setPageInfo] = useState({ hasNextPage: false, hasPreviousPage: false, startCursor: null, endCursor: null });
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedGenres, setSelectedGenres] = useState([]);

  const fetchTvShows = async ({ after = null, before = null, first = PAGE_SIZE, last = null, searchFilter = '', genres = [] } = {}) => {
    setLoading(true);
    try {
      // Build nested genre filter
      const genreFilters = genres.map(name => ({ name: { eq: name } }));

      const variables = {
        first,
        after,
        last,
        before,
        where: {
          ...(searchFilter ? { name: { contains: searchFilter } } : {}),
          ...(genres.length > 0
            ? { tvShowGenres: { some: { genre: { or: genreFilters } } } }
            : {}),
        },
        order: [{ releaseDate: "DESC" }],
      };



      const data = await graphql(GET_TVSHOWS_PAGINATED, variables);

      const edges = data.data.tvShows.edges;
      setTvShows(edges.map(e => e.node));
      setPageInfo(data.data.tvShows.pageInfo);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTvShows({ first: PAGE_SIZE, searchFilter: searchTerm, genres: selectedGenres });
  }, [searchTerm, selectedGenres]);

  const handleSearchSubmit = (e) => {
    e.preventDefault();
    setSearchTerm(search);
  };

  const toggleGenre = (genre) => {
    setSelectedGenres(prev =>
      prev.includes(genre) ? prev.filter(g => g !== genre) : [...prev, genre]
    );
  };

  return (
    <Container className="mt-5">
      {/* Search Form */}
      <Form onSubmit={handleSearchSubmit} className="mb-3 d-flex gap-2">
        <Form.Control
          type="text"
          placeholder="Search TV Shows..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <Button type="submit" disabled={loading}>Search</Button>

        {/* Genres Dropdown */}
        <DropdownButton id="genre-dropdown" title="Select Genres" className="ms-2">
          <div style={{ maxHeight: '300px', overflowY: 'auto', padding: '0.5rem' }}>
            {GENRES.map(genre => (
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
      </Form>

      {/* Display selected genres */}
      {selectedGenres.length > 0 && (
        <div className="mb-3">
          {selectedGenres.map(g => (
            <Badge key={g} bg="secondary" className="me-1">{g}</Badge>
          ))}
        </div>
      )}

      <Row xs={1} md={2} lg={3} className="g-4">
        {tvShows.map(show => (
          <Col key={show.id}>
            <TvShowCard show={show} />
          </Col>
        ))}
      </Row>

      <div className="d-flex justify-content-center mt-4 gap-2">
        <Button
          disabled={!pageInfo.hasPreviousPage || loading}
          onClick={() =>
            fetchTvShows({
              last: PAGE_SIZE,
              before: pageInfo.startCursor,
              first: null,
              after: null,
              searchFilter: searchTerm,
              genres: selectedGenres,
            })
          }
        >
          Previous
        </Button>
        <Button
          disabled={!pageInfo.hasNextPage || loading}
          onClick={() =>
            fetchTvShows({
              first: PAGE_SIZE,
              after: pageInfo.endCursor,
              last: null,
              before: null,
              searchFilter: searchTerm,
              genres: selectedGenres,
            })
          }
        >
          Next
        </Button>
      </div>
    </Container>
  );
}

export default TvShowPage;
