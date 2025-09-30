import { api } from './api';


export const getFavoriteTvShows = async (pageNumber = 1, pageSize = 10, token) => {

  try {

    const response = await api.get(`/favorites?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return { success: true, data: response.data };
  } catch (error) {
    console.error("Error fetching favorite TV shows:", error);
    return { success: false, message:"Failed to fetch favorite TV shows" };
  }
};

export const addFavoriteTvShow = async (tvShowId) => {
  try {
    const response = await api.post(`/favorites?tvShowIds=${tvShowId}`);
    return { success: true, data: response.data };
  } catch (error) {
    console.error("Error adding favorite TV show:", error);
    return { success: false, message: "Failed to add favorite TV show" };
  }
};

export const removeFavoriteTvShow = async (tvShowId) => {
  try {
    const response = await api.delete(`/favorites?tvShowId=${tvShowId}`);
    return { success: true, data: response.data };
  } catch (error) {
    console.error("Error removing favorite TV show:", error);
    return { success: false, message: "Failed to remove favorite TV show" };
  }};
