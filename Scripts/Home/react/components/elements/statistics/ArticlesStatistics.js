import React from 'react'
import Paper from '../misc/Paper'
import LocalMallOutlinedIcon from '@material-ui/icons/LocalMallOutlined';
import RemoveCircleTwoToneIcon from '@material-ui/icons/RemoveCircleTwoTone';
import MonetizationOnTwoToneIcon from '@material-ui/icons/MonetizationOnTwoTone';
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';
import { Avatar, makeStyles, Box } from '@material-ui/core';
import { getLowStockCount, getTotalStockFacture, getTotalStock } from '../../../queries/articleQueries';
import { formatMoney } from '../../../utils/moneyUtils';
import { useSite } from '../../providers/SiteProvider';

const useStyles = makeStyles(theme => ({
    root: {
        padding: '32px 26px',
        marginLeft: 32,
        cursor: 'pointer',
        userSelect: 'none',
    },
    icon: {
        width: 56,
        height: 56,
        color: 'white',
    },
    block: {
        position: 'absolute',
        bottom: 20,
        left: 20
    },
    avatar: {
        backgroundColor: 'orange',
        padding: 48,
        position: 'relative',
        alignSelf: 'center'
    },
    avatarTotalStock: {
        backgroundColor: theme.palette.primary.main,
        padding: 48,
        position: 'relative',
        alignSelf: 'center'
    },
    number: {
        fontSize: 20,
        color: 'orange',
        fontWeight: 500
    },
    text: {
        fontSize: 14,
        textTransform: 'uppercase',
        color: '#393a3d',
        opacity: 0.8
    },
}));

const ArticlesStatistics = ({ onLowStockCountClick, lowStockSelected }) => {
    const [lowStockCount, setLowStockCount] = React.useState(0);
    const [totalStock, setTotalStock] = React.useState(0);
    const { siteId } = useSite();

    React.useEffect(() => {
        getLowStockCount(siteId).then(res => setLowStockCount(res));
        getTotalStock(siteId).then(res => setTotalStock(res));
    }, [siteId]);


    const classes = useStyles();
    return (
        <Box display="flex" justifyContent="space-around">
            <Paper className={classes.root} style={{
                backgroundColor: lowStockSelected ? '#eff4f7' : '#FFF'
            }}
                onClick={() => onLowStockCountClick ? onLowStockCountClick() : null}
            >
                <Box display="flex" flexDirection="column">
                    <div>
                        <Avatar className={classes.avatar}>
                            <LocalMallOutlinedIcon className={classes.icon} />
                            <RemoveCircleTwoToneIcon className={classes.block} style={{ color: 'red' }} />
                        </Avatar>
                        <div className={classes.number}>
                            {lowStockCount}
                        </div>
                        <div className={classes.text}>
                            Stocks faibles 
                        </div>
                    </div>
                </Box>
            </Paper>
            <Paper className={classes.root}>
                <Box display="flex" flexDirection="column">
                    <Avatar className={classes.avatarTotalStock}>
                        <StorefrontOutlinedIcon className={classes.icon} />
                        <MonetizationOnTwoToneIcon className={classes.block} style={{ color: 'orange' }} />
                    </Avatar>
                    <div className={classes.number}>
                        {formatMoney(totalStock)}
                    </div>
                    <div className={classes.text}>
                        Total du stock
                    </div>
                </Box>
            </Paper>
        </Box>
    )
}

export const ArticlesFactureStatistics = () => {
    const classes = useStyles();
    const [totalStock, setTotalStock] = React.useState(0);

    React.useEffect(() => {
        getTotalStockFacture().then(res => setTotalStock(res));
    }, []);

    return (
        <Box display="flex" justifyContent="space-around">
            <Paper className={classes.root}>
                <Box display="flex" flexDirection="column">
                    <Avatar className={classes.avatarTotalStock}>
                        <StorefrontOutlinedIcon className={classes.icon} />
                        <MonetizationOnTwoToneIcon className={classes.block} style={{ color: 'orange' }} />
                    </Avatar>
                    <div className={classes.number}>
                        {formatMoney(totalStock)}
                    </div>
                    <div className={classes.text}>
                        Total du stock
                    </div>
                </Box>
            </Paper>
        </Box>
    )
}

export default ArticlesStatistics;
