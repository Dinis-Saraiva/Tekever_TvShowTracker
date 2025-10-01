import { api } from './api';

export const getTvShowByID = async (tvShowId) => {
  try {
    const response = await api.get(`/TvShow/GetTvShowByID/${tvShowId}`, {
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

export const exportTvShowPdf = async (tvShowId) => {
  try {
    const response = await api.get(`/TvShow/${tvShowId}/export-pdf`, {
      responseType: 'blob',
    });
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `tvshow_${tvShowId}.pdf`);
    document.body.appendChild(link);
    link.click();
    link.remove();
    return { success: true };
  } catch (error) {
    console.error('Export PDF error:', error);
    return {
      success: false,
      message: error.response?.data?.message || 'Failed to export PDF'
    };
  }
};