import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Card, Row, Col, Badge, Spinner, ListGroup, Button } from 'react-bootstrap';
import Utils from '../Utils';
import { getTvShowByID } from '../Enpoints/TvShow';
import { graphql } from '../Enpoints/api';
import {GET_EPISODES_BY_TVSHOW_ID} from '../queries';
import FavoriteButton from './FavouriteButton';

const TvShowDetail = () => {
  const { id } = useParams();
  const [show, setShow] = useState(null);
  const [episodes, setEpisodes] = useState([]);
  const [loadingEpisodes, setLoadingEpisodes] = useState(true);
  const [pageInfo, setPageInfo] = useState({ hasNextPage: false, hasPreviousPage: false, startCursor: null, endCursor: null });
  const EPISODES_PER_PAGE = 20;

  useEffect(() => {
    const fetchShow = async () => {
      try {
        const res = await getTvShowByID(Number(id));
        if (res.success) {
          setShow(res.tvShow);
        } else {
          console.error(res.message);
        }
      } catch (err) {
        console.error(err);
      }
    };
    fetchShow();
    fetchEpisodes();
  }, [id]);

  // Episodes logic stays the same
  const fetchEpisodes = async ({ before = null, after = null, first = EPISODES_PER_PAGE, last = EPISODES_PER_PAGE } = {}) => {
    setLoadingEpisodes(true);
    try {

      const variables = { tvShowId: Number(id), first, after, last, before };
      const res = await graphql(GET_EPISODES_BY_TVSHOW_ID, variables);
      const edges = res.data.episodesByTvShowId.edges; 
      
      setEpisodes(edges.map(edge => edge.node));
      const info = res.data.episodesByTvShowId.pageInfo; 
      
      setPageInfo(info);

    } catch (err) {
      console.error(err);
    } finally {
      setLoadingEpisodes(false);
    }
  };


  if (!show) return (
    <div className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
      <Spinner animation="border" role="status">
        <span className="visually-hidden">Loading...</span>
      </Spinner>
    </div>
  );

  const episodesBySeason = episodes.reduce((acc, ep) => {
    if (!acc[ep.seasonNumber]) acc[ep.seasonNumber] = [];
    acc[ep.seasonNumber].push(ep);
    return acc;
  }, {});

  return (
    <div className="container mt-4">
      {/* TV Show Card */}
      <Card className="shadow-sm mb-4">
        <Row className="g-0">
          <Col md={4}>
            <Card.Img
              src={Utils.getImageUrl(show.imageUrl)}
              alt={show.name}
              className="img-fluid rounded-start"
            />
          </Col>
          <Col md={8}>
            <Card.Body>
              <Card.Title>{show.name}</Card.Title>
              <Card.Text>{show.description}</Card.Text>

              <ListGroup variant="flush" className="mb-3">
                <ListGroup.Item><strong>Release:</strong> {Utils.formatDate(show.releaseDate)}</ListGroup.Item>
                <ListGroup.Item><strong>Seasons:</strong> {show.seasons}</ListGroup.Item>
                <ListGroup.Item><strong>Rating:</strong> {show.rating}</ListGroup.Item>
                <ListGroup.Item><strong>Origin:</strong> {show.origin}</ListGroup.Item>
                <ListGroup.Item><strong>Favorite: </strong><FavoriteButton tvShowId={show.id} initialFavorite={show.isFavorite}/></ListGroup.Item>
              </ListGroup>

              <div className="mb-2">
                <strong>Genres: </strong>
                {show.genres?.map(genre => (
                  <Badge bg="secondary" key={genre.name} className="me-1">{genre.name}</Badge>
                ))}
              </div>

              <div className="mb-2">
                <strong>Actors: </strong>
                {show.cast?.map(actor => (
                  <Link to={`/person/${actor.id}`} key={actor.id} style={{ textDecoration: 'none' }}>
                    <Badge bg="info" className="me-1" style={{ cursor: 'pointer' }}>
                      {actor.name}
                    </Badge>
                  </Link>
                ))}
              </div>

              <div>
                <strong>Directors: </strong>
                {show.directors && show.directors.length > 0 ? (
                  show.directors.map(director => (
                    <Link to={`/person/${director.id}`} key={director.id} style={{ textDecoration: 'none' }}>
                      <Badge bg="warning" text="dark" className="me-1" style={{ cursor: 'pointer' }}>
                        {director.name}
                      </Badge>
                    </Link>
                  ))
                ) : (
                  <span>N/A</span>
                )}
              </div>

            </Card.Body>
          </Col>
        </Row>
      </Card>

      {/* Episodes Section stays unchanged */}
      <h4>Episodes</h4>
      {loadingEpisodes ? (
        <div className="d-flex justify-content-center my-4">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </div>
      ) : episodes.length === 0 ? (
        <p>No episodes found.</p>
      ) : (
        <>
          {Object.keys(episodesBySeason).map(season => (
            <div key={season}>
              <h5 className="mt-3 mb-2">Season {season}</h5>
              <ListGroup className="mb-3">
                {episodesBySeason[season].map(ep => (
                  <ListGroup.Item key={ep.id}>
                    <strong>Episode {ep.episodeNumber}:</strong> {ep.title}
                    <br />
                    <small>Air Date: {Utils.formatDate(ep.airDate)}</small>
                    {ep.summary && <p className="mb-0">{ep.summary}</p>}
                  </ListGroup.Item>
                ))}
              </ListGroup>
            </div>
          ))}

          {/* Pagination */}
          <div className="d-flex justify-content-center mb-4">
            <Button
              variant="secondary"
              className="me-2"
              disabled={!pageInfo.hasPreviousPage || loadingEpisodes}
              onClick={() => fetchEpisodes({ last: EPISODES_PER_PAGE, before: pageInfo.startCursor, first: null, after: null })}
            >
              Previous
            </Button>
            <Button
              variant="secondary"
              className="ms-2"
              disabled={!pageInfo.hasNextPage || loadingEpisodes}
              onClick={() => fetchEpisodes({ first: EPISODES_PER_PAGE, after: pageInfo.endCursor, last: null, before: null })}
            >
              Next
            </Button>
          </div>
        </>
      )}
    </div>
  );
};

export default TvShowDetail;
