import { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { Row, Col, Spinner, Alert, Modal, Button } from "react-bootstrap";
import { getFavoriteTvShows } from "./Enpoints/favorites";
import TvShowCard from "./TvShow/TvShowCard";
import { UserContext } from "./UserManagement/UserContext";
import { sendRecommendationsEmail } from "./Enpoints/recomendations";

const FavoriteTvShows = () => {
  const { user } = useContext(UserContext);
  const [tvShows, setTvShows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [error, setError] = useState(null);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [emailLoading, setEmailLoading] = useState(false);
  const [emailSuccess, setEmailSuccess] = useState(null);
  const [emailError, setEmailError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) {
      setShowLoginModal(true);
      return;
    }

    const fetchTvShows = async () => {
      setLoading(true);
      setError(null);

      const result = await getFavoriteTvShows(pageNumber, 15);

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

  const handleSendEmail = async () => {
    setEmailLoading(true);
    setEmailSuccess(null);
    setEmailError(null);

    try {
      const result = await sendRecommendationsEmail(); // Your API call
      if (result.success) {
        setEmailSuccess("Recommendations sent successfully!");
      } else {
        setEmailError(result.message || "Failed to send recommendations.");
      }
    } catch (err) {
      setEmailError("An error occurred while sending email.");
    } finally {
      setEmailLoading(false);
    }
  };

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
      {/* Send Email Button */}
      <div className="d-flex justify-content-end mb-3">
        <Button 
          variant="success" 
          onClick={handleSendEmail} 
          disabled={emailLoading}
        >
          {emailLoading ? (
            <>
              <Spinner 
                as="span" 
                animation="border" 
                size="sm" 
                role="status" 
                aria-hidden="true" 
              /> Sending...
            </>
          ) : (
            "Send Recommendations by Email"
          )}
        </Button>
      </div>

      {emailSuccess && <Alert variant="success">{emailSuccess}</Alert>}
      {emailError && <Alert variant="danger">{emailError}</Alert>}

      <Row xs={1} md={2} lg={3} className="g-4">
        {tvShows.map((show) => (
          <Col key={show.id}>
            <TvShowCard show={show} />
          </Col>
        ))}
      </Row>

      <div className="d-flex justify-content-center mt-4 gap-2">
        <Button
          variant="dark"
          disabled={pageNumber === 1}
          onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
        >
          Previous
        </Button>
        <Button
          variant="dark"
          disabled={pageNumber === totalPages}
          onClick={() => setPageNumber((prev) => Math.min(prev + 1, totalPages))}
        >
          Next
        </Button>
      </div>
    </div>
  );
};

export default FavoriteTvShows;
