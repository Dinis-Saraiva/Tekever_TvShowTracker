import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { request } from 'graphql-request';
import TvShowCard from './TvShowCard';
import { GET_TVSHOWS_PAGINATED } from './queries';

const endpoint = 'https://localhost:7211/graphql';
const PAGE_SIZE = 15;

function TvShowPage() {
  const [tvShows, setTvShows] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [pageInfo, setPageInfo] = useState({ hasNextPage: false, hasPreviousPage: false, startCursor: null, endCursor: null });
  const [loading, setLoading] = useState(false);

  const fetchTvShows = async (afterCursor = null) => {
    setLoading(true);
    try {
      const data = await request(endpoint, GET_TVSHOWS_PAGINATED, {
        first: PAGE_SIZE,
        after: afterCursor,
      });
      const edges = data.tvShows.edges;
      setTvShows(edges.map(e => e.node));
      setTotalCount(data.tvShows.totalCount);
      setPageInfo(data.tvShows.pageInfo);
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
        <Button disabled={!pageInfo.hasPreviousPage || loading} onClick={() => fetchTvShows(pageInfo.startCursor)}>
          Previous
        </Button>
        <Button disabled={!pageInfo.hasNextPage || loading} onClick={() => fetchTvShows(pageInfo.endCursor)}>
          Next
        </Button>
      </div>
    </Container>
  );
}

export default TvShowPage;
