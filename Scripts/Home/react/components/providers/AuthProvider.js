import React from 'react';
import { getUserInfo } from '../../queries/utilisateurQueries';
import { useLoader } from './LoaderProvider';

const AuthContext = React.createContext();

export const useAuth = () => {
    const context = React.useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within a AuthProvider')
    }
    return context
}

const AuthProvider = ({ children }) => {
    const { showLoader } = useLoader();
    const [isAdmin, setIsAdmin] = React.useState(false);
    const [username, setUsername] = React.useState();
    const [canUpdateQteStock, setCanUpdateQteStock] = React.useState(false);
    const [canViewDashboard, setCanViewDashboard] = React.useState(false);
    const [canViewSuiviAchats, setCanViewSuiviAchats] = React.useState(false);
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
    const [canAddBonLivraisons, setCanAddBonLivraisons] = React.useState(false);
    const [canDeleteBonLivraisons, setCanDeleteBonLivraisons] = React.useState(false);
    const [canUpdateBonLivraisons, setCanUpdateBonLivraisons] = React.useState(false);
    const [canViewBonLivraisons, setCanViewBonLivraisons] = React.useState(false);
    //-----Facture de vente
    const [canManageFacturesVente, setCanManageFacturesVente] = React.useState(false);
    //-----Facture d'achat
    const [canManageFacturesAchat, setCanManageFacturesAchat] = React.useState(false);
    //-----BA
    const [canManageBonAvoirsAchat, setCanManageBonAvoirsAchat] = React.useState(false);
    const [canManageBonAvoirsVente, setCanManageBonAvoirsVente] = React.useState(false);

    React.useEffect(() => {
        showLoader(true);
        getUserInfo().then(response => {
            const claims = response?.claims;
            const isAdmin = response?.isAdmin;
            console.log({ claims })
            setIsAdmin(isAdmin);
            setUsername(response?.username);
            //Documents
            setCanAddBonLivraisons(Boolean(claims.find(x => x === 'CanAddBonLivraisons')) || isAdmin);
            setCanUpdateBonLivraisons(Boolean(claims.find(x => x === 'CanUpdateBonLivraisons')) || isAdmin);
            setCanDeleteBonLivraisons(Boolean(claims.find(x => x === 'CanDeleteBonLivraisons')) || isAdmin);
            setCanViewBonLivraisons(Boolean(claims.find(x => x === 'CanViewBonLivraisons')) || isAdmin);

            setCanManageBonReceptions(Boolean(claims.find(x => x === 'CanManageBonReceptions')) || isAdmin);

            setCanManageFacturesVente(Boolean(claims.find(x => x === 'CanManageFacturesVente')) || isAdmin);

            setCanManageFacturesAchat(Boolean(claims.find(x => x === 'CanManageFacturesAchat')) || isAdmin);

            setCanManageBonAvoirsAchat(Boolean(claims.find(x => x === 'CanManageBonAvoirsAchat')) || isAdmin);

            setCanManageBonAvoirsVente(Boolean(claims.find(x => x === 'CanManageBonAvoirsVente')) || isAdmin);

            //
            setCanViewDashboard(Boolean(claims.find(x => x === 'CanViewDashboard')) || isAdmin);
            setCanViewSuiviAchats(Boolean(claims.find(x => x === 'CanViewSuiviAchats')) || isAdmin);
            setCanUpdateQteStock(Boolean(claims.find(x => x === 'CanUpdateQteStock')) || isAdmin);
            setCanManageClients(Boolean(claims.find(x => x === 'CanManageClients')) || isAdmin);
            setCanManageFournisseurs(Boolean(claims.find(x => x === 'CanManageFournisseurs')) || isAdmin);
            setCanManagePaiementsClients(Boolean(claims.find(x => x === 'CanManagePaiementsClients')) || isAdmin);
            setCanManagePaiementsFournisseurs(Boolean(claims.find(x => x === 'CanManagePaiementsFournisseurs')) || isAdmin);
            setCanManageArticles(Boolean(claims.find(x => x === 'CanManageArticles')) || isAdmin);
            setCanManageMouvements(Boolean(claims.find(x => x === 'CanManageMouvements')) || isAdmin);
            setCanManageSites(Boolean(claims.find(x => x === 'CanManageSites')) || isAdmin);
            setCanManageDepenses(Boolean(claims.find(x => x === 'CanManageDepenses')) || isAdmin);
        }).finally(() => {
            showLoader(false);
        })
    }, [])

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
            canAddBonLivraisons,
            canUpdateBonLivraisons,
            canDeleteBonLivraisons,
            canViewBonLivraisons,
            canManageFacturesVente,
            canManageFacturesAchat,
            canManageBonAvoirsAchat,
            canManageBonAvoirsVente,
            //
            canViewSuiviAchats,
        }}>
            {children}
        </AuthContext.Provider>)
};

export default AuthProvider
