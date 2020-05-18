import React from 'react';
import { render } from 'react-dom';
import { HashRouter as Router, Route, Switch } from "react-router-dom";
import Dashboard from './components/views/dashboard/Dashboard';
import BonLivraison from './components/views/bonLivraison/BonLivraison';
import NavigationMenu from './components/elements/navigation-menu/NavigationMenu';
import ThemeProvider from './components/providers/ThemeProvider';
import { ModalProvider } from "react-modal-hook";
import TopBar from './components/elements/layout/TopBar';
import TitleProvider from './components/providers/TitleProvider';
import { TransitionGroup } from "react-transition-group";
import ClientList from './components/views/client/clientList';
import SnackBarProvider from './components/providers/SnackBarProvider';
import SupplierList from './components/views/fournisseur/SupplierList';
import BonLivraisonList from './components/views/bonLivraison/BonLivraisonList';
import SiteProvider from './components/providers/SiteProvider';
import ArticleList from './components/views/articles/ArticleList';
import ArticlesMarginList from './components/views/articles/ArticlesMarginList';
import SiteList from './components/views/sites/SiteList';
import BonReception from './components/views/bonReception/BonReception';
import BonReceptionList from './components/views/bonReception/BonReceptionList';
import Devis from './components/views/devis/Devis';
import DevisList from './components/views/devis/DevisList';
import Facture from './components/views/facture-client/FactureClient';
import SettingsProvider from './components/providers/SettingsProvider';
import FactureList from './components/views/facture-client/FactureClientList';
import SideMenu from './components/elements/layout/SideMenu';

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
                                            <Route path="/Devis" component={Devis} />
                                            <Route path="/Facture" component={Facture} />
                                            <Route path="/BonReception" component={BonReception} />
                                            <Route path="/BonLivraison" component={BonLivraison} />
                                            <Route path="/BonLivraisonList" component={BonLivraisonList} />
                                            <Route path="/FactureList" component={FactureList} />
                                            <Route path="/BonReceptionList" component={BonReceptionList} />
                                            <Route path="/DevisList" component={DevisList} />
                                            <Route path="/ArticleList" component={ArticleList} />
                                            <Route path="/SiteList" component={SiteList} />
                                            <Route path="/ArticlesMarginList" component={ArticlesMarginList} />
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