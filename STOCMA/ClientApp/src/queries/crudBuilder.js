import api from './api'
import buildQuery from 'odata-query'

const ODATA_URL = '/Odata/'

export const saveData = async (table, data, expand) => {
    const allParams = buildQuery({
        expand: expand?.join(',')
    })
    const URL = ODATA_URL + table + allParams

    try {
        const res = await api.post(URL, data, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        if (res.data) {
            return res.data;
        } else {
            return res;
        }
    } catch (e) {
        console.error(e);
    }
}

export const updateData = async (table, data, id, expand) => {
    const allParams = buildQuery({
        expand: expand?.join(',')
    })
    const URL = ODATA_URL + table + `(${id})` + allParams;

    try {
        const res = await api.put(URL, data, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });

        return res.status === 204;
    } catch (e) {
        console.error(e);
    }
}

export const partialUpdateData = async (table, data, id, expand) => {
    const allParams = buildQuery({
        expand: expand?.join(',')
    })
    const URL = ODATA_URL + table + `(${id})` + allParams;

    try {
        const res = await api.patch(URL, data, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        });
        return res.status === 204;
    } catch (e) {
        console.error(e);
    }
}

export const deleteData = async (table, id) => {
    const URL = ODATA_URL + table + `(${id})`

    try {
        const res = await (await api.delete(URL));
        return res.status === 204;
    } catch (e) {
        console.error(e);
    }
}

export const getData = async (table, params, filters, expand) => {
    const allParams = buildQuery({
        filter: filters,
        count: true,
        top: 10,
        skip: params?.$skip || 0,
        expand: expand?.join(',')
    })

    const URL = ODATA_URL + table + allParams;

    try {
        const res = (await api.get(URL))?.data;
        return { data: res?.value || [], totalItems: res?.['@odata.count'] };
    } catch (e) {
        console.log(e);
    }
}

export const getSingleData = async (table, id, expand) => {
    const allParams = buildQuery({
        expand: expand?.join(',')
    })

    const URL = ODATA_URL + table + `(${id})` + allParams;

    try {
        const res = (await api.get(URL))?.data;
        return res;
    } catch (e) {
        console.error(e);
    }
}

export const getAllData = async (table, filters, expand) => {
    const allParams = buildQuery({
        filter: filters,
        expand: expand?.join(',')
    })

    const URL = ODATA_URL + table + allParams;

    try {
        const res = (await api.get(URL)).data;
        return res.value;
    } catch (e) {
        console.error(e);
    }
}