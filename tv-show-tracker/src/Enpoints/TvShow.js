import {api} from './api'; 

export const getTvShowByID = async (tvShowId) => {
  try {
    const response = await api.get('/TvShow/GetTvShowByID', {
      params: { tvShowId },
    });
    return { success: true, tvShow: response.data };
  } catch (error) {
    console.error('Get TV show error:', error);
    return { 
      success: false, 
      message: error.response?.data?.message || 'Failed to fetch TV show' 
    };
  }
};
