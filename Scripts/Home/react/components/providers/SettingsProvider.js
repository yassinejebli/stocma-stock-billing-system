import React from 'react';
import { getAllData, updateData } from '../../queries/crudBuilder';

const TABLE = 'Settings';
const SettingsContext = React.createContext({
});

export const useSettings = () => {
    const context = React.useContext(SettingsContext);
    if (!context) {
        throw new Error('useSettings must be used within a SettingsProvider')
    }
    return context
}

const SettingsProvider = ({ children }) => {
    //Modules
    const [barcodeModule, setBarcodeModule] = React.useState(null);
    const [articleMarginModule, setArticleMarginModule] = React.useState(null);
    const [clientMarginModule, setClientMarginModule] = React.useState(null);
    const [depenseModule, setDepenseModule] = React.useState(null);
    const [siteModule, setSiteModule] = React.useState(null);
    const [suiviModule, setSuiviModule] = React.useState(null);
    const [utilisateursModule, setUtilisateursModule] = React.useState(null);
    const [rapportVenteModule, setRapportVenteModule] = React.useState(null);
    const [mouvementModule, setMouvementModule] = React.useState(null);
    const [paiementModule, setPaiementModule] = React.useState(null);
    const [articleImageModule, setArticleImageModule] = React.useState(null);
    const [restoreBLModule, setRestoreBLModule] = React.useState(null);
    const [articlesStatisticsModule, setArticlesStatisticsModule] = React.useState(null);
    const [clientLoyaltyModule, setClientLoyaltyModule] = React.useState(null);
    const [articlesNotSellingModule, setArticlesNotSellingModule] = React.useState(null);

    //BL
    const [BLDiscount, setBLDiscount] = React.useState(null);
    const [BLPayment, setBLPayment] = React.useState(null);

    //FA
    const [factureDiscount, setFactureDiscount] = React.useState(null);
    const [facturePayment, setFacturePayment] = React.useState(null);
    const [factureCheque, setFactureCheque] = React.useState(null);

    //Devis
    const [devisDiscount, setDevisDiscount] = React.useState(null);
    const [devisValidity, setDevisValidity] = React.useState(null);
    const [devisPayment, setDevisPayment] = React.useState(null);
    const [devisTransport, setDevisTransport] = React.useState(null);
    const [devisDeliveryTime, setDevisDeliveryTime] = React.useState(null);

    //Barcode
    const [barcode, setBarcode] = React.useState(null);

    React.useEffect(() => {
        fetchSettings();
    }, []);


    //FA client
    React.useEffect(() => {
        if (facturePayment)
            updateData(TABLE, facturePayment, facturePayment?.Id);
    }, [facturePayment]);

    React.useEffect(() => {
        if (factureCheque)
            updateData(TABLE, factureCheque, factureCheque?.Id);
    }, [factureCheque]);

    //Barcode
    React.useEffect(() => {
        if (barcode)
            updateData(TABLE, barcode, barcode?.Id);
    }, [barcode]);

    //BL
    React.useEffect(() => {
        if (BLPayment)
            updateData(TABLE, BLPayment, BLPayment?.Id);
    }, [BLPayment]);
    // React.useEffect(() => {
    //     if (BLDiscount)
    //         updateData(TABLE, BLDiscount, BLDiscount?.Id);
    // }, [BLDiscount]);

    //Devis
    // React.useEffect(() => {
    //     if (devisDiscount)
    //         updateData(TABLE, devisDiscount, devisDiscount?.Id);
    // }, [devisDiscount]);

    React.useEffect(() => {
        if (devisValidity)
            updateData(TABLE, devisValidity, devisValidity?.Id);
    }, [devisValidity]);

    React.useEffect(() => {
        if (devisPayment)
            updateData(TABLE, devisPayment, devisPayment?.Id);
    }, [devisPayment]);

    React.useEffect(() => {
        if (devisTransport)
            updateData(TABLE, devisTransport, devisTransport?.Id);
    }, [devisTransport]);

    React.useEffect(() => {
        if (devisDeliveryTime)
            updateData(TABLE, devisDeliveryTime, devisDeliveryTime?.Id);
    }, [devisDeliveryTime]);

    const fetchSettings = () => {
        getAllData(TABLE)
            .then(res => {
                //Modules
                setBarcodeModule(res.find(x => x.Code === 'module_barcode'));
                setArticleMarginModule(res.find(x => x.Code === 'module_article_margin'));
                setClientMarginModule(res.find(x => x.Code === 'module_client_margin'));
                setDepenseModule(res.find(x => x.Code === 'module_depense'));
                setSiteModule(res.find(x => x.Code === 'module_site'));
                setSuiviModule(res.find(x => x.Code === 'module_suivi'));
                setUtilisateursModule(res.find(x => x.Code === 'module_utilisateurs'));
                setRapportVenteModule(res.find(x => x.Code === 'module_rapport_vente'));
                setMouvementModule(res.find(x => x.Code === 'module_mouvement'));
                setPaiementModule(res.find(x => x.Code === 'module_paiement'));
                setArticleImageModule(res.find(x => x.Code === 'module_image_article'));
                setRestoreBLModule(res.find(x => x.Code === 'module_restoration_bl'));
                setArticlesStatisticsModule(res.find(x => x.Code === 'module_statistique_articles'));
                setClientLoyaltyModule(res.find(x => x.Code === 'module_client_fidelite'));
                setArticlesNotSellingModule(res.find(x => x.Code === 'module_article_non_vendus'));
                //Devis
                setDevisDiscount(res.find(x => x.Code === 'devis_discount'));
                setDevisValidity(res.find(x => x.Code === 'devis_validity'));
                setDevisPayment(res.find(x => x.Code === 'devis_payment'));
                setDevisTransport(res.find(x => x.Code === 'devis_transport'));
                setDevisDeliveryTime(res.find(x => x.Code === 'devis_delivery_time'));
                //BL
                setBLDiscount(res.find(x => x.Code === 'bl_discount'));
                setBLPayment(res.find(x => x.Code === 'bl_payment'));
                //FA
                setFactureDiscount(res.find(x => x.Code === 'fa_discount'));
                setFacturePayment(res.find(x => x.Code === 'fa_payment'));
                setFactureCheque(res.find(x => x.Code === 'fa_cheque'));

                //Barcode
                setBarcode(res.find(x => x.Code === 'barcode'));

            })
            .catch(err => console.error(err));
    }

    return (
        <SettingsContext.Provider value={{
            devisDiscount,
            setDevisDiscount,
            devisValidity,
            setDevisValidity,
            devisPayment,
            setDevisPayment,
            devisTransport,
            setDevisTransport,
            devisDeliveryTime,
            setDevisDeliveryTime,
            //BL
            BLDiscount,
            setBLDiscount,
            BLPayment,
            setBLPayment,
            //FA
            factureDiscount,
            setFactureDiscount,
            facturePayment,
            setFacturePayment,
            factureCheque,
            setFactureCheque,
            //Barcode,
            barcode,
            setBarcode,
            //Modules
            barcodeModule,
            setBarcodeModule,
            articleMarginModule,
            setArticleMarginModule,
            clientMarginModule,
            depenseModule,
            siteModule,
            suiviModule,
            utilisateursModule,
            rapportVenteModule,
            mouvementModule,
            paiementModule,
            articleImageModule,
            restoreBLModule,
            articlesStatisticsModule,
            clientLoyaltyModule,
            articlesNotSellingModule,
        }}>
            {children}
        </SettingsContext.Provider>)
};

export default SettingsProvider
