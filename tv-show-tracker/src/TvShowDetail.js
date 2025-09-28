import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import {graphql} from './Enpoints/api'; 
import { GET_TVSHOW_BY_ID } from './queries';

const TvShowDetail = () => {
  const { id } = useParams();
  const [show, setShow] = useState(null);

  useEffect(() => {
    const fetchShow = async () => {
      try {
        const res = await graphql(GET_TVSHOW_BY_ID,{id}); // adjust endpoint
        console.log(res);
        setShow(res.data);
      } catch (err) {
        console.error(err);
      }
    };

    fetchShow();
  }, [id]);

  if (!show) return <div>Loading...</div>;

  return (
    <div className="container mt-4">
      <h2>{show.name}</h2>
      <img src={show.imageUrl} alt={show.name} style={{ maxWidth: '300px' }} />
      <p>{show.description}</p>
      <ul>
        <li>Release: {show.releaseDate}</li>
        <li>Seasons: {show.seasons}</li>
        <li>Rating: {show.rating}</li>
        <li>Origin: {show.origin}</li>
      </ul>
      <div>
        {show.genres?.map((genre) => (
          <span key={genre.name} className="badge bg-secondary me-1">
            {genre.name}
          </span>
        ))}
      </div>
    </div>
  );
};

export default TvShowDetail;
