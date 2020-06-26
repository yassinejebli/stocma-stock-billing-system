import React from 'react';
import Chart from "react-apexcharts";
import { makeStyles, Box, useTheme } from '@material-ui/core';
import { useSite } from '../../providers/SiteProvider';
import { getMonthlyProfitAndTurnover } from '../../../queries/DashboardQueries';
import { format } from 'date-fns';
import { fr } from 'date-fns/locale';
import { formatMoney } from '../../../utils/moneyUtils';

const useStyles = makeStyles(theme => ({
    wrapper: {
        display: 'flex',
        justifyContent: 'space-between',
        marginTop: 16
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

const MonthlyProfitAndExpenses = () => {
    const theme = useTheme();
    const { siteId } = useSite();
    const classes = useStyles();
    const [turnoverData, setTurnoverData] = React.useState();
    const [profitData, setProfitData] = React.useState();
    const [totalProfit, setTotalProfit] = React.useState();
    const [totalTurnover, setTotalTurnover] = React.useState();
    React.useEffect(() => {
        loadMonthlyProfitAndExpenses();
    }, []);

    const loadMonthlyProfitAndExpenses = () => {
        const today = new Date();
        getMonthlyProfitAndTurnover(siteId).then(res => {
            const categories = res.map(x => {
                const date = new Date(today.getFullYear(), x.month - 1, 1);
                return format(date, 'MMM Y', { locale: fr });
            });

            const defaultOptions = {
                options: {
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
            setTotalProfit(res.reduce((sum, curr) => {
                sum += curr.profit;
                return sum;
            }, 0));

            setTurnoverData({
                ...defaultOptions,
                series: [
                    {
                        name: "Ventes",
                        data: res.map(x => x.turnover)
                    }
                ]
            });

            setProfitData({
                ...defaultOptions,
                series: [{
                    name: 'Ventes',
                    data: res.map(x => x.turnover)
                }, {
                    name: 'Bénéfices',
                    data: res.map(x => x.profit)
                }],
            });
        });
    }

    return (
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
                        <div className={classes.amount} style={{color: 'rgb(0, 227, 150)'}}>
                            {formatMoney(totalProfit)} DH
                        </div>
                        <div className={classes.text}>
                            Total des bénéfices
                        </div>
                    </div>
                </Box>
                <Box mt={1} minHeight={300}>
                    {profitData && <Chart
                        options={profitData.options}
                        series={profitData.series}
                        type="area"
                        height="100%"
                        width="100%"
                    />}
                </Box>
            </div>
        </div>
    )
}

export default MonthlyProfitAndExpenses;
