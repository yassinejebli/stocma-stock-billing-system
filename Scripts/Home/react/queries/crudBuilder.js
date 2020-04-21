const ODATA_URL = '/Odata/'

export const saveData = async (table, data, expand) => {
    const URL = ODATA_URL + table + (expand?`?$expand=${expand}`:'')

    try {
        const res = await (await fetch(URL, {
            method: 'POST',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const updateData = async (table, data, id, expand) => {
    const URL = ODATA_URL + table + `(guid'${id}')` + (expand?`?$expand=${expand}`:'')

    try {
        const res = await (await fetch(URL, {
            method: 'PUT',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }));
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const deleteData = async (table, id) => {
    const URL = ODATA_URL + table + `(guid'${id}')`

    try {
        const res = await (await fetch(URL, {
            method: 'DELETE',
            cache: 'no-cache'
        }));
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const getData = async (table, params, filters, expand) => {
    const allParams = {
        $inlinecount: 'allpages',
        $top: 10,
        $skip: 0,
        ...params
    }

    const nonEmptyFilters = filters?.filter(x=>x.value);
    if(nonEmptyFilters?.length > 0){
        allParams['$filter'] = nonEmptyFilters.filter(x=>x.value)
        .map(x=> 'indexof(' + x.field + ',\'' + x.value + '\') gt -1').join(' or ');
    }

    if(expand){
        allParams['$expand']= expand?.join(',');
    }

    const parsedParams = '?'+new URLSearchParams(allParams).toString();
    const URL = ODATA_URL + table + parsedParams;

    try {
        const res = await (await fetch(URL)).json();
        return {data: res?.value||[],totalItems: res?.['odata.count']};
    } catch (e) {
        console.log(e);
    }
}

export const getSingleData = async (table, id,expand) => {
    const params = {};
    if(expand){
        params['$expand']= expand?.join(',');
    }

    const parsedParams = '?'+new URLSearchParams(params).toString();
    const URL = ODATA_URL + table + `(guid'${id}')` + parsedParams;

    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}