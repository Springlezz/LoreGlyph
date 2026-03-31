import api from "./axiosInstance";

export const userService = {
  resetPassword: (dto) => api.post("/User/reset-forgotten-password", dto),
  deleteAccount: (userId) => api.delete(`/User/${userId}`),
  getMe: () => api.get("/User/me"),
};
