import React, { useState, useContext } from "react";
import { Button, Spinner, Modal } from "react-bootstrap";
import { addFavoriteTvShow, removeFavoriteTvShow } from "../Enpoints/favorites";
import { UserContext } from "../UserManagement/UserContext";

const FavoriteButton = ({ tvShowId, initialFavorite }) => {
  const { user } = useContext(UserContext);
  const [isFavorite, setIsFavorite] = useState(initialFavorite);
  const [loading, setLoading] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);

  const toggleFavorite = async () => {
    if (!user) {
      setShowLoginModal(true);
      return;
    }

    setLoading(true);
    try {
      if (isFavorite) {
        const res = await removeFavoriteTvShow(tvShowId);
        if (res.success) setIsFavorite(false);
      } else {
        const res = await addFavoriteTvShow(tvShowId);
        if (res.success) setIsFavorite(true);
      }
    } catch (err) {
      console.error("Error toggling favorite:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button
        variant={isFavorite ? "danger" : "outline-primary"}
        onClick={toggleFavorite}
        disabled={loading}
        className="rounded-pill shadow-sm px-4 py-2 fw-semibold"
        style={{ transition: "all 0.3s ease" }}
      >
        {loading ? (
          <Spinner as="span" animation="border" size="sm" />
        ) : isFavorite ? (
          "Favorited"
        ) : (
          "Add to Favorites"
        )}
      </Button>

      <Modal show={showLoginModal} onHide={() => setShowLoginModal(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Login Required</Modal.Title>
        </Modal.Header>
        <Modal.Body>You must be logged in to add favorites.</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowLoginModal(false)}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};

export default FavoriteButton;
