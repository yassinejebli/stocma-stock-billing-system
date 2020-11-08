import React from 'react';
import { render } from 'react-dom';
import { HashRouter as Router, Route, Switch } from "react-router-dom";
import Dashboard from './components/views/dashboard/Dashboard';
import BonLivraison from './components/views/bon-livraison/BonLivraison';
import ThemeProvider from './components/providers/ThemeProvider';
import { ModalProvider } from "react-modal-hook";
import TitleProvider from './components/providers/TitleProvider';
import { TransitionGroup } from "react-transition-group";
import ClientList from './components/views/client/clientList';
import SnackBarProvider from './components/providers/SnackBarProvider';
import SupplierList from './components/views/fournisseur/SupplierList';
import BonLivraisonList from './components/views/bon-livraison/BonLivraisonList';
import SiteProvider, { useSite } from './components/providers/SiteProvider';
import ArticleList from './components/views/articles/ArticleList';
import ArticlesMarginList from './components/views/articles/ArticlesMarginList';
import SiteList from './components/views/sites/SiteList';
import BonReception from './components/views/bon-reception/BonReception';
import BonReceptionList from './components/views/bon-reception/BonReceptionList';
import Devis from './components/views/devis/Devis';
import DevisList from './components/views/devis/DevisList';
import Facture from './components/views/facture-client/FactureClient';
import SettingsProvider, { useSettings } from './components/providers/SettingsProvider';
import FactureList from './components/views/facture-client/FactureClientList';
import SideMenu from './components/elements/layout/SideMenu';
import ScrollToTop from './components/elements/misc/ScrollToTop';
import FakeFacture from './components/views/fake-facture-client/FakeFactureClient';
import FakeFactureClientList from './components/views/fake-facture-client/FakeFactureClientList';
import FakeArticleList from './components/views/articles/FakeArticleList';
import FakeFactureAchat from './components/views/fake-facture-achat/FakeFactureAchat';
import FakeFactureAchatList from './components/views/fake-facture-achat/FakeFactureAchatList';
import FactureAchat from './components/views/facture-achat/FactureAchat';
import FactureAchatList from './components/views/facture-achat/FactureAchatList';
import BonCommande from './components/views/bon-commande/BonCommande';
import BonCommandeList from './components/views/bon-commande/BonCommandeList';
import BonAvoirVente from './components/views/bon-avoir-vente/BonAvoirVente';
import BonAvoirVenteList from './components/views/bon-avoir-vente/AvoirVenteList';
import BonAvoirAchat from './components/views/bon-avoir-achat/BonAvoirAchat';
import BonAvoirAchatList from './components/views/bon-avoir-achat/BonAvoirAchatList';
import StockMouvement from './components/views/stock-mouvement/StockMouvement';
import StockMouvementList from './components/views/stock-mouvement/StockMouvementList';
import SuiviVentes from './components/views/ventes/suivi/SuiviVentes';
import SuiviAchats from './components/views/achats/suivi/SuiviAchats';
import PaiementClientList from './components/views/paiement-client/PaiementClientList';
import PaiementFournisseurList from './components/views/paiement-fournisseur/PaiementFournisseurList';
import Depense from './components/views/depense/Depense';
import DepenseList from './components/views/depense/DepenseList';
import Rapports from './components/views/ventes/rapports/Rapports';
import LoaderProvider from './components/providers/LoaderProvider';
import TypeDepenseList from './components/views/type-depense/TypeDepenseList';
import UtilisateurList from './components/views/utilisateurs/UtilisateurList';
import AuthProvider, { useAuth } from './components/providers/AuthProvider';
import TypePaiementList from './components/views/type-paiement/TypePaiementList';
import BankPaiementsClientList from './components/views/paiement-client/BankPaiementsClientList';
import BankPaiementsFournisseurList from './components/views/paiement-fournisseur/BankPaiementsFournisseurList';
import ClientsMarginList from './components/views/client/ClientsMarginList';
import CodeBarreEtiquettes from './components/views/code-barres/CodeBarreEtiquettes';
import Inventaire from './components/views/stock-mouvement/Inventaire';
import InventaireList from './components/views/stock-mouvement/InventaireList';
import ArticlesNotSellingList from './components/views/articles/ArticlesNotSellingList';
import ClientsNotBuyingList from './components/views/client/ClientsNotBuyingList';
import ArticleCategoriesList from './components/views/article-categories/ArticleCategoriesList';
import PaiementClientFactureList from './components/views/paiement-client-facture/PaiementClientFactureList';
import PaiementFactureFournisseurList from './components/views/paiement-fournisseur-facture/PaiementFactureFournisseurList';

