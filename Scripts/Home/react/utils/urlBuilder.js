export const getPrintBonLivraisonURL = (params) => {
  const allParams = {
    ...params,
    IsArabic: false,
    ShowPrices: true,
  };
  const parsedParams = new URLSearchParams(allParams).toString();
  return `/BL?${parsedParams}`;
};
export const getPrintBonReceptionURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/BR?${parsedParams}`;
};

export const getPrintDevisURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Devis?${parsedParams}`;
};

export const getPrintFactureURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Facture?${parsedParams}`;
};

export const getPrintFakeFactureURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/FakeFacture?${parsedParams}`;
};

export const getPrintFakeFactureAchatURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/FakeFactureAchat?${parsedParams}`;
};

export const getImageURL = (fileName) => {
  return `/UserFiles/images/articles/${fileName}`;
};

export const getPrintBonCommandeURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/BonCommande?${parsedParams}`;
};

export const getPrintBonAvoirVenteURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/BonAvoirVente?${parsedParams}`;
};

export const getPrintClientAccountSummaryURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Paiements?${parsedParams}`;
};

export const getExportClientAccountSummaryURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Paiements/Export?${parsedParams}`;
};

export const getPrintFournisseurAccountSummaryURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/PaiementsF?${parsedParams}`;
};

export const getPrintRapportVentesBLByClientURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/RapportsVentes/BLByClient?${parsedParams}`;
};

export const getPrintRapportVentesBLURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/RapportsVentes/BL?${parsedParams}`;
};

export const getPrintRapportVentesFAURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/RapportsVentes/FA?${parsedParams}`;
};

export const getPrintRapportTransactionURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/RapportsVentes/Transactions?${parsedParams}`;
};

export const getPrintCodeBarreEtiquetteURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Barcode/SingleEtiquette?${parsedParams}`;
};

export const getPrintMultipleCodeBarreEtiquetteURL = (ids) => {
  return `/Barcode/MultipleEtiquettes?${ids}`;
};

export const getPrintInventaireURL = ({ ids, idSite, titre, showBarCode }) => {
  return `/PrintInventory?${ids}&idSite=${idSite}&titre=${titre}&showBarCode=${showBarCode}`;
};

export const getPrintInventaireTousLesArticlesURL = ({ idSite }) => {
  return `/PrintInventory/TousLesArticles?idSite=${idSite}`;
};

export const getPrintInventaireArticleNonCalculesURL = ({ idSite }) => {
  return `/PrintInventory/ArticlesNonCalcules?idSite=${idSite}`;
};

export const getPrintSituationGlobaleClientsURL = () => {
  return `/SituationGlobale/Clients`;
};

export const getPrintSituationGlobaleFournisseursURL = () => {
  return `/SituationGlobale/Fournisseurs`;
};

export const getPrintTarifURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/Tarif?${parsedParams}`;
};

export const getPrintArticleFactureReport = () => {
  return `/EtatArticleFacture`;
};

export const getExportStockURL = (params) => {
  const parsedParams = new URLSearchParams(params).toString();
  return `/ExportArticles?${parsedParams}`;
};
