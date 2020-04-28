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
import SiteForm from '../forms/SiteForm';
import PaiementFournisseurForm from '../forms/PaiementFournisseurForm';

const border = '1px solid #d8d8d8';

//elements
const items = {
    article: 'article',
    fournisseur: 'fournisseur',
    client: 'client',
    paiementClient: 'paiementClient',
    paiementFournisseur: 'paiementFournisseur',
    site: 'site',
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
        fontWeight: 500
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

    React.useEffect(() => {
        setSelectedItem(null);
    }, [props.onExited])

    const getForm = () => {
        switch (selectedItem) {
            case items.article:
                return <ArticleForm />;
            case items.client:
                return <ClientForm />;
            case items.fournisseur:
                return <FournisseurForm />;
            case items.paiementClient:
                return <PaiementClientForm />;
            case items.paiementFournisseur:
                return <PaiementFournisseurForm />;
            case items.site:
                return <SiteForm />;
            default:
                null;
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
    const classes = useStyles();

    return (
        <>
            <div className={classes.header}>Ajouter un élément</div>
            <div className={classes.item} onClick={() => open(items.article)}>
                <Avatar className={classes.avatar}>
                    <AddShoppingCartIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Article</div>
                    <div className={classes.description}>Les produits que vous achetez / vendez et que vous suivez leurs quantités</div>
                </div>
            </div>
            <div className={classes.item} onClick={() => open(items.client)}>
                <Avatar className={classes.avatar}>
                    <GroupAddOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Client</div>
                    <div className={classes.description}>Ajouter un nouveau client</div>
                </div>
            </div>
            <div className={classes.item} onClick={() => open(items.fournisseur)}>
                <Avatar className={classes.avatar}>
                    <GroupAddOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Fournisseur</div>
                    <div className={classes.description}>Ajouter un nouveau fournisseur</div>
                </div>
            </div>
            <div className={classes.item} onClick={() => open(items.paiementClient)}>
                <Avatar className={classes.avatar}>
                    <AccountBalanceWalletOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Paiement (Client)</div>
                    <div className={classes.description}>Ajouter des paiements effectués par vos clients</div>
                </div>
            </div>
            <div className={classes.item} onClick={() => open(items.paiementFournisseur)}>
                <Avatar className={classes.avatar}>
                    <AccountBalanceWalletOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Paiement (Fournisseur)</div>
                    <div className={classes.description}>Ajouter des paiements que vous avez effectués à vos fournisseurs</div>
                </div>
            </div>
            <div className={classes.item} onClick={() => open(items.site)}>
                <Avatar className={classes.avatar}>
                    <StorefrontOutlinedIcon className={classes.icon} />
                </Avatar>
                <div className={classes.content}>
                    <div className={classes.title}>Dépôt/Magasin</div>
                    <div className={classes.description}>Ajouter un nouveau dépôt ou magasin</div>
                </div>
            </div>
        </>
    )
}

export default SideWrapperDialog