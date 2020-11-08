import React from 'react';
import Chart from "react-apexcharts";
import { makeStyles, Box, useTheme } from '@material-ui/core';
import { useSite } from '../../providers/SiteProvider';
import { getMonthlyProfitAndTurnover } from '../../../queries/DashboardQueries';
import { format } from 'date-fns';
import { fr } from 'date-fns/locale';
import { formatMoney } from '../../../utils/moneyUtils';
import TrendingUpIcon from '@material-ui/icons/TrendingUp';

const useStyles = makeStyles(theme => ({
    wrapper: {
        display: 'flex',
        justifyContent: 'space-between',
        marginTop: 16
    },
    titleWrapper: {
        marginTop: 16,
        display: 'flex',
        alignItems: 'center',
    },
    title: {
        fontWeight: 500,
        fontSize: 24,
        marginLeft: 4,
    },
    icon: {
        width: 30,
        height: 30,
        color: 'green',
    },
    card: {
        paddingTop: 26,
        width: '100%',
        backgroundColor: '#FFF',
        boxShadow: '0px 1px 22px -12px #607D8B',
    },
    textWrapper: {
    },
    amount: {
        fontWeight: 600,
        fontSize: 22,
        fontFamily: 'Helvetica, Arial, sans-serif',
        textAnchor: 'start',
        dominantBaseline: 'auto',
        lineHeight: 0.9,
        color: theme.palette.primary.main,
    },
    text: {
        fontSize: 14,
        // color: theme.palette.primary.main,
    }
}));

const MonthlyProfitAndExpenses = ({ year }) => {
    const { siteId } = useSite();
    const classes = useStyles();
    const [data, setData] = React.useState();
    const [totalNetProfit, setTotalNetProfit] = React.useState();
    const [totalProfit, setTotalProfit] = React.useState();
    const [totalTurnover, setTotalTurnover] = React.useState();
    React.useEffect(() => {
        loadMonthlyProfitAndExpenses(year);
    }, [year]);

    const loadMonthlyProfitAndExpenses = (year) => {
        setData(null);
        getMonthlyProfitAndTurnover(siteId, year).then(res => {
            const categories = res.map(x => {
                const date = new Date(year, x.month - 1, 1);
                return format(date, 'MMM Y', { locale: fr });
            });

            const defaultOptions = {
                options: {
                    dataLabels: {
                        enabled: true,
                        offsetX: -10,
                        formatter: function (val, opts) {
                            return formatMoney(val) + ' DH'
                        },
                        style: {
                            fontSize: '0.8rem',
                        },
                    },
                    stroke: {
                        show: true,
                        width: 4,
                        dashArray: 0,
                        opacity: 0.4,
                    },
                    grid: {
                        show: true,
                        padding: {
                            left: 0,
                            right: 0
                        }
                    },
                    chart: {
                        toolbar: {
                            show: false,
                            offsetX: 0,
                            offsetY: 0,
                            defaultLocale: 'fr'
                        },
                    },
                    xaxis: {
                        categories,
                        tooltip: {
                            enabled: false
                        },
                        axisBorder: {
                            show: true
                        },
                        axisTicks: {
                            show: true,
                        }
                    },
                    yaxis: {
                        labels: {
                            formatter: function (value) {
                                return formatMoney(value) + ' DH'
                            },
                            offsetX: -8,
                        },
                    },
                }
            };

            setTotalTurnover(res.reduce((sum, curr) => {
                sum += curr.turnover;
                return sum;
            }, 0));

            setTotalNetProfit(res.reduce((sum, curr) => {
                sum += curr.netProfit;
                return sum;
            }, 0));

            setTotalProfit(res.reduce((sum, curr) => {
                sum += curr.profit;
                return sum;
            }, 0));

            setData({
                ...defaultOptions,
                series: [{
                    name: 'Ventes',
                    data: res.map(x => x.turnover)
                }, {
                    name: 'Bénéfices net',
                    data: res.map(x => x.netProfit)
                },
                {
                    name: 'Bénéfices',
                    data: res.map(x => x.profit)
                }],
            });
        });
    }

    return (
        <>
            <div className={classes.titleWrapper}>
                <TrendingUpIcon className={classes.icon} />
                <div className={classes.title}>
                    Chiffre d'affaires & bénéfices
                </div>
            </div>
            <div className={classes.wrapper}>
                <div className={classes.card}>
                    <Box display="flex" justifyContent="space-around">
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} >
                                {formatMoney(totalTurnover)} DH
                        </div>
                            <div className={classes.text}>
                                Total des ventes
                        </div>
                        </div>
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} style={{ color: 'rgb(254, 176, 25)' }}>
                                {formatMoney(totalProfit)} DH
                        </div>
                            <div className={classes.text}>
                                Total des bénéfices
                        </div>
                        </div>
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} style={{ color: 'rgb(0, 227, 150)' }}>
                                {formatMoney(totalNetProfit)} DH
                        </div>
                            <div className={classes.text}>
                                Total des bénéfices net
                        </div>
                        </div>
                    </Box>
                    <Box mt={1} minHeight={300}>
                        {data && <Chart
                            options={data.options}
                            series={data.series}
                            type="area"
                            height="100%"
                            width="100%"
                        />}
                    </Box>
                </div>
            </div>
        </>
    )
}

export default MonthlyProfitAndExpenses;
