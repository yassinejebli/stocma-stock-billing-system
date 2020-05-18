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
            setFactureCheque
        }}>
            {children}
        </SettingsContext.Provider>)
};

export default SettingsProvider
