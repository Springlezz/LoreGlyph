import api  from "./axiosInstance";

const API = "http://localhost:5248/api/User";

export const userService = {
  resetPassword: (dto) => api.post("/User/reset-forgotten-password", dto),
  getMe: () => api.get("/User/me"),
};
