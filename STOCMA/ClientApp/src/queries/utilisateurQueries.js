import api from './api'

const ODATA_URL = '/Users/'

export const updateUtilisateur = async (userData) => {
    const URL = ODATA_URL + 'UpdateUser';
    try {
        const res = await api.put(URL, userData, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.status === 200;
    } catch (e) {
        console.log(e);
    }
}

export const updateUserPassword = async (userData) => {
    const URL = ODATA_URL + 'UpdatePassword';
    try {
        const res = await api.post(URL, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.status === 200;
    } catch (e) {
        console.log(e);
    }
}


export const setClaim = async (userData) => {
    const URL = ODATA_URL + 'SetClaim';
    try {
        const res = await (await fetch(URL, {
            method: 'POST',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        }));
        return res.data;
    } catch (e) {
        console.log(e);
    }
}

export const hasClaim = async (userData) => {
    const URL = ODATA_URL + 'HasClaim';
    try {
        const res = await (await api.post(URL, userData, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        }));
        return res?.data?.userHasClaim;
    } catch (e) {
        console.log(e);
    }
}

export const createUser = async (userData) => {
    const URL = ODATA_URL + 'CreateUser';
    try {
        const res = await api.post(URL, userData, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.data;
    } catch (e) {
        console.log(e);
    }
}

export const removeUser = async (userData) => {
    const URL = ODATA_URL + 'Removeuser';
    try {
        const res = await api.delete(URL, userData, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const getUserInfo = async () => {
    const URL = ODATA_URL + 'GetCurrentUserClaims';
    try {
        const res = await api.get(URL, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.data;
    } catch (e) {
        console.log(e);
    }
}

export const getUsers = async () => {
    const URL = ODATA_URL + 'GetUsers';
    try {
        const res = await api.get(URL, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.data;
    } catch (e) {
        console.log(e);
    }
}