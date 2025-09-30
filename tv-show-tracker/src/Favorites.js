import { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { Row, Col, Spinner, Pagination, Alert, Modal, Button } from "react-bootstrap";
import { getFavoriteTvShows } from "./Enpoints/favorites";
import TvShowCard from "./TvShow/TvShowCard";
import { UserContext } from "./UserManagement/UserContext"; 

const FavoriteTvShows = () => {
  const { user } = useContext(UserContext);
  const [tvShows, setTvShows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [error, setError] = useState(null);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) {
      setShowLoginModal(true);
      return;
    }

    const fetchTvShows = async () => {
      setLoading(true);
      setError(null);

      const result = await getFavoriteTvShows(pageNumber, 10);

      if (result.success) {
        setTvShows(result.data.items.map((fav) => fav.tvShow));
        setTotalPages(result.data.totalPages);
      } else {
        setError(result.message);
      }

      setLoading(false);
    };

    fetchTvShows();
  }, [pageNumber, user]);

if (!user) {
  return (
    <Modal show={showLoginModal} onHide={() => navigate("/")} centered>
      <Modal.Header closeButton>
        <Modal.Title>Login Required</Modal.Title>
      </Modal.Header>
      <Modal.Body>You must be logged in to view your favorite TV shows.</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={() => navigate("/")}>
          Close
        </Button>
        <Button variant="primary" onClick={() => navigate("/login")}>
          Go to Login
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

  if (loading) return <Spinner animation="border" />;

  if (error) return <Alert variant="danger">{error}</Alert>;

  if (tvShows.length === 0) {
    return <Alert variant="info">No favorites yet.</Alert>;
  }

  return (
    <div>
      <Row xs={1} md={2} lg={3} className="g-4">
        {tvShows.map((show) => (
          <Col key={show.id}>
            <TvShowCard show={show} />
          </Col>
        ))}
      </Row>

      {totalPages > 1 && (
        <Pagination className="mt-4">
          <Pagination.Prev
            onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
            disabled={pageNumber === 1}
          />
          {[...Array(totalPages)].map((_, idx) => (
            <Pagination.Item
              key={idx + 1}
              active={idx + 1 === pageNumber}
              onClick={() => setPageNumber(idx + 1)}
            >
              {idx + 1}
            </Pagination.Item>
          ))}
          <Pagination.Next
            onClick={() => setPageNumber((prev) => Math.min(prev + 1, totalPages))}
            disabled={pageNumber === totalPages}
          />
        </Pagination>
      )}
    </div>
  );

};

export default FavoriteTvShows;
