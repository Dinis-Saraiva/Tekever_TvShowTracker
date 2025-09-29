import { useState } from "react";
import { useTvShows } from "./Hook/TvShowHook";

export default function TvShowSearchPage() {
  const [search, setSearch] = useState("");
  const [genre, setGenre] = useState("");

  const { shows, pageInfo, fetchShows, loading } = useTvShows({ search, genre });

  return (
    <div className="p-4">
      <div className="mb-4">
        <input
          type="text"
          placeholder="Search TV Shows..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="border p-2 rounded mr-2"
        />

        <select
          value={genre}
          onChange={(e) => setGenre(e.target.value)}
          className="border p-2 rounded"
        >
          <option value="">All Genres</option>
          <option value="Drama">Drama</option>
          <option value="Comedy">Comedy</option>
          <option value="Sci-Fi">Sci-Fi</option>
          {/* optional: populate dynamically from API */}
        </select>
      </div>

      {loading && <p>Loading...</p>}

      <div>
        {shows.map(({ node }) => (
          <div key={node.id} className="p-4 border rounded mb-2">
            <img src={node.imageUrl} alt={node.name} width={150} />
            <h2>{node.name}</h2>
            <p>{node.description}</p>
            <p>Genres: {node.genres.map((g) => g.name).join(", ")}</p>
          </div>
        ))}
      </div>

      {pageInfo?.hasNextPage && (
        <button
          className="mt-4 p-2 bg-blue-500 text-white rounded"
          onClick={() => fetchShows(pageInfo.endCursor)}
        >
          Load More
        </button>
      )}
    </div>
  );
}
