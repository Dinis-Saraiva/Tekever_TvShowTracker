import { gql } from 'graphql-request';

export const GET_TVSHOWS_PAGINATED = gql`
  query GetTvShows(
    $first: Int
    $after: String
    $last: Int
    $before: String
    $where: TvShowFilterInput
    $order: [TvShowSortInput!]
    
  ) {
    tvShows(first: $first, after: $after, last: $last, before: $before,where: $where,
      order: $order) {
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
  query GetTvShowById($id: Int!) {
    tvShowById(id: $id) {
      id
      name
      description
      releaseDate
      seasons
      rating
      imageUrl
      origin
      rating
      genres {
        name
      }
      actors { id name
        }
      directors { id name
        }
    }
  }
`;

export const GET_EPISODES_BY_TVSHOW_ID = gql`
  query GetEpisodesByTvShowId($tvShowId: Int!, $first: Int, $after: String, $last: Int, $before: String) {
    episodesByTvShowId(tvShowId: $tvShowId, first: $first, after: $after, last: $last, before: $before) {
      edges {
        node {
          id
          title
          airDate
          summary
          seasonNumber
          episodeNumber
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

export const GET_PERSON_BY_PERSONID = gql`
  query GetPersonById($id: Int!) {
    personById(id: $id) {
      id
      name
      bio
      birthDate
      profileImageUrl
      workedOn { id role
        tvShow { id name
        }
      }
    }
  }
`;
