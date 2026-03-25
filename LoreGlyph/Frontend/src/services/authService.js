import axios from "axios";

const API = "http://localhost:5248/api/Auth";

export const authService = {
  register: (dto) => axios.post(`${API}/register`, dto),
  login: (dto) => axios.post(`${API}/login`, dto),
};
