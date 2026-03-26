import api from "./axiosInstance";

export const authService = {
  register: (dto) => api.post("/Auth/register", dto),

  async login(dto) {
    const response = await api.post("/Auth/login", dto);

    const data = response.data;

    localStorage.setItem("token", data.token);
    localStorage.setItem("userName", data.userName);
    localStorage.setItem("login", data.login);

    return data;
  },
  resetPassword: (dto) => api.post("/Auth/reset-password", dto)
};