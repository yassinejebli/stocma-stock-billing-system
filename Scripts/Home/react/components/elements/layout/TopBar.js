import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import { makeStyles } from '@material-ui/core/styles';
import MenuIcon from '@material-ui/icons/Menu';
import MoreIcon from '@material-ui/icons/MoreVert';
import AddIcon from '@material-ui/icons/Add';
import { useModal } from 'react-modal-hook';
import SideWrapperDialog from '../dialogs/SideWrapperDialog';
import { useTitle } from '../../providers/TitleProvider';
import SiteSelect from '../site-select/SiteSelect';
import { drawerWidth } from './SideMenu';
import SettingsDialog from '../dialogs/SettingsDialog';

const useStyles = makeStyles((theme) => ({
    root: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    toolbar: {
        display: 'flex',
        justifyContent: 'space-between',
    },
    titleWrapper: {
        display: 'flex'
    },
    title: {
        alignSelf: 'center',
        fontSize: 16
    }
}));

export default function TopBar() {
    const { title } = useTitle();
    const classes = useStyles();
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideWrapperDialog open={open} onExited={onExited} onClose={hideModal} />
    ));
    // const [showModalSideMenu, hideModalSideMenu] = useModal(({ in: open, onExited }) => (
    // ));
    const [showSettingSideMenu, hideSettingSideMenu] = useModal(({ in: open, onExited }) => (
        <SettingsDialog open={open} onExited={onExited} onClose={hideSettingSideMenu} />
    ));

    return (
        <>
            <AppBar position="sticky" className={classes.root}>
                <Toolbar className={classes.toolbar} variant="dense">
                    <div className={classes.titleWrapper}>
                        <IconButton
                            edge="start"
                            className={classes.menuButton}
                            color="inherit"
                            onClick={null}
                        >
                            <MenuIcon />
                        </IconButton>
                        <div className={classes.title}>
                            {title}
                        </div>
                    </div>
                    <SiteSelect />
                    <div>
                        <IconButton onClick={showModal} color="inherit">
                            <AddIcon />
                        </IconButton>
                        <IconButton onClick={showSettingSideMenu} edge="end" color="inherit">
                            <MoreIcon />
                        </IconButton>
                    </div>
                </Toolbar>
            </AppBar>
        </>
    );
}
