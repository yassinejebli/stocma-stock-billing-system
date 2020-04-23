import React from 'react';

const SiteContext = React.createContext({
    siteId: 1,
    siteName: 'Magasin 1'
});

export const useSite = () => {
    const context = React.useContext(SiteContext);
    if (!context) {
        throw new Error('useSite must be used within a SiteProvider')
    }
    return context
}


const SiteProvider = ({children}) => {
    const [siteId, setSiteId] = React.useState(1);
    const [siteName, setSiteName] = React.useState('Magasin 1');

    const setSite = (site) => {
        setIdSite(site)
    }

    return (
        <SiteContext.Provider value={{
            siteId,
            siteName,
            setSite
        }}>
            {children}
        </SiteContext.Provider>)
};

export default SiteProvider
