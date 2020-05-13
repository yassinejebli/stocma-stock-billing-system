import React from 'react'
import makeStyles from '@material-ui/core/styles/makeStyles';
import { formatMoney } from '../../../utils/moneyUtils';

const useStyles = makeStyles(theme => ({
    root: {

    },
    flex: {
        marginLeft: 'auto',
        marginTop: 32,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'flex-end',
    },
    text: {
        fontSize: 18,
        fontWeight: 500,
        marginRight: 120

    },
    smallText: {
        fontSize: 16,
        marginRight: 120,
    },
    total: {
        fontSize: 28,
        fontWeight: 600,
    },
    discount: {
        fontSize: 28
    }
}));

const TotalText = ({ total = 0, discount = 0 }) => {
    const classes = useStyles();
    return <div className={classes.root}>
        {discount > 0 ? <div className={classes.flex}><div className={classes.smallText}>
            Total (sans remise)
        </div>
            <div className={classes.discount}>
                {formatMoney(total)}
            </div></div>
            : null
        }
        {discount > 0 ? <div className={classes.flex}><div className={classes.smallText}>
            Remise
        </div>
            <div className={classes.discount}>
                -{formatMoney(discount)}
            </div></div>
            : null
        }
        <div className={classes.flex}>
            <div className={classes.text}>
                Total
        </div>
            <div className={classes.total}>
                {formatMoney(total - discount)}
            </div>
        </div>
    </div>
}

export default TotalText