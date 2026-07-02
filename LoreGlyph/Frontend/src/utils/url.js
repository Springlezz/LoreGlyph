const API_URL = import.meta.env.VITE_API_URL;

export const fileUrl = (path) => {
  if (!path) {
    return null;
  }
  return `${API_URL}${path}`;
};
