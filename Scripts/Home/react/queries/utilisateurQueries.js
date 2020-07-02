const TABLE = 'ApplicationUsers';
const ODATA_URL = '/Users/'

export const updateUtilisateur = async (userData) => {
    const URL = ODATA_URL + 'UpdateUser';
    try {
        const res = await (await fetch(URL, {
            method: 'PUT',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        }));
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const updateUserPassword = async (userData) => {
    const URL = ODATA_URL + 'UpdatePassword';
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
        return res;
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
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const hasClaim = async (userData) => {
    const URL = ODATA_URL + 'HasClaim';
    try {
        const res = await (await fetch(URL, {
            method: 'POST',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        })).json();
        return res?.userHasClaim;
    } catch (e) {
        console.log(e);
    }
}

export const createUser = async (userData) => {
    const URL = ODATA_URL + 'CreateUser';
    try {
        const res = await (await fetch(URL, {
            method: 'POST',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        })).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const removeUser = async (userData) => {
    const URL = ODATA_URL + 'Removeuser';
    try {
        const res = await fetch(URL, {
            method: 'DELETE',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const getUserInfo = async () => {
    const URL = ODATA_URL + 'GetCurrentUserClaims';
    try {
        const res = await (await fetch(URL, {
            method: 'GET',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const deleteUtilisateur = async (id) => {
    const URL = ODATA_URL + 'ArticleSites' + `(IdArticle=${IdArticle},IdSite=${IdSite})`
    try {
        const res = await (await fetch(URL, {
            method: 'DELETE',
            cache: 'no-cache'
        }));
        return res;
    } catch (e) {
        console.error(e);
    }
}

export const saveArticle = async (article, qteStock, idSite) => {
    const parsedParams = new URLSearchParams({
        QteStock: qteStock,
        IdSite: idSite,
        $expand: 'ArticlesSites'
    }).toString();

    const URL = ODATA_URL + TABLE + '?' + parsedParams;
    const { QteStockSum, ...data } = article;
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