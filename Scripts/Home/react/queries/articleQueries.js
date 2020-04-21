const TABLE = 'Articles';

export const getArticles = async (field, text) => {
    let filterText = '';
    if(!text)
        return [];

    filterText = '&$filter=indexof(' + field + ',\'' + text + '\') gt -1 and Ref ne \'-\'';
  
    const URL = '/Odata/' + TABLE + '?&$top=20' + filterText;

    try {
        const res = await ( await fetch(URL) ).json();
        return res?.value
      } catch(e) { 
          console.log(e); 
          return [];
    }
}

export const getLastPriceSale = async (articleId, clientId) => {
    const parsedParams = new URLSearchParams({
        IdClient: clientId,
        IdArticle: articleId
    }).toString();

    const URL = '/SalesHistory/getPriceLastSale?'+parsedParams;

    try {
        const res = await ( await fetch(URL, {}) ).json();
        return res;
      } catch(e) { 
          console.log(e); 
          return 0;
    }
}