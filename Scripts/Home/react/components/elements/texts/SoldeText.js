import React from 'react'
import makeStyles from '@material-ui/core/styles/makeStyles';
import { format } from 'date-fns';
import { formatMoney } from '../../../utils/moneyUtils';
import { useSnackBar } from '../../providers/SnackBarProvider';

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


const getBalanceBeforeDateAndRealBalance = async (params) => {
    const parsedParams = new URLSearchParams(params).toString();
    const URL = `/PaiementsData/getBalanceBeforeDateAndRealBalance?${parsedParams}`
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
        return [];
    }
}

const SoldeText = ({ clientId, date, refresh }) => {
    const { showSnackBar } = useSnackBar();
    const classes = useStyles();
    const [solde, setSolde] = React.useState(0)
    const [soldeBeforeDate, setSoldeBeforeDate] = React.useState(0)
    const [calculatedSolde, setCalculatedSolde] = React.useState(0)
    React.useEffect(() => {
        getBalanceBeforeDateAndRealBalance({
            id: clientId,
            date: date.toISOString()
        }).then(response => {
            setSoldeBeforeDate(response.soldeBeforeDate)
            setSolde(response.solde)
            setCalculatedSolde(response.calculatedSolde)
            if (response.calculatedSolde !== response.solde) {
                showSnackBar({
                    error: true,
                    text: 'Attention: Le solde réel doit être égal au solde calculé!'
                })
            }
        })
    }, [clientId, date, refresh])
    return <div className={classes.root}>
        <div className={classes.flex}><div className={classes.smallText}>
            Solde avant {format(date, 'dd/MM/yyyy')}
        </div>
            <div className={classes.discount}>
                {formatMoney(soldeBeforeDate)}
            </div>
        </div>
        <div className={classes.flex}>
            <div className={classes.text}>
                Solde calculé
            </div>
            <div className={classes.total} style={{ color: (solde !== calculatedSolde) ? 'red' : 'black' }}>
                {formatMoney(calculatedSolde)}
            </div>
        </div>
        <div className={classes.flex}>
            <div className={classes.text}>
                Solde réel
            </div>
            <div className={classes.total}>
                {formatMoney(solde)}
            </div>
        </div>
    </div>
}

export default SoldeText