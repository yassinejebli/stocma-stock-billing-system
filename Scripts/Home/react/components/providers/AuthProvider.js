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
    const [canManageClients, setCanManageClients] = React.useState(false);
    const [canManageFournisseurs, setCanManageFournisseurs] = React.useState(false);
    const [canAddPaiements, setCanAddPaiements] = React.useState(false);
    const [canRemovePaiements, setCanRemovePaiements] = React.useState(false);
    const [canViewPaiements, setCanViewPaiements] = React.useState(false);

    React.useEffect(()=>{
        getUserInfo().then(response=>{
            const claims = response?.claims;
            const isAdmin = response?.isAdmin;
            console.log({claims})
            setIsAdmin(isAdmin);
            setUsername(response?.username);
            setCanViewDashboard(Boolean(claims.find(x=>x==='CanViewDashboard'))||isAdmin);
            setCanUpdateQteStock(Boolean(claims.find(x=>x==='CanUpdateQteStock'))||isAdmin);
            setCanManageClients(Boolean(claims.find(x=>x==='CanManageClients'))||isAdmin);
            setCanManageFournisseurs(Boolean(claims.find(x=>x==='CanManageFournisseurs'))||isAdmin);
            setCanAddPaiements(Boolean(claims.find(x=>x==='CanAddPaiements'))||isAdmin);
            setCanRemovePaiements(Boolean(claims.find(x=>x==='CanRemovePaiements'))||isAdmin);
            setCanViewPaiements(Boolean(claims.find(x=>x==='CanViewPaiements'))||isAdmin);
        })
    },[])

    return (
        <AuthContext.Provider value={{
            username,
            isAdmin,
            //paiements
            canAddPaiements,
            canRemovePaiements,
            canViewPaiements,
            //
            canUpdateQteStock,
            canViewDashboard,
            canManageClients,
            canManageFournisseurs,
        }}>
            {children}
        </AuthContext.Provider>)
};

export default AuthProvider