const Routes = () => {
    const {
        barcodeModule,
        articleMarginModule,
        clientMarginModule,
        depenseModule,
        siteModule,
        utilisateursModule,
        suiviModule,
        mouvementModule,
        rapportVenteModule,
        paiementModule,
        clientLoyaltyModule,
        articlesNotSellingModule,
    } = useSettings();
    const {
        isAdmin,
        canManageClients,
        canManageFournisseurs,
        canViewDashboard,
        canManageMouvements,
        canManageArticles,
        canManageSites,
        canManageDepenses,
        canManageBonReceptions,
        canManageBonAvoirsVente,
        canManageBonAvoirsAchat,
        canManageFacturesVente,
        canManageFacturesAchat,
        canManagePaiementsClients,
        canManagePaiementsFournisseurs,
        canViewSuiviAchats,
    } = useAuth();

    const {useVAT} = useSite();

    return <>
        {canViewDashboard && <Route exact path="/" component={Dashboard} />}
        {isAdmin && utilisateursModule?.Enabled && <Route path="/liste-des-utilisateurs" component={UtilisateurList} />}
        {isAdmin && rapportVenteModule?.Enabled && <Route path="/rapports-des-ventes" component={Rapports} />}

        {
            depenseModule?.Enabled && <>
                {canManageDepenses && <Route path="/liste-types-de-depense" component={TypeDepenseList} />}
                {canManageDepenses && <Route path="/depense" component={Depense} />}
                {canManageDepenses && <Route path="/liste-des-depenses" component={DepenseList} />}
            </>
        }

        {
            paiementModule?.Enabled && <>
                {canManagePaiementsFournisseurs && <Route path="/liste-paiements-des-fournisseurs" component={useVAT?PaiementFactureFournisseurList : PaiementFournisseurList} />}
                {canManagePaiementsClients && <Route path="/liste-paiements-des-clients" component={useVAT ? PaiementClientFactureList:PaiementClientList} />}
                {canManagePaiementsClients && <Route path="/liste-cheques-et-effets-des-clients" component={BankPaiementsClientList} />}
                {canManagePaiementsFournisseurs && <Route path="/liste-cheques-et-effets-des-fournisseurs" component={BankPaiementsFournisseurList} />}
            </>
        }



        {suiviModule?.Enabled && <Route path="/suivi-des-ventes" component={SuiviVentes} />}
        {suiviModule?.Enabled && canViewSuiviAchats && <Route path="/suivi-des-achats" component={SuiviAchats} />}

        {
            mouvementModule?.Enabled && <>
                {canManageMouvements && <Route path="/mouvement-stock" component={StockMouvement} />}
                {canManageMouvements && <Route path="/liste-mouvement-stock" component={StockMouvementList} />}
            </>
        }

        {canManageBonAvoirsAchat && <Route path="/bon-avoir-achat" component={BonAvoirAchat} />}
        {canManageBonAvoirsVente && <Route path="/liste-bon-avoir-achat" component={BonAvoirAchatList} />}

        {canManageBonAvoirsVente && <Route path="/bon-avoir-vente" component={BonAvoirVente} />}
        {canManageBonAvoirsVente && <Route path="/liste-bon-avoir-vente" component={BonAvoirVenteList} />}

        <Route path="/BonCommande" component={BonCommande} />
        <Route path="/BonCommandeList" component={BonCommandeList} />
        <Route path="/Devis" component={Devis} />

        {canManageFacturesAchat && <Route path="/FactureAchat" component={FactureAchat} />}
        {canManageFacturesAchat && <Route path="/FactureAchatList" component={FactureAchatList} />}
        {canManageFacturesAchat && <Route path="/_FactureAchat" component={FakeFactureAchat} />}
        {canManageFacturesAchat && <Route path="/_FactureAchatList" component={FakeFactureAchatList} />}

        {canManageFacturesVente && <Route path="/_Facture" component={FakeFacture} />}
        {canManageFacturesVente && <Route path="/_FactureList" component={FakeFactureClientList} />}
        {canManageFacturesVente && <Route path="/Facture" component={Facture} />}
        {canManageFacturesVente && <Route path="/FactureList" component={FactureList} />}

        {barcodeModule?.Enabled && <Route path="/code-barres" component={CodeBarreEtiquettes} />}

        {canManageBonReceptions && <Route path="/BonReception" component={BonReception} />}
        <Route path="/BonLivraison" component={BonLivraison} />
        <Route path="/BonLivraisonList" component={BonLivraisonList} />
        {canManageBonReceptions && <Route path="/BonReceptionList" component={BonReceptionList} />}
        <Route path="/DevisList" component={DevisList} />

        <Route path="/inventaire" component={Inventaire} />
        <Route path="/liste-inventaire" component={InventaireList} />

        {canManageArticles && <Route path="/ArticleList" component={ArticleList} />}
        {canManageArticles && <Route path="/_ArticleList" component={FakeArticleList} />}
        {canManageSites && siteModule?.Enabled && <Route path="/SiteList" component={SiteList} />}
        {isAdmin && articleMarginModule?.Enabled && <Route path="/marge-articles" component={ArticlesMarginList} />}
        {isAdmin && articlesNotSellingModule?.Enabled && <Route path="/articles-non-vendus" component={ArticlesNotSellingList} />}
        {isAdmin && clientMarginModule?.Enabled && <Route path="/marge-clients" component={ClientsMarginList} />}
        {clientLoyaltyModule?.Enabled && <Route path="/clients-non-fidèles" component={ClientsNotBuyingList} />}
        {canManageClients && <Route path="/ClientList" component={ClientList} />}
        {canManageFournisseurs && <Route path="/SupplierList" component={SupplierList} />}
        {isAdmin && paiementModule?.Enabled && <Route path="/liste-methodes-de-paiement" component={TypePaiementList} />}
        {canManageArticles && <Route path="/liste-des-familles" component={ArticleCategoriesList} />}
    </>;
}

const App = () => {
    return (
        <LoaderProvider>
            <AuthProvider>
                <SiteProvider>
                    <SettingsProvider>
                        <ThemeProvider>
                            <SnackBarProvider>
                                <TitleProvider>
                                    <Router>
                                        <ScrollToTop />
                                        <ModalProvider rootComponent={TransitionGroup}>
                                            <Switch>
                                                <SideMenu onClose={() => null}>
                                                    <Routes />
                                                </SideMenu>
                                            </Switch>
                                            {/* <NavigationMenu /> */}
                                        </ModalProvider>
                                    </Router>
                                </TitleProvider>
                            </SnackBarProvider>
                        </ThemeProvider>
                    </SettingsProvider>
                </SiteProvider>
            </AuthProvider>
        </LoaderProvider>
    )
}

render(<App />, document.getElementById('app'));