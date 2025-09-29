import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Button, Form, InputGroup } from 'react-bootstrap';
import { graphql } from '../Enpoints/api';
import { GET_PEOPLE_PAGINATED } from '../queries';
import PersonCard from './PersonCard';

const PAGE_SIZE = 15;

function PeoplePage() {
  const [people, setPeople] = useState([]);
  const [pageInfo, setPageInfo] = useState({
    hasNextPage: false,
    hasPreviousPage: false,
    startCursor: null,
    endCursor: null,
  });
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  const fetchPeople = async ({
    after = null,
    before = null,
    first = PAGE_SIZE,
    last = null,
    searchFilter = '',
  } = {}) => {
    setLoading(true);
    try {
      const variables = {
        first,
        after,
        last,
        before,
        where: searchFilter ? { name: { contains: searchFilter } } : {},
        order: [{ name: "ASC" }], // order people alphabetically
      };

      const data = await graphql(GET_PEOPLE_PAGINATED, variables);
      const edges = data.data.people.edges;

      setPeople(edges.map((e) => e.node));
      setPageInfo(data.data.people.pageInfo);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPeople({
      first: PAGE_SIZE,
      searchFilter: searchTerm,
    });
  }, [searchTerm]);

  return (
    <Container className="mt-5">
      {/* Search Bar */}
      <div className="mb-4">
        <InputGroup>
          <Form.Control
            type="text"
            placeholder="Search people..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
          <Button
            variant="dark"
            disabled={loading}
            onClick={() => setSearchTerm(search)}
          >
            Search
          </Button>
        </InputGroup>
      </div>

      {/* People Grid */}
      <Row xs={1} md={2} lg={3} className="g-4">
        {people.map((person) => (
          <Col key={person.id}>
            <PersonCard person={person} />
          </Col>
        ))}
      </Row>

      {/* Pagination */}
      <div className="d-flex justify-content-center mt-4 gap-2">
        <Button
          variant="dark"
          disabled={!pageInfo.hasPreviousPage || loading}
          onClick={() =>
            fetchPeople({
              last: PAGE_SIZE,
              before: pageInfo.startCursor,
              first: null,
              after: null,
              searchFilter: searchTerm,
            })
          }
        >
          Previous
        </Button>
        <Button
          variant="dark"
          disabled={!pageInfo.hasNextPage || loading}
          onClick={() =>
            fetchPeople({
              first: PAGE_SIZE,
              after: pageInfo.endCursor,
              last: null,
              before: null,
              searchFilter: searchTerm,
            })
          }
        >
          Next
        </Button>
      </div>
    </Container>
  );
}

export default PeoplePage;
