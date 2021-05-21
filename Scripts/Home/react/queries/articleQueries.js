import buildQuery from "odata-query";

const TABLE = "Articles";
const ODATA_URL = "/Odata/";

export const getArticles = async (filters) => {
  // if (!filters.and[2]?.or) return [];
  const allParams = buildQuery({
    expand: "Article/Categorie",
    filter: filters,
    top: 20,
    skip: 0,
  });

  const URL = "/Odata/" + "ArticleSites" + allParams;

  try {
    const res = await (await fetch(URL)).json();
    return res?.value;
  } catch (e) {
    console.error(e);
    return [];
  }
};

export const getFakeArticles = async (filters) => {
  console.log({ filters });
  if (!filters.and) return [];

  const allParams = buildQuery({
    filter: filters,
    top: 20,
    skip: 0,
  });

  const URL = "/Odata/" + "ArticleFactures" + allParams;

  try {
    const res = await (await fetch(URL)).json();
    console.log({ res });
    return res?.value;
  } catch (e) {
    console.log(e);
    return [];
  }
};

export const getLastPriceSale = async (articleId, clientId) => {
  const parsedParams = new URLSearchParams({
    IdClient: clientId,
    IdArticle: articleId,
  }).toString();

  const URL = "/SalesHistory/getPriceLastSale?" + parsedParams;

  try {
    const res = await (await fetch(URL, {})).json();
    return res;
  } catch (e) {
    console.log(e);
    return 0;
  }
};

export const getArticleByBarCode = async (barCode, clientId, siteId) => {
  const parsedParams = new URLSearchParams({
    IdClient: clientId,
    BarCode: barCode,
    IdSite: siteId,
  }).toString();

  const URL = "/SalesHistory/GetArticleByBarCode?" + parsedParams;

  try {
    const res = await (await fetch(URL, {})).json();
    return res;
  } catch (e) {
    console.log(e);
    return 0;
  }
};

export const getArticleAchatByBarCode = async (
  barCode,
  fournisseurId,
  siteId
) => {
  const parsedParams = new URLSearchParams({
    IdFournisseur: fournisseurId,
    BarCode: barCode,
    IdSite: siteId,
  }).toString();

  const URL = "/SalesHistory/GetArticleAchatByBarCode?" + parsedParams;

  try {
    const res = await (await fetch(URL, {})).json();
    return res;
  } catch (e) {
    console.log(e);
    return 0;
  }
};

export const getLastPricePurchase = async (articleId, fournisseurId) => {
  const parsedParams = new URLSearchParams({
    IdFournisseur: fournisseurId,
    IdArticle: articleId,
  }).toString();

  const URL = "/SalesHistory/getPriceLastPurchase?" + parsedParams;

  try {
    const res = await (await fetch(URL, {})).json();
    return res;
  } catch (e) {
    console.log(e);
    return 0;
  }
};

export const updateArticle = async (
  article,
  id,
  qteStock,
  idSite,
  disabled
) => {
  const parsedParams = new URLSearchParams({
    QteStock: qteStock,
    IdSite: idSite,
    Disabled: disabled,
  }).toString();

  const URL = ODATA_URL + TABLE + `(${id})` + "?" + parsedParams;
  delete article["QteStockSum"];
  delete article["ArticleSites"];
  try {
    const res = await await fetch(URL, {
      method: "PATCH",
      cache: "no-cache",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(article),
    });
    return res;
  } catch (e) {
    console.log(e);
  }
};

export const deleteArticle = async (IdSite, IdArticle) => {
  const URL =
    ODATA_URL + "ArticleSites" + `(IdArticle=${IdArticle},IdSite=${IdSite})`;
  try {
    const res = await await fetch(URL, {
      method: "DELETE",
      cache: "no-cache",
    });
    return res;
  } catch (e) {
    console.error(e);
  }
};

export const saveArticle = async (article, qteStock, idSite) => {
  const parsedParams = new URLSearchParams({
    QteStock: qteStock,
    IdSite: idSite,
    $expand: "ArticlesSites",
  }).toString();

  const URL = ODATA_URL + TABLE + "?" + parsedParams;
  const { QteStockSum, ...data } = article;
  try {
    const res = await (
      await fetch(URL, {
        method: "POST",
        cache: "no-cache",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      })
    ).json();
    return res;
  } catch (e) {
    console.log(e);
  }
};

export const getLowStockCount = async (IdSite) => {
  const URL = "/Statistics/LowStockArticles?IdSite=" + IdSite;
  try {
    const res = await (await fetch(URL)).json();
    return res;
  } catch (e) {
    console.log(e);
    return [];
  }
};

export const getTotalStock = async (IdSite) => {
  const URL = "/Statistics/TotalStock?IdSite=" + IdSite;
  try {
    const res = await (await fetch(URL)).json();
    return res;
  } catch (e) {
    console.log(e);
    return [];
  }
};

export const getTotalStockFacture = async () => {
  const URL = "/Statistics/TotalStockFacture";
  try {
    const res = await (await fetch(URL)).json();
    return res;
  } catch (e) {
    console.log(e);
    return [];
  }
};

export const getMarginArticles = async (idSite, skip, filters) => {
  const parsedParams = new URLSearchParams({
    IdSite: idSite,
    Skip: skip,
    SearchText: filters.searchText,
    From: filters.dateFrom?.toISOString(),
    To: filters.dateTo?.toISOString(),
  }).toString();
  const URL = "/Statistics/ArticlesWithMargin?" + parsedParams;
  try {
    const res = await (await fetch(URL)).json();
    return res;
  } catch (e) {
    console.log(e);
    return [];
  }
};

export const getArticlesNotSelling = async (idSite, skip, filters) => {
  const parsedParams = new URLSearchParams({
    IdSite: idSite,
    Skip: skip,
    SearchText: filters.searchText,
    Months: filters.months,
  }).toString();
  const URL = "/Statistics/ArticlesNotSellingIn?" + parsedParams;
  try {
    const res = await (await fetch(URL)).json();
    return res;
  } catch (e) {
    console.log(e);
    return [];
  }
};

//Inventory
export const getInventoryList = async (siteId, limit) => {
  const parsedParams = new URLSearchParams({
    idSite: siteId,
    limit,
  }).toString();

  const URL = "/Inventory/getInventaireList?" + parsedParams;

  try {
    const res = await (await fetch(URL, {})).json();
    return res;
  } catch (e) {
    console.log(e);
    return 0;
  }
};
