import React from 'react'
import ExpansionPanel from '../../../elements/expansion/ExpansionPanel'
import { makeStyles, Box, Button } from '@material-ui/core'
import DatePicker from '../../../elements/date-picker/DatePicker';
import Chart from "react-apexcharts";
import { formatMoney } from '../../../../utils/moneyUtils';

const useStyles = makeStyles(theme => ({
    root: {

    },
}));


const VentesParUtilisateurs = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const [data, setData] = React.useState();

    React.useEffect(() => {
        getData(dateFrom, dateTo).then(res => {

            const categories = res.map(x => x.user);

            const defaultOptions = {
                options: {
                    plotOptions: {
                        bar: {
                            dataLabels: {
                                position: 'top',
                            },
                        }
                    },
                    dataLabels: {
                        enabled: false,
                    },
                    stroke: {
                        show: true,
                        width: 1,
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
                        },
                    },
                }
            };

            setData({
                ...defaultOptions,
                series: [{
                    name: 'Ventes',
                    data: res.map(x => x.totalSales)
                }, {
                    name: 'Bénéfices',
                    data: res.map(x => x.totalProfit)
                }],
            });
        })
    }, [dateFrom, dateTo])

    return (
        <ExpansionPanel title="Rapport des ventes / opérateur">
            <Box display="flex" flexDirection="column" width="100%">
                <Box width="100%">
                    <DatePicker
                        value={dateFrom}
                        label="Date de début"
                        onChange={(date) => {
                            date && date.setHours(0, 0, 0, 0);
                            setDateFrom(date)
                        }}
                    />
                    <DatePicker
                        style={{
                            marginLeft: 12
                        }}
                        value={dateTo}
                        label="Date de fin"
                        onChange={(date) => {
                            date && date.setHours(23, 59, 59, 999);
                            setDateTo(date)
                        }}
                    />
                </Box>
                <Box width="100%" minHeight={300}>
                    {data && <Chart
                        options={data.options}
                        series={data.series}
                        type="bar"
                        height="100%"
                        width="100%"
                    />}
                </Box>
            </Box>
        </ExpansionPanel>
    )
}
const BASE_URL = '/UserStatistics/'

export const getData = async (dateFrom, dateTo) => {
    const parsedParams = new URLSearchParams({
        dateFrom: dateFrom?.toISOString(),
        dateTo: dateTo?.toISOString(),
    }).toString();
    const URL = BASE_URL + `SalesByUser?${parsedParams}`
    try {
        const res = await (await fetch(URL)).json();
        return res;
    } catch (e) {
        console.log(e);
    }
}

export default VentesParUtilisateurs;
