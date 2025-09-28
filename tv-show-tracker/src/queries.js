import { gql } from 'graphql-request';

export const GET_TVSHOWS_PAGINATED = gql`
  query GetTvShows($first: Int!, $after: String) {
  tvShows(first: $first, after: $after) {
    totalCount
    edges {
      node {
        id
        name
        description
        releaseDate
        seasons
        rating
        imageUrl
        origin
        genres (first:4) {edges {
            node { name }
          }}
      }
      cursor
    }
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
  }
}

`;
