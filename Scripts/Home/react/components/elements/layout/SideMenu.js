import React from 'react';
import { SwipeableDrawer, ListItemIcon, ListItem, List, ListItemText, Divider, makeStyles } from '@material-ui/core';
import DashboardIcon from '@material-ui/icons/Dashboard';
import PeopleAltIcon from '@material-ui/icons/PeopleAlt';
import LocalMallIcon from '@material-ui/icons/LocalMall';
import { useHistory } from 'react-router-dom';


const useStyles = makeStyles({
    list: {
        minWidth: 250,
    }
});

const SideMenu = (props) => {
    const classes = useStyles();
    const history = useHistory();
    return (
        <SwipeableDrawer
            anchor="left"
            {...props}
        >
            <div
                className={classes.list}
                role="presentation"
                onClick={props.onClose}
                onKeyDown={props.onClose}
            >
                <List>
                    <ListItem button onClick={()=>history.replace('/')}>
                        <ListItemIcon><DashboardIcon /></ListItemIcon>
                        <ListItemText primary="Tableau du bord" />
                    </ListItem>
                    <ListItem button onClick={()=>history.replace('/ClientList')}>
                        <ListItemIcon><PeopleAltIcon /></ListItemIcon>
                        <ListItemText primary="Clients" />
                    </ListItem>
                    <ListItem button onClick={()=>history.replace('/SupplierList')}>
                        <ListItemIcon><PeopleAltIcon /></ListItemIcon>
                        <ListItemText primary="Fournisseurs" />
                    </ListItem>
                </List>
                <Divider />
                <List>
                    <ListItem button onClick={()=>history.replace('/ArticleList')}>
                        <ListItemIcon><LocalMallIcon /></ListItemIcon>
                        <ListItemText primary="Articles & Stock" />
                    </ListItem>
                </List>
            </div>
        </SwipeableDrawer>
    )
}

export default SideMenu;