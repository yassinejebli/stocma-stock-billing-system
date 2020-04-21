import React from 'react'
import MuiPaper from '@material-ui/core/Paper'
import makeStyles from '@material-ui/core/styles/makeStyles';

const useStyles = makeStyles(theme=>({
    root: {
        padding: '42px 26px'
    },
}));

const Paper = ({children, ...props}) => {
    const classes = useStyles();
    return <MuiPaper {...props} className={classes.root}>
        {children}
    </MuiPaper>
}

export default Paper