import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { graphql } from './Enpoints/api';
import TvShowCard from './TvShowCard';
import { GET_TVSHOWS_PAGINATED } from './queries';

const PAGE_SIZE = 15;

function TvShowPage() {
  const [tvShows, setTvShows] = useState([]);
  const [pageInfo, setPageInfo] = useState({ hasNextPage: false, hasPreviousPage: false, startCursor: null, endCursor: null });
  const [loading, setLoading] = useState(false);

  const fetchTvShows = async ({ after = null, before = null, first = PAGE_SIZE, last = PAGE_SIZE } = {}) => {
  setLoading(true);
  try {
    const variables = { first, after, last, before };
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
    fetchTvShows();
  }, []);

  return (
    <Container className="mt-5">
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
          onClick={() => fetchTvShows({ last: PAGE_SIZE, before: pageInfo.startCursor, first: null , after: null })}
        >
          Previous
        </Button>
        <Button
          disabled={!pageInfo.hasNextPage || loading}
          onClick={() => fetchTvShows({ first: PAGE_SIZE, after: pageInfo.endCursor, last: null, before: null })}
        >
          Next
        </Button>

      </div>
    </Container>
  );
}

export default TvShowPage;
