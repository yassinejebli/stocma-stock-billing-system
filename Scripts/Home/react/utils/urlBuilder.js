export const getPrintBonLivraisonURL = (params) => {
    const allParams = {
        ...params,
        IsArabic: false,
        ShowPrices: true
    };
    const parsedParams = new URLSearchParams(allParams).toString();
    return `/Print/BL?${parsedParams}`
}
export const getPrintBonReceptionURL = (params) => {
    const parsedParams = new URLSearchParams(params).toString();
    return `/Print/BR?${parsedParams}`
}

export const getPrintDevisURL = (params) => {
    const parsedParams = new URLSearchParams(params).toString();
    return `/Print/Devis?${parsedParams}`
}

export const getPrintFactureURL = (params) => {
    const parsedParams = new URLSearchParams(params).toString();
    return `/Print/Facture?${parsedParams}`
}

export const getPrintFakeFactureURL = (params) => {
    const parsedParams = new URLSearchParams(params).toString();
    return `/Print/FakeFacture?${parsedParams}`
}

export const getImageURL = (fileName) => {
    return `/UserFiles/images/articles/${fileName}`;
};