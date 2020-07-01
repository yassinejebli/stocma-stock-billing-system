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