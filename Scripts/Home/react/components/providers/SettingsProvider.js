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
    const [devisDiscount, setDevisDiscount] = React.useState(null);
    const [devisValidity, setDevisValidity] = React.useState(null);
    const [devisPayment, setDevisPayment] = React.useState(null);
    const [devisTransport, setDevisTransport] = React.useState(null);
    const [devisDeliveryTime, setDevisDeliveryTime] = React.useState(null);

    React.useEffect(() => {
        fetchSettings();
    }, []);

    React.useEffect(() => {
        if (devisDiscount)
            updateData(TABLE, devisDiscount, devisDiscount?.Id);
    }, [devisDiscount]);

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
            setDevisDeliveryTime
        }}>
            {children}
        </SettingsContext.Provider>)
};

export default SettingsProvider
