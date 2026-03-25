import axios from "axios";

const API = "http://localhost:5248/api/User";

export const userService = {
  resetPassword: (dto) => axios.post(`${API}/reset-forgotten-password`, dto),
};
