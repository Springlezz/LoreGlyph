import api from "./axiosInstance";

export const wordService = {
  getAll(languageId) {
    return api.get(`/word/${languageId}`);
  },
  create(languageId, wordData) {
    return api.post(`/word/${languageId}`, wordData);
  },
  update(wordId, wordData) {
    return api.put(`/word/${wordId}`, wordData);
  },
  delete(wordId) {
    return api.delete(`/word/${wordId}`);
  },
  updateOrder(languageId, orderedWords) {
    return api.put(
      `/word/update-word-order?languageId=${languageId}`,
      orderedWords,
    );
  },
};
