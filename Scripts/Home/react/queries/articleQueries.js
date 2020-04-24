const TABLE = 'Articles';
const ODATA_URL = '/Odata/'

export const getArticles = async (field, text) => {
    let filterText = '';
    if (!text)
        return [];

    filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1 and Ref ne \'-\'';

    const URL = '/Odata/' + TABLE + '?&$top=20' + filterText;

    try {
        const res = await (await fetch(URL)).json();
        return res?.value
    } catch (e) {
        console.log(e);
        return [];
    }
}

export const getLastPriceSale = async (articleId, clientId) => {
    const parsedParams = new URLSearchParams({
        IdClient: clientId,
        IdArticle: articleId
    }).toString();

    const URL = '/SalesHistory/getPriceLastSale?' + parsedParams;

    try {
        const res = await (await fetch(URL, {})).json();
        return res;
    } catch (e) {
        console.log(e);
        return 0;
    }
}

export const updateArticle = async (article, id, qteStock, idSite) => {
    const parsedParams = new URLSearchParams({
        QteStock: qteStock,
        IdSite: idSite
    }).toString();

    const URL = ODATA_URL + TABLE + `(guid'${id}')` + '?' + parsedParams;
    delete article['QteStockSum'];
    delete article['ArticleSites'];
    try {
        const res = await (await fetch(URL, {
            method: 'PUT',
            cache: 'no-cache',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(article)
        }));
        return res;
    } catch (e) {
        console.log(e);
    }
}

export const saveArticle = async (article, qteStock, idSite) => {
    const parsedParams = new URLSearchParams({
        QteStock: qteStock,
        IdSite: idSite,
        $expand: 'ArticlesSites'
    }).toString();

    const URL = ODATA_URL + TABLE + '?'+ parsedParams;
    const {QteStockSum, ...data} = article;
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