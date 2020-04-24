import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import Dialog from '@material-ui/core/Dialog';

export const useStyles = makeStyles(theme => ({
    root: {
        height: '100%',
        minHeight: 600,
        width: '100%',
        overflow:'hidden'
    }
}));

const IframeDialog = ({src, children, ...props}) => {
    const classes = useStyles();
    
    return (
        <Dialog open fullWidth {...props}>
            {children}
            <iframe id="iframe-dialog" src={src} frameBorder="0" className={classes.root} />
        </Dialog>
    )
}

export default IframeDialog