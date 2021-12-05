import axios from 'axios'
import authService from '../components/api-authorization/AuthorizeService';

const api = axios.create({
    baseURL: `/`
});

api.interceptors.request.use(async function (config) {
    const token = await authService.getAccessToken();
    config.headers.Authorization = 'Bearer ' + token;

    return config;
});

api.defaults.maxRedirects = 0;

export default api;
