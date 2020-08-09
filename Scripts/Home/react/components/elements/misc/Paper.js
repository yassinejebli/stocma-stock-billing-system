import React from 'react'
import MuiPaper from '@material-ui/core/Paper'
import makeStyles from '@material-ui/core/styles/makeStyles';

const useStyles = makeStyles(theme=>({
    root: {
        padding: '42px 26px',
        backgroundColor: 'rgba(255,255,255,0.84)',
    },
}));

const Paper = ({children, ...props}) => {
    const classes = useStyles();
    return <MuiPaper className={classes.root} {...props}>
        {children}
    </MuiPaper>
}

export default Paper