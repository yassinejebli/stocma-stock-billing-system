import React from 'react'
import makeStyles from '@material-ui/core/styles/makeStyles';
import { formatMoney } from '../../../utils/moneyUtils';

const useStyles = makeStyles(theme => ({
    root: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'flex-end',
        marginLeft: 'auto',
        marginTop: 32
    },
    text: {
        fontSize: 18,
        fontWeight: 500,
        marginRight: 120

    },
    total: {
        fontSize: 28,
        fontWeight: 600,
    }
}));

const TotalText = ({ total = 0 }) => {
    const classes = useStyles();
    return <div className={classes.root}>
        <div className={classes.text}>
            Total
        </div>
        <div className={classes.total}>
            {formatMoney(total)}
        </div>
    </div>
}

export default TotalText