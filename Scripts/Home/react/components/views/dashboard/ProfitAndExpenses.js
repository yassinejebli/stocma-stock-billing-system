import React from 'react';
import Chart from "react-apexcharts";
import { makeStyles, Box, useTheme } from '@material-ui/core';
import { useSite } from '../../providers/SiteProvider';
import { getDailyProfitAndTurnover } from '../../../queries/DashboardQueries';
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
        width: '30%',
        backgroundColor: '#FFF',
        boxShadow: '0px 1px 22px -12px #607D8B',
        height: 'fit-content'
    },
    textWrapper: {
        paddingLeft: 28
    },
    amount: {
        fontWeight: 600,
        fontSize: 22,
        fontFamily: 'Helvetica, Arial, sans-serif',
        textAnchor: 'start',
        dominantBaseline: 'auto',
        lineHeight: 0.9
    },
    text: {
        fontSize: 14,
    }
}));

const ProfitAndExpenses = () => {
    const theme = useTheme();
    const { siteId } = useSite();
    const classes = useStyles();
    const [turnoverData, setTurnoverData] = React.useState();
    const [profitData, setProfitData] = React.useState();
    const [expensesData, setExpensesData] = React.useState();
    const [totalProfit, setTotalProfit] = React.useState();
    const [totalExpenses, setTotalExpenses] = React.useState();
    const [totalTurnover, setTotalTurnover] = React.useState();
    React.useEffect(() => {
        loadProfitAndExpenses();
    }, []);

    const loadProfitAndExpenses = () => {
        const today = new Date();
        getDailyProfitAndTurnover(siteId).then(res => {
            const categories = res.map(x => {
                const date = new Date(today.getFullYear(), today.getMonth(), x.day);
                return format(date, 'EEEE dd MMM Y', { locale: fr });
            });

            const defaultOptions = {
                options: {
                    stroke: {
                        show: true,
                        width: 4,
                        dashArray: 0,
                        opacity: 0.4,
                        curve: 'straight',
                    },
                    grid: {
                        show: false,
                        padding: {
                            left: 0,
                            right: 0
                        }
                    },
                    chart: {
                        id: "basic-bar",
                        toolbar: false,
                        sparkline: {
                            enabled: true
                        }
                    },
                    xaxis: {
                        labels: {
                            show: false
                        },
                        categories,
                        tooltip: {
                            enabled: false
                        },
                        axisBorder: {
                            show: false
                        },
                        axisTicks: {
                            show: false,
                        }
                    },
                    yaxis: {
                        labels: {
                            formatter: function (value) {
                                return formatMoney(value) + ' DH'
                            }
                        },
                    }
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
            setTotalExpenses(res.reduce((sum, curr) => {
                sum += curr.expense;
                return sum;
            }, 0));

            setExpensesData({
                ...defaultOptions,
                series: [
                    {
                        name: "Dépenses",
                        data: res.map(x => x.expense)
                    }
                ]
            });

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
                series: [
                    {
                        name: "Bénéfices",
                        data: res.map(x => x.profit)
                    }
                ]
            });
        });
    }

    return (
        <div className={classes.wrapper}>
            <div className={classes.card}>
                <div className={classes.textWrapper}>
                    <div className={classes.amount}>
                        {formatMoney(totalTurnover)} DH
                    </div>
                    <div className={classes.text}>
                        Ventes
                    </div>
                </div>
                <Box height={80}>
                    {turnoverData && <Chart
                        options={turnoverData.options}
                        series={turnoverData.series}
                        type="area"
                        height="100%"
                        width="100%"
                    />}
                </Box>
            </div>
            <div className={classes.card}>
                <div className={classes.textWrapper}>
                    <div className={classes.amount}>
                        {formatMoney(totalExpenses)} DH
                    </div>
                    <div className={classes.text}>
                        Dépenses
                    </div>
                </div>
                <Box height={80}>
                    {expensesData && <Chart
                        options={expensesData.options}
                        series={expensesData.series}
                        type="area"
                        height="100%"
                        width="100%"
                    />}
                </Box>
            </div>
            <div className={classes.card}>
                <div className={classes.textWrapper}>
                    <div className={classes.amount}>
                        {formatMoney(totalProfit)} DH
                    </div>
                    <div className={classes.text}>
                        Bénéfices
                    </div>
                </div>
                <Box height={80}>
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

export default ProfitAndExpenses;
