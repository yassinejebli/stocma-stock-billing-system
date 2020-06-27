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
import SiteProvider from './components/providers/SiteProvider';
import ArticleList from './components/views/articles/ArticleList';
import ArticlesMarginList from './components/views/articles/ArticlesMarginList';
import SiteList from './components/views/sites/SiteList';
import BonReception from './components/views/bon-reception/BonReception';
import BonReceptionList from './components/views/bon-reception/BonReceptionList';
import Devis from './components/views/devis/Devis';
import DevisList from './components/views/devis/DevisList';
import Facture from './components/views/facture-client/FactureClient';
import SettingsProvider from './components/providers/SettingsProvider';
import FactureList from './components/views/facture-client/FactureClientList';
import SideMenu from './components/elements/layout/SideMenu';
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

const App = () => {
    return (
        <SiteProvider>
            <SettingsProvider>
                <ThemeProvider>
                    <SnackBarProvider>
                        <TitleProvider>
                            <Router>
                                <ModalProvider rootComponent={TransitionGroup}>
                                    <Switch>
                                        <SideMenu onClose={() => null}>
                                            <Route exact path="/" component={Dashboard} />
                                            <Route path="/depense" component={Depense} />
                                            <Route path="/liste-des-depenses" component={DepenseList} />
                                            <Route path="/liste-paiements-des-fournisseurs" component={PaiementFournisseurList} />
                                            <Route path="/liste-paiements-des-clients" component={PaiementClientList} />
                                            <Route path="/suivi-des-ventes" component={SuiviVentes} />
                                            <Route path="/suivi-des-achats" component={SuiviAchats} />
                                            <Route path="/mouvement-stock" component={StockMouvement} />
                                            <Route path="/liste-mouvement-stock" component={StockMouvementList} />
                                            <Route path="/bon-avoir-achat" component={BonAvoirAchat} />
                                            <Route path="/liste-bon-avoir-achat" component={BonAvoirAchatList} />
                                            <Route path="/bon-avoir-vente" component={BonAvoirVente} />
                                            <Route path="/liste-bon-avoir-vente" component={BonAvoirVenteList} />
                                            <Route path="/BonCommande" component={BonCommande} />
                                            <Route path="/BonCommandeList" component={BonCommandeList} />
                                            <Route path="/Devis" component={Devis} />
                                            <Route path="/FactureAchat" component={FactureAchat} />
                                            <Route path="/FactureAchatList" component={FactureAchatList} />
                                            <Route path="/_FactureAchat" component={FakeFactureAchat} />
                                            <Route path="/_FactureAchatList" component={FakeFactureAchatList} />
                                            <Route path="/_Facture" component={FakeFacture} />
                                            <Route path="/_FactureList" component={FakeFactureClientList} />
                                            <Route path="/Facture" component={Facture} />
                                            <Route path="/BonReception" component={BonReception} />
                                            <Route path="/BonLivraison" component={BonLivraison} />
                                            <Route path="/BonLivraisonList" component={BonLivraisonList} />
                                            <Route path="/FactureList" component={FactureList} />
                                            <Route path="/BonReceptionList" component={BonReceptionList} />
                                            <Route path="/DevisList" component={DevisList} />
                                            <Route path="/ArticleList" component={ArticleList} />
                                            <Route path="/_ArticleList" component={FakeArticleList} />
                                            <Route path="/SiteList" component={SiteList} />
                                            <Route path="/marge-articles" component={ArticlesMarginList} />
                                            <Route path="/ClientList" component={ClientList} />
                                            <Route path="/SupplierList" component={SupplierList} />
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
    )
}

render(<App />, document.getElementById('app'));