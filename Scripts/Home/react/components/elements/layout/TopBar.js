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

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginBottom: 26
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
        alignSelf: 'center'
    }
}));

export default function TopBar() {
    const { title } = useTitle();
    const classes = useStyles();
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideWrapperDialog open={open} onExited={onExited} onClose={hideModal} />
    ));

    return (
        <AppBar position="sticky" className={classes.root}>
            <Toolbar className={classes.toolbar} variant="dense">
                <div className={classes.titleWrapper}>
                    <IconButton
                        edge="start"
                        className={classes.menuButton}
                        color="inherit"
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
                    <IconButton edge="end" color="inherit">
                        <MoreIcon />
                    </IconButton>
                </div>
            </Toolbar>
        </AppBar>
    );
}
