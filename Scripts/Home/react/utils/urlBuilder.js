export const getPrintBonLivraisonURL = (params) => {
    const allParams = {
        ...params,
        IsArabic: false,
        ShowPrices: true
    };
    const parsedParams = new URLSearchParams(allParams).toString();
    return `/Print/BL?${parsedParams}`
}

