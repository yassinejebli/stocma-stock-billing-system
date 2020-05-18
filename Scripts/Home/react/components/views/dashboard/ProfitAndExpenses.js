import React from 'react';
import Chart from "react-apexcharts";
import { makeStyles, Box, useTheme } from '@material-ui/core';
import { useSite } from '../../providers/SiteProvider';
import { getProfitAndTurnover } from '../../../queries/DashboardQueries';

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
        fontSize: 26,
        fontFamily: 'Helvetica, Arial, sans-serif',
        textAnchor: 'start',
        dominantBaseline: 'auto',
        lineHeight: 0.9
    },
    text: {
        fontSize: 16,
    }
}));

const ProfitAndExpenses = () => {
    const theme = useTheme();
    const {siteId} = useSite();
    const classes = useStyles();
    const [data, setData] = React.useState();
    const [totalProfit, setTotalProfit] = React.useState();
    const [totalTurnover, setTotalTurnover] = React.useState();
    React.useEffect(() => {
        loadProfitAndExpenses();
    }, []);

    const loadProfitAndExpenses = () => {
        getProfitAndTurnover(siteId).then(res=>{
            console.log({res})
            setData({
                options: {
                    stroke: {
                        show: true,
                        width: 4,
                        dashArray: 0,
                        opacity: 0.4
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
                        tooltip: {
                            enabled: false
                        },
                        categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999],
                        axisBorder: {
                            show: false
                        },
                        axisTicks: {
                            show: false,
                        }
                    },
                    yaxis: {
                        labels: {
                            show: false
                        },
                    }
                },
                series: [
                    {
                        name: "series-1",
                        data: [30, 40, 45, 50, 49, 60, 70, 91]
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
                        255 000 DH
                    </div>
                    <div className={classes.text}>
                        Ventes
                    </div>
                </div>
                <Box height={80}>
                    {data && <Chart
                        options={data.options}
                        series={data.series}
                        type="area"
                        height="100%"
                        width="100%"
                    />}
                </Box>
            </div>
            <div className={classes.card}>
                <div className={classes.textWrapper}>
                    <div className={classes.amount}>
                        100 000 DH
                    </div>
                    <div className={classes.text}>
                        Bénéfices
                    </div>
                </div>
                <Box height={80}>
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
    )
}

export default ProfitAndExpenses;
