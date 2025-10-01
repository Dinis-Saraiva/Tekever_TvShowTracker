import { api } from './api';

export const sendRecommendationsEmail = async () => {
  try {
    const response = await api.post(`/recommendations/send`);
    return { success: true, message: "Sent Recommendations by email" };
  } catch (error) {
    console.error("Error sending TV show recommendations", error);
    return { success: false, message: "Failed to send TV show recommendation" };
  }
};