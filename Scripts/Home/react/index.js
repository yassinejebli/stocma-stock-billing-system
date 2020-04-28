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

const App = () => {
    return (
        <SiteProvider>
            <ThemeProvider>
                <SnackBarProvider>
                    <ModalProvider rootComponent={TransitionGroup}>
                        <TitleProvider>
                            <Router>
                                <TopBar />
                                <Switch>
                                    <Route exact path="/" component={Dashboard} />
                                    <Route path="/BonReception" component={BonReception} />
                                    <Route path="/BonLivraison" component={BonLivraison} />
                                    <Route path="/BonLivraisonList" component={BonLivraisonList} />
                                    <Route path="/BonReceptionList" component={BonReceptionList} />
                                    <Route path="/ArticleList" component={ArticleList} />
                                    <Route path="/SiteList" component={SiteList} />
                                    <Route path="/ArticlesMarginList" component={ArticlesMarginList} />
                                    <Route path="/ClientList" component={ClientList} />
                                    <Route path="/SupplierList" component={SupplierList} />
                                </Switch>
                                <NavigationMenu />
                            </Router>
                        </TitleProvider>
                    </ModalProvider>
                </SnackBarProvider>
            </ThemeProvider>
        </SiteProvider>
    )
}

render(<App />, document.getElementById('app'));