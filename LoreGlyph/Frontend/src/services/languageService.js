import { api } from './axiosInstance'

export const languageService = {
  getAll: () => api.get('/Language'),

  create: (dto) => api.post('/Language', dto),

  update: (id, dto) => api.put(`/Language/${id}`, dto),

  delete: (id) => api.delete(`/Language/${id}`)
}