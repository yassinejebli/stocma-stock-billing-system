import React from 'react';
import makeStyles from "@material-ui/core/styles/makeStyles";
import { Backdrop, CircularProgress } from '@material-ui/core';

export const useStyles = makeStyles(theme => ({
    root: {
        zIndex: 999
    }
}));

const Loader = ({loading}) => {
    const classes = useStyles();

    return (
        <Backdrop className={classes.root} open={loading}>
            <CircularProgress thickness={5} style={{color: 'rgb(66, 135, 245)'}} />
        </Backdrop>
    )
}

export default Loader