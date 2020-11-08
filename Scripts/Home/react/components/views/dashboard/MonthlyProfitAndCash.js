import React from 'react';
import Chart from "react-apexcharts";
import { makeStyles, Box, useTheme } from '@material-ui/core';
import { useSite } from '../../providers/SiteProvider';
import { format } from 'date-fns';
import { fr } from 'date-fns/locale';
import { formatMoney } from '../../../utils/moneyUtils';
import { getMonthlyProfitAndCash } from '../../../queries/DashboardQueries';
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';

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
        color: 'gold',
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

const MonthlyProfitAndCash = ({ year }) => {
    const { siteId } = useSite();
    const classes = useStyles();
    const [data, setData] = React.useState();
    const [totalNetCash, setTotalNetCash] = React.useState();
    const [totalExpenses, setTotalExpenses] = React.useState();
    const [totalCash, setTotalCash] = React.useState();
    React.useEffect(() => {
        loadMonthlyProfitAndCash(year);
    }, [year]);

    const loadMonthlyProfitAndCash = (year) => {
        setData(null);
        getMonthlyProfitAndCash(siteId, year).then(res => {
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

            setTotalCash(res.reduce((sum, curr) => {
                sum += curr.cashProfit;
                return sum;
            }, 0));

            setTotalNetCash(res.reduce((sum, curr) => {
                sum += curr.netProfit;
                return sum;
            }, 0));

            setTotalExpenses(res.reduce((sum, curr) => {
                sum += curr.expenses;
                return sum;
            }, 0));

            setData({
                ...defaultOptions,
                series: [{
                    name: 'Espèce',
                    data: res.map(x => x.cashProfit)
                }, {
                    name: 'Espèce net',
                    data: res.map(x => x.netProfit)
                }, {
                    name: 'Dépense',
                    data: res.map(x => x.expenses)
                },
                ],
            });
        });
    }

    return (
        <>
            <div className={classes.titleWrapper}>
                <MonetizationOnIcon className={classes.icon} />
                <div className={classes.title}>
                    Liquidité en espèces
                </div>
            </div>
            <div className={classes.wrapper}>
                <div className={classes.card}>
                    <Box display="flex" justifyContent="space-around">
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} >
                                {formatMoney(totalCash)} DH
                        </div>
                            <div className={classes.text}>
                                Espèce
                        </div>
                        </div>
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} style={{ color: 'rgb(254, 176, 25)' }}>
                                {formatMoney(totalExpenses)} DH
                        </div>
                            <div className={classes.text}>
                                Dépense
                        </div>
                        </div>
                        <div className={classes.textWrapper}>
                            <div className={classes.amount} style={{ color: 'rgb(0, 227, 150)' }}>
                                {formatMoney(totalNetCash)} DH
                        </div>
                            <div className={classes.text}>
                                Espèce net
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

export default MonthlyProfitAndCash;
