export const getPrintBonLivraisonURL = (params) => {
    const allParams = {
        ...params,
        IsArabic: false,
        ShowPrices: true
    };
    const parsedParams = new URLSearchParams(allParams).toString();
    return `/Print/BL?${parsedParams}`
}

export const getImageURL = (fileName) => {
    return `/UserFiles/images/articles/${fileName}`;
};