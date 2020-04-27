import React from 'react';
import { getAllData } from '../../queries/crudBuilder';

const SiteContext = React.createContext({
    siteId: 1,
    siteName: 'Magasin 1',
    sites: []
});

export const useSite = () => {
    const context = React.useContext(SiteContext);
    if (!context) {
        throw new Error('useSite must be used within a SiteProvider')
    }
    return context
}

//use localstorage to save the selected site
const SiteProvider = ({ children }) => {
    const [siteId, setSiteId] = React.useState(1);
    const [siteName, setSiteName] = React.useState('Magasin 1');
    const [sites, setSites] = React.useState([]);

    React.useEffect(() => {
        getAllData('Sites')
            .then(res => setSites(res))
            .catch(err => console.error(err));
    }, []);

    const setSite = (site) => {
        if (site) {
            setSiteId(site.Id)
            setSiteName(site.Name)
        }
    }

    return (
        <SiteContext.Provider value={{
            siteId,
            siteName,
            setSite,
            sites,
            setSites
        }}>
            {children}
        </SiteContext.Provider>)
};

export default SiteProvider
