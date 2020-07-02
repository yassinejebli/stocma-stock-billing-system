import React from 'react';
import { getUserInfo } from '../../queries/utilisateurQueries';

const AuthContext = React.createContext();

export const useAuth = () => {
    const context = React.useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within a AuthProvider')
    }
    return context
}

const AuthProvider = ({ children }) => {
    const [isAdmin, setIsAdmin] = React.useState(false);
    const [username, setUsername] = React.useState();
    const [canUpdateQteStock, setCanUpdateQteStock] = React.useState(false);
    const [canViewDashboard, setCanViewDashboard] = React.useState(false);
    const [canManageArticles, setCanManageArticles] = React.useState(false);
    const [canManageClients, setCanManageClients] = React.useState(false);
    const [canManageFournisseurs, setCanManageFournisseurs] = React.useState(false);
    const [canManagePaiementsClients, setCanManagePaiementsClients] = React.useState(false);
    const [canManagePaiementsFournisseurs, setCanManagePaiementsFournisseurs] = React.useState(false);
    const [canManageMouvements, setCanManageMouvements] = React.useState(false);
    const [canManageSites, setCanManageSites] = React.useState(false);
    const [canManageDepenses, setCanManageDepenses] = React.useState(false);
    //Documents
    //-----Bon de reception
    const [canManageBonReceptions, setCanManageBonReceptions] = React.useState(false);
    //-----Bon de livraison
    const [canAddBonLivraison, setCanAddBonLivraison] = React.useState(false);
    const [canDeleteBonLivraison, setCanDeleteBonLivraison] = React.useState(false);
    const [canUpdateBonLivraison, setCanUpdateBonLivraison] = React.useState(false);
    const [canViewBonLivraison, setCanViewBonLivraison] = React.useState(false);
    //-----Facture de vente
    const [canManageFacturesVente, setCanManageFacturesVente] = React.useState(false);
    //-----Facture d'achat
    const [canManageFacturesAchat, setCanManageFacturesAchat] = React.useState(false);
    

    React.useEffect(()=>{
        getUserInfo().then(response=>{
            const claims = response?.claims;
            const isAdmin = response?.isAdmin;
            console.log({claims})
            setIsAdmin(isAdmin);
            setUsername(response?.username);
            //Documents
            setCanAddBonLivraison(Boolean(claims.find(x=>x==='CanAddBonLivraison'))||isAdmin);
            setCanUpdateBonLivraison(Boolean(claims.find(x=>x==='CanUpdateBonLivraison'))||isAdmin);
            setCanDeleteBonLivraison(Boolean(claims.find(x=>x==='CanDeleteBonLivraison'))||isAdmin);
            setCanViewBonLivraison(Boolean(claims.find(x=>x==='CanViewBonLivraison'))||isAdmin);

            setCanManageBonReceptions(Boolean(claims.find(x=>x==='CanManageBonReceptions'))||isAdmin);

            setCanManageFacturesVente(Boolean(claims.find(x=>x==='CanManageFacturesVente'))||isAdmin);

            setCanManageFacturesAchat(Boolean(claims.find(x=>x==='CanManageFacturesAchat'))||isAdmin);

            //
            setCanViewDashboard(Boolean(claims.find(x=>x==='CanViewDashboard'))||isAdmin);
            setCanUpdateQteStock(Boolean(claims.find(x=>x==='CanUpdateQteStock'))||isAdmin);
            setCanManageClients(Boolean(claims.find(x=>x==='CanManageClients'))||isAdmin);
            setCanManageFournisseurs(Boolean(claims.find(x=>x==='CanManageFournisseurs'))||isAdmin);
            setCanManagePaiementsClients(Boolean(claims.find(x=>x==='CanManagePaiementsClients'))||isAdmin);
            setCanManagePaiementsFournisseurs(Boolean(claims.find(x=>x==='CanManagePaiementsFournisseurs'))||isAdmin);
            setCanManageArticles(Boolean(claims.find(x=>x==='CanManageArticles'))||isAdmin);
            setCanManageMouvements(Boolean(claims.find(x=>x==='CanManageMouvements'))||isAdmin);
            setCanManageSites(Boolean(claims.find(x=>x==='CanManageSites'))||isAdmin);
            setCanManageDepenses(Boolean(claims.find(x=>x==='CanManageDepenses'))||isAdmin);
        })
    },[])

    return (
        <AuthContext.Provider value={{
            username,
            isAdmin,
            //Paiements
            canManagePaiementsFournisseurs,
            canManagePaiementsClients,
            //Misc
            canUpdateQteStock,
            canViewDashboard,
            canManageClients,
            canManageFournisseurs,
            canManageArticles,
            canManageMouvements,
            canManageSites,
            canManageDepenses,
            //Documents
            canManageBonReceptions,
            canAddBonLivraison,
            canUpdateBonLivraison,
            canDeleteBonLivraison,
            canViewBonLivraison,
            canManageFacturesVente,
            canManageFacturesAchat,
        }}>
            {children}
        </AuthContext.Provider>)
};

export default AuthProvider
