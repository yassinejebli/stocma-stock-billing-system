import React from 'react';
import { getAllData } from '../../queries/crudBuilder';

const SiteContext = React.createContext({
    siteId: 1,
    siteName: 'Magasin 1',
    sites: [],
    company: {}
});

export const useSite = () => {
    const context = React.useContext(SiteContext);
    if (!context) {
        throw new Error('useSite must be used within a SiteProvider')
    }
    return context
}

const SiteProvider = ({ children }) => {
    const savedSiteId = localStorage.getItem('site') ? Number(localStorage.getItem('site')) : 1;
    const [siteId, setSiteId] = React.useState(savedSiteId);
    const [siteName, setSiteName] = React.useState('Magasin 1');
    const [company, setCompany] = React.useState({});
    const [sites, setSites] = React.useState([]);

    React.useEffect(() => {
        fetchSites();
        fetchCompany();
    }, []);

    const fetchSites = () => {
        getAllData('Sites')
            .then(res => setSites(res.filter(x => !x.Disabled)))
            .catch(err => console.error(err));
    }

    const fetchCompany = () => {
        getAllData('Companies')
            .then(res => setCompany(res[0]))
            .catch(err => console.error(err));
    }

    console.log({company})
    
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
            hasMultipleSites: sites?.length > 1,
            siteName,
            setSite,
            sites,
            company,
            //TODO: remove this
            site: {
                Id: siteId,
                Name: sites?.find(x=>x.Id === siteId)?.Name
            },
            fetchSites,
            useVAT: company?.UseVAT
        }}>
            {children}
        </SiteContext.Provider>)
};

export default SiteProvider
