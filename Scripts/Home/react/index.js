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

const App = () => {
    return (
        <ThemeProvider>
            <SnackBarProvider>
                <ModalProvider rootComponent={TransitionGroup}>
                    <TitleProvider>
                        <Router>
                            <TopBar />
                            <Switch>
                                <Route exact path="/" component={Dashboard} />
                                <Route path="/BonLivraison" component={BonLivraison} />
                                <Route path="/BonLivraisonList" component={BonLivraisonList} />
                                <Route path="/ClientList" component={ClientList} />
                                <Route path="/SupplierList" component={SupplierList} />
                            </Switch>
                            <NavigationMenu />
                        </Router>
                    </TitleProvider>
                </ModalProvider>
            </SnackBarProvider>
        </ThemeProvider>
    )
}

render(<App />, document.getElementById('app'));