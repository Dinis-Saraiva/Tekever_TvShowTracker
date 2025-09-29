import { useState, useEffect } from "react";
import { GET_TVSHOWS_PAGINATED } from "../queries";
import { graphql } from "../Enpoints/api";

export function useTvShows({ search, genre, pageSize = 10 }) {
  const [shows, setShows] = useState([]);
  const [pageInfo, setPageInfo] = useState(null);
  const [loading, setLoading] = useState(false);

  const fetchShows = async (afterCursor) => {
    setLoading(true);
    try {
      const variables = {
  first: pageSize,
  after: afterCursor,
  where: {
    ...(search ? { name: { contains: search } } : {}),
    ...(genre ? { genres: { some: { name: { eq: genre } } } } : {}),
  },
  order: [{ releaseDate: "DESC" }],
};


      const data = await graphql(GET_TVSHOWS_PAGINATED, variables);

      if (afterCursor) {
        // append results
        setShows((prev) => [...prev, ...data.tvShows.edges]);
      } else {
        // replace results
        setShows(data.tvShows.edges);
      }
      setPageInfo(data.tvShows.pageInfo);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // re-run when search/genre changes
  useEffect(() => {
    fetchShows(null);
  }, [search, genre]);

  return { shows, pageInfo, fetchShows, loading };
}
