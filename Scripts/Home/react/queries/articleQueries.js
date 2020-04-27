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

export const updateArticle = async (article, id, qteStock, idSite, disabled) => {
    const parsedParams = new URLSearchParams({
        QteStock: qteStock,
        IdSite: idSite,
        Disabled: disabled
    }).toString();

    const URL = ODATA_URL + TABLE + `(${id})` + '?' + parsedParams;
    delete article['QteStockSum'];
    delete article['ArticleSites'];
    try {
        const res = await (await fetch(URL, {
            method: 'PATCH',
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

export const deleteArticle = async (IdSite, IdArticle) => {
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

export const getLowStockCount = async (IdSite) => {
    const URL = '/Statistics/LowStockArticles?IdSite='+IdSite;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}

export const getTotalStock = async (IdSite) => {
    const URL = '/Statistics/TotalStock?IdSite='+IdSite;
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}

export const getMarginArticles = async (IdSite) => {
    const URL = '/Statistics/ArticlesWithMargin?IdSite='+IdSite
    try {
        const res = await (await fetch(URL)).json();
        console.log({res})
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}