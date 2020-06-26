import React from 'react';
import {
    ListItemIcon, ListItem,
    List, ListItemText, Divider, makeStyles, CssBaseline, Drawer,
    Collapse,
    Box
} from '@material-ui/core';
import DashboardIcon from '@material-ui/icons/Dashboard';
import PeopleAltIcon from '@material-ui/icons/PeopleAlt';
import LocalMallIcon from '@material-ui/icons/LocalMall';
import ExpandLess from '@material-ui/icons/ExpandLess';
import LocalOfferIcon from '@material-ui/icons/LocalOffer';
import ExpandMore from '@material-ui/icons/ExpandMore';
import { useHistory } from 'react-router-dom';
import SettingsIcon from '@material-ui/icons/Settings';
import TopBar from './TopBar';
import { useModal } from 'react-modal-hook';
import SettingsDialog from '../dialogs/SettingsDialog';
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart';
import AccountBalanceIcon from '@material-ui/icons/AccountBalance';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';

export const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
    root: {
        display: 'flex',
        flexDirection: 'column',
    },
    appBar: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerPaper: {
        width: drawerWidth,
        background: 'rgb(58, 59, 61)'
    },
    toolbar: theme.mixins.toolbar,
    content: {
        padding: theme.spacing(3),
        paddingTop: 8,
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
    },
    list: {
        color: '#FFF'
    },
    icon: {
        color: '#FFF'
    },
    divider: {
        backgroundColor: 'rgba(255,255,255 , 0.2)'
    },
    logoWrapper: {
        color: '#3b93f7',
        textTransform: 'uppercase',
        fontWeight: 500,
        fontSize: 32,
        textAlign: 'center',
        marginTop: 26,
        marginBottom: 26,
        userSelect: 'none',
        cursor: 'pointer',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    }
}));


function SideMenu(props) {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <CssBaseline />
            <TopBar />
            <Drawer
                className={classes.drawer}
                variant="permanent"
                classes={{
                    paper: classes.drawerPaper,
                }}
                anchor="left"
            >
                <div className={classes.logoWrapper}>
                    <img width={74} src="/Content/images/inventory.svg" />
                    <div>
                        STOCMA
                    </div>
                </div>
                <Divider className={classes.divider} />
                <MenuItems />
            </Drawer>
            <main className={classes.content}>
                {props.children}
            </main>
        </div>
    );
}


const MenuItems = () => {
    const [showSettingSideMenu, hideSettingSideMenu] = useModal(({ in: open, onExited }) => (
        <SettingsDialog open={open} onExited={onExited} onClose={hideSettingSideMenu} />
    ));
    const [openAccounts, setOpenAccounts] = React.useState(false);
    const [openSettings, setOpenSettings] = React.useState(false);
    const [openSales, setOpenSales] = React.useState(false);
    const [openSituations, setOpenSituations] = React.useState(false);
    const [openPurchases, setOpenPurchases] = React.useState(false);
    const [openStockArticles, setOpenStockArticles] = React.useState(false);
    const classes = useStyles();
    const history = useHistory();

    return (
        <>
            <List className={classes.list}>
                <ListItem button onClick={() => history.replace('/')}>
                    <ListItemIcon><DashboardIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Tableau du bord" />
                </ListItem>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenAccounts(!openAccounts)}>
                    <ListItemIcon><PeopleAltIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Comptes" />
                    {openAccounts ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openAccounts} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button onClick={() => history.replace('/ClientList')}>
                            <ListItemIcon />
                            <ListItemText primary="Clients" />
                        </ListItem>
                        <ListItem button onClick={() => history.replace('/SupplierList')}>
                            <ListItemIcon />
                            <ListItemText primary="Fournisseurs" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenSales(!openSales)}>
                    <ListItemIcon><LocalOfferIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Ventes" />
                    {openSales ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openSales} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button className={classes.nested} onClick={() => history.push('/BonLivraison')}>
                            <ListItemIcon />
                            <ListItemText primary="Bon de livraison" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/_Facture')}>
                            <ListItemIcon />
                            <ListItemText primary="Facture" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/Devis')}>
                            <ListItemIcon />
                            <ListItemText primary="Devis" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/bon-avoir-vente')}>
                            <ListItemIcon />
                            <ListItemText primary="Avoir" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/suivi-des-ventes')}>
                            <ListItemIcon />
                            <ListItemText primary="Suivi" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenPurchases(!openPurchases)}>
                    <ListItemIcon><ShoppingCartIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Achats" />
                    {openPurchases ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openPurchases} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button className={classes.nested} onClick={() => history.push('/BonReception')}>
                            <ListItemIcon />
                            <ListItemText primary="Bon de réception" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/_FactureAchat')}>
                            <ListItemIcon />
                            <ListItemText primary="Facture" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/BonCommande')}>
                            <ListItemIcon />
                            <ListItemText primary="Commande" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/bon-avoir-achat')}>
                            <ListItemIcon />
                            <ListItemText primary="Avoir" />
                        </ListItem>
                        <ListItem button className={classes.nested} onClick={() => history.push('/suivi-des-achats')}>
                            <ListItemIcon />
                            <ListItemText primary="Suivi" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenStockArticles(!openStockArticles)}>
                    <ListItemIcon><LocalMallIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Stock" />
                    {openStockArticles ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openStockArticles} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button onClick={() => history.replace('/ArticleList')}>
                            <ListItemIcon />
                            <ListItemText primary="Articles" />
                        </ListItem>
                        <ListItem button onClick={() => history.replace('/_ArticleList')}>
                            <ListItemIcon />
                            <ListItemText primary="Articles (Facture)" />
                        </ListItem>
                        <ListItem button onClick={() => history.replace('/mouvement-stock')}>
                            <ListItemIcon />
                            <ListItemText primary="Mouvements" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenSituations(!openSituations)}>
                    <ListItemIcon><AccountBalanceIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Situation" />
                    {openSituations ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openSituations} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button onClick={() => history.replace('/liste-paiements-des-clients')}>
                            <ListItemIcon />
                            <ListItemText primary="Clients" />
                        </ListItem>
                        <ListItem button onClick={() => history.replace('/liste-paiements-des-fournisseurs')}>
                            <ListItemIcon />
                            <ListItemText primary="Fournisseurs" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <List className={classes.list}>
                <ListItem button onClick={() => setOpenSettings(!openSettings)}>
                    <ListItemIcon><SettingsIcon className={classes.icon} /></ListItemIcon>
                    <ListItemText primary="Paramétrage" />
                    {openSettings ? <ExpandLess /> : <ExpandMore />}
                </ListItem>
                <Collapse in={openSettings} timeout="auto" unmountOnExit>
                    <List component="div" disablePadding>
                        <ListItem button onClick={showSettingSideMenu}>
                            <ListItemIcon />
                            <ListItemText primary="Documents" />
                        </ListItem>
                        <ListItem button onClick={() => history.replace('/SiteList')}>
                            <ListItemIcon />
                            <ListItemText primary="Dépôts/Magasins" />
                        </ListItem>
                    </List>
                </Collapse>
            </List>
            <Divider className={classes.divider} />
            <Box mt="auto">
                <Divider className={classes.divider} />
                <List className={classes.list}>
                    <ListItem button onClick={() => document.getElementById('logoutForm')?.submit()}>
                        <ListItemIcon><ExitToAppIcon className={classes.icon} style={{transform: 'scaleX(-1)'}} /></ListItemIcon>
                        <ListItemText primary="Sortir" />
                    </ListItem>
                </List>
            </Box>

        </>
    );
}

export default SideMenu;