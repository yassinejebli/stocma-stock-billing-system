import React from 'react'
import makeStyles from '@material-ui/core/styles/makeStyles';
import red from '@material-ui/core/colors/red';

const useStyles = makeStyles(theme=>({
    root: {
        color: red[500],
        fontSize: 13,
        marginTop: 8
    },
}));

const Error = ({children, ...props}) => {
    const classes = useStyles();
    return <div {...props} className={classes.root}>
        {children}
    </div>
}

export default Error