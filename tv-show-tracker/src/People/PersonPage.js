import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { graphql } from '../Enpoints/api';
import { GET_PERSON_BY_PERSONID } from '../queries';
import { Card, Row, Col, Badge, Spinner, ListGroup, Button } from 'react-bootstrap';
import utils from '../Utils';

const PersonPage = () => {
  const { id } = useParams();
  const [person, setPerson] = useState(null);
  const [loading, setLoading] = useState(true);

  // Fetch person details
  useEffect(() => {
    const fetchPerson = async () => {
      try {
        const res = await graphql(GET_PERSON_BY_PERSONID, { id: Number(id) });
        setPerson(res.data.personById);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchPerson();
  }, [id]);



  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </div>
    );
  }

  if (!person) {
    return <p className="text-center mt-5">Person not found.</p>;
  }


  return (
    <div className="container mt-4">
      {/* Person Card */}
      <Card className="shadow-sm mb-4">
        <Row className="g-0">
          <Col md={4}>
            <Card.Img
              src={utils.getImageUrl(person.profileImageUrl)}
              alt={person.name}
              className="img-fluid rounded-start"
            />
          </Col>
          <Col md={8}>
            <Card.Body>
              <Card.Title>{person.name}</Card.Title>
              <Card.Text>{person.bio || 'No biography available.'}</Card.Text>

              <ListGroup variant="flush" className="mb-3">
                <ListGroup.Item>
                  <strong>Birth Date:</strong> {utils.formatDate(person.birthDate)}
                </ListGroup.Item>
              </ListGroup>
            </Card.Body>
          </Col>
        </Row>
      </Card>


      <h4>Worked On</h4>
      {person.workedOn && person.workedOn.length > 0 ? (
        <ListGroup variant="flush">
          {person.workedOn.map((work) => (
            <ListGroup.Item key={work.id}>
              <Row className="align-items-center">
                <Col md={4}>
                  <strong>Role:</strong> {work.role}
                </Col>
                <Col md={5}>
                  <Link to={`/show/${work.tvShow.id}`} style={{ textDecoration: "none" }}>
                    <Badge bg="info" className="p-2">
                      {work.tvShow.name}
                    </Badge>
                  </Link>
                </Col>
                <Col md={3} className="text-muted text-end">
                  {work.tvShow.releaseDate
                    ? new Date(work.tvShow.releaseDate).getFullYear()
                    : "N/A"}
                </Col>
              </Row>
            </ListGroup.Item>
          ))}
        </ListGroup>
      ) : (
        <p className="text-muted fst-italic">This person has no credited work.</p>
      )}
    </div>
  );
};

export default PersonPage;