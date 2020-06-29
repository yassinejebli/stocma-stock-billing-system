import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Backdrop, CircularProgress, LinearProgress } from '@material-ui/core';

export const useStyles = makeStyles(theme => ({
    root: {
        zIndex: 10000
    }
}));

const Loader = ({ loading }) => {
    const classes = useStyles();

    return (
        <Backdrop className={classes.root} open={loading}>
            <CircularProgress size={76} style={{ color: 'rgb(66, 135, 245)' }} />
        </Backdrop>
    )
}

export const LinearLoader = ({ loading }) => {
    const classes = useStyles();
    if (!loading)
        return null;
    return (
        <div className={classes.root} style={{
            position: 'fixed',
            width: '100%',
            top: 0
        }}>
            <LinearProgress size={76} style={{ color: 'rgb(66, 135, 245)' }} />
        </div>
    )
}

export default Loader