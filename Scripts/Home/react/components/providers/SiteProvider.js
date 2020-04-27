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
    const savedSiteId = localStorage.getItem('site') ?  Number(localStorage.getItem('site')) : 1;
    const [siteId, setSiteId] = React.useState(savedSiteId);
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
            localStorage.setItem('site', site.Id);
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
