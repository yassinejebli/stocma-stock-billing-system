import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Drawer, Avatar } from '@material-ui/core';
import AddShoppingCartIcon from '@material-ui/icons/AddShoppingCart';
import GroupAddOutlinedIcon from '@material-ui/icons/GroupAddOutlined';
import AccountBalanceWalletOutlinedIcon from '@material-ui/icons/AccountBalanceWalletOutlined';
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';
import ArticleForm from '../forms/ArticleForm';
import ClientForm from '../forms/ClientForm';
import FournisseurForm from '../forms/FournisseurForm';
import PaiementClientForm from '../forms/PaiementClientForm';
import PaiementFactureClientForm from '../forms/PaiementFactureClientForm';
import SiteForm from '../forms/SiteForm';
import PaiementFournisseurForm from '../forms/PaiementFournisseurForm';
import PaiementFactureFournisseurForm from '../forms/PaiementFactureFournisseurForm';
import ArticleCategoriesForm from '../forms/ArticleCategoriesForm';
import FakeArticleForm from '../forms/FakeArticleForm';
import { useSite } from '../../providers/SiteProvider';
import { useAuth } from '../../providers/AuthProvider';
import { useSettings } from '../../providers/SettingsProvider';
import CategoryIcon from '@material-ui/icons/Category';

const border = '1px solid #d8d8d8';

//elements
const items = {
    article: 'article',
    fakeArticle: 'fakeArticle',
    fournisseur: 'fournisseur',
    client: 'client',
    paiementClient: 'paiementClient',
    paiementFournisseur: 'paiementFournisseur',
    site: 'site',
    categorie: 'categorie',
}

export const useStyles = makeStyles(theme => ({
    root: {
        padding: '26px',
        display: 'flex',
        flexDirection: 'column',
        width: 400
    },
    header: {
        fontSize: 28,
        marginBottom: 32
    },
    item: {
        display: 'flex',
        alignItems: 'center',
        padding: '20px 0',
        borderTop: border,
        cursor: 'pointer',
        '&:hover .MuiAvatar-root': {
            backgroundColor: '#22496f'
        }
    },
    content: {
        marginLeft: 12,
        maxWidth: 400
    },
    title: {
        fontWeight: 500,
        fontSize: 16
    },
    description: {
        marginTop: 6,
        opacity: 0.8
    },
    avatar: {
        height: 100,
        width: 100,
        backgroundColor: '#7290af'
    },
    icon: {
        height: 50,
        width: 50
    }
}));

const SideWrapperDialog = (props) => {
    const [selectedItem, setSelectedItem] = React.useState(null);
    const {useVAT} = useSite();
    React.useEffect(() => {
        setSelectedItem(null);
    }, [props.onExited])

    const getForm = () => {
        console.log({selectedItem})
        switch (selectedItem) {
            case items.article:
                return <ArticleForm />;
            case items.fakeArticle:
                return <FakeArticleForm />;
            case items.client:
                return <ClientForm />;
            case items.fournisseur:
                return <FournisseurForm />;
            case items.paiementClient:
                return useVAT ? <PaiementFactureClientForm /> : <PaiementClientForm />;
            case items.paiementFournisseur:
                return useVAT ? <PaiementFactureFournisseurForm /> : <PaiementFournisseurForm />;
            case items.site:
                return <SiteForm />;
            case items.categorie:
                return <ArticleCategoriesForm />;
            default:
                return null;
        }
    }

    return (
        <SideDialogWrapper {...props}>
            {!selectedItem && <Menu open={(item) => setSelectedItem(item)} />}
            {getForm()}
        </SideDialogWrapper>
    )
}

export const SideDialogWrapper = ({ children, onExited, ...props }) => {
    const classes = useStyles();
    return (
        <Drawer anchor="right" {...props}>
            <div className={classes.root}>
                {children}
            </div>
        </Drawer>
    )
}

const Menu = ({ open }) => {
    const {
        siteModule,
        paymentModule
    } = useSettings();
    const classes = useStyles();
    const { useVAT } = useSite();
    const {
        canManageClients,
        canManageFournisseurs,
        canManageArticles,
        canManageSites,
        canManagePaiementsClients,
        canManagePaiementsFournisseurs,
    } = useAuth();
    return (
        <>
            <div className={classes.header}>Ajouter un élément</div>
            { paymentModule?.Enabled && canManagePaiementsClients && <div className={classes.item} onClick={() => open(items.paiementClient)}>
                <Avatar className={classes.avatar}>
                    <AccountBalanceWalletOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Paiement (Client)</div>
                    <div className={classes.description}>Ajouter des paiements effectués par vos clients</div>
                </div>
            </div>}
            {paymentModule?.Enabled && canManagePaiementsFournisseurs && <div className={classes.item} onClick={() => open(items.paiementFournisseur)}>
                <Avatar className={classes.avatar}>
                    <AccountBalanceWalletOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Paiement (Fournisseur)</div>
                    <div className={classes.description}>Ajouter des paiements que vous avez effectués à vos fournisseurs</div>
                </div>
            </div>}
            {canManageArticles && <>
                <div className={classes.item} onClick={() => open(items.article)}>
                    <Avatar className={classes.avatar}>
                        <AddShoppingCartIcon className={classes.icon} />
                    </Avatar>
                    <div className={classes.content}>
                        <div className={classes.title}>Article</div>
                        <div className={classes.description}>Les produits que vous achetez / vendez et que vous suivez leurs quantités</div>
                    </div>
                </div>
                {
                    !useVAT && <div className={classes.item} onClick={() => open(items.fakeArticle)}>
                        <Avatar className={classes.avatar}>
                            <AddShoppingCartIcon className={classes.icon} />
                        </Avatar>
                        <div className={classes.content}>
                            <div className={classes.title}>Article (Factures)</div>
                            <div className={classes.description}>Les produits que vous achetez / vendez et que vous suivez leurs quantités</div>
                        </div>
                    </div>
                }
            </>}
            {canManageClients && <div className={classes.item} onClick={() => open(items.client)}>
                <Avatar className={classes.avatar}>
                    <GroupAddOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Client</div>
                    <div className={classes.description}>Ajouter un nouveau client</div>
                </div>
            </div>}
            {canManageFournisseurs && <div className={classes.item} onClick={() => open(items.fournisseur)}>
                <Avatar className={classes.avatar}>
                    <GroupAddOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Fournisseur</div>
                    <div className={classes.description}>Ajouter un nouveau fournisseur</div>
                </div>
            </div>}
            {canManageArticles && <div className={classes.item} onClick={() => open(items.categorie)}>
                <Avatar className={classes.avatar}>
                    <CategoryIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Famille</div>
                    <div className={classes.description}>Ajouter une nouvelle famille</div>
                </div>
            </div>}
            {canManageSites && siteModule?.Enabled && <div className={classes.item} onClick={() => open(items.site)}>
                <Avatar className={classes.avatar}>
                    <StorefrontOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Dépôt/Magasin</div>
                    <div className={classes.description}>Ajouter un nouveau dépôt ou magasin</div>
                </div>
            </div>}
        </>
    )
}

export default SideWrapperDialog