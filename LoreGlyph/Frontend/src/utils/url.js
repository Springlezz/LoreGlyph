const API_URL = import.meta.env.VITE_API_URL;

export const fileUrl = (path) => {
    if (!path) {
        return null;
    }

    const baseUrl = API_URL.replace(/\/api\/?$/, '');

    return `${baseUrl}${path}`;
};