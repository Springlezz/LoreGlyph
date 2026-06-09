import api from "./axiosInstance";

export const languageService = {
  getAll: () => api.get("/Language"),
  create: (dto) => api.post("/Language", dto),
  update: (id, dto) => api.put(`/Language/${id}`, dto),
  delete: (id) => api.delete(`/Language/${id}`),

  getShareInfo: (id) => api.get(`/Language/${id}/share`),

  share: (id) => api.post(`/Language/${id}/share`),

  unshare: (id) => api.delete(`/Language/${id}/share`),
};