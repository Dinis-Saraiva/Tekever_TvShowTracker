import { gql } from 'graphql-request';

export const GET_TVSHOWS_PAGINATED = gql`
  query GetTvShows(
    $first: Int
    $after: String
    $last: Int
    $before: String
  ) {
    tvShows(first: $first, after: $after, last: $last, before: $before) {
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
          genres {
            name
          }
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

export const GET_TVSHOW_BY_ID = gql`
  query GetTvShows($id: ID!) {
    tvShows(id: $id) {
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
      genres {
        name
      }
    }
}}}
`;
