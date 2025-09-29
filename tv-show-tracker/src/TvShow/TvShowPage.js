import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { graphql } from '../Enpoints/api';
import TvShowCard from './TvShowCard';
import { GET_TVSHOWS_PAGINATED } from '../queries';
import TvShowFilters from './TvShowFilters';

const PAGE_SIZE = 15;

const RATING_ENUMS = {
  "TV-MA": "TV_MA",
  "TV-14": "TV_14",
  "TV-Y7": "TV_Y7",
  "TV-PG": "TV_PG",
  "TV-G": "TV_G",
  "TV-Y": "TV_Y",
};

function TvShowPage() {
  const [tvShows, setTvShows] = useState([]);
  const [pageInfo, setPageInfo] = useState({ hasNextPage: false, hasPreviousPage: false, startCursor: null, endCursor: null });
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedGenres, setSelectedGenres] = useState([]);
  const [selectedRating, setSelectedRating] = useState(null);
  const [orderDirection, setOrderDirection] = useState("DESC");

  const fetchTvShows = async ({ after = null, before = null, first = PAGE_SIZE, last = null, searchFilter = '', genres = [], rating = null, order = "DESC" } = {}) => {
    setLoading(true);
    try {
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
          ...(rating ? { rating: { eq: RATING_ENUMS[rating] } } : {}),
        },
        order: [{ releaseDate: order }],
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
    fetchTvShows({
      first: PAGE_SIZE,
      searchFilter: searchTerm,
      genres: selectedGenres,
      rating: selectedRating,
      order: orderDirection,
    });
  }, [searchTerm, selectedGenres, selectedRating, orderDirection]);

  return (
    <Container className="mt-5">
      {/* Filters */}
      <TvShowFilters
        search={search}
        setSearch={setSearch}
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        loading={loading}
        selectedGenres={selectedGenres}
        setSelectedGenres={setSelectedGenres}
        selectedRating={selectedRating}
        setSelectedRating={setSelectedRating}
        orderDirection={orderDirection}
        setOrderDirection={setOrderDirection}
      />

      {/* TV Shows Grid */}
      <Row xs={1} md={2} lg={3} className="g-4">
        {tvShows.map(show => (
          <Col key={show.id}>
            <TvShowCard show={show} />
          </Col>
        ))}
      </Row>

      {/* Pagination */}
      <div className="d-flex justify-content-center mt-4 gap-2">
        <Button
          variant="dark"
          disabled={!pageInfo.hasPreviousPage || loading}
          onClick={() =>
            fetchTvShows({
              last: PAGE_SIZE,
              before: pageInfo.startCursor,
              first: null,
              after: null,
              searchFilter: searchTerm,
              genres: selectedGenres,
              rating: selectedRating,
              order: orderDirection,
            })
          }
        >
          Previous
        </Button>
        <Button
          variant="dark"
          disabled={!pageInfo.hasNextPage || loading}
          onClick={() =>
            fetchTvShows({
              first: PAGE_SIZE,
              after: pageInfo.endCursor,
              last: null,
              before: null,
              searchFilter: searchTerm,
              genres: selectedGenres,
              rating: selectedRating,
              order: orderDirection,
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
