import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import TitleIcon from '../../elements/misc/TitleIcon'
import { TextField, FormControl, Select, MenuItem } from '@material-ui/core'
import { useTitle } from '../../providers/TitleProvider'
import useDebounce from '../../../hooks/useDebounce'
import { useLoader } from '../../providers/LoaderProvider'
import { getClientsNotBuying } from '../../../queries/clientQueries'
import { inputTypes } from '../../../types/input'
import { formatMoney } from '../../../utils/moneyUtils'
import PeopleOutlineIcon from '@material-ui/icons/PeopleOutline';

const ClientsNotBuyingList = () => {
    const { showLoader } = useLoader()
    const fetchIdRef = React.useRef(0);
    const { setTitle } = useTitle();
    const [data, setData] = React.useState([]);
    const [months, setMonths] = React.useState(3);
    const [searchText, setSearchText] = React.useState('');
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const columns = React.useMemo(
        () => clientsNotBuyingColumns(),
        []
    )
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            months,
            searchText: debouncedSearchText
        }
    }, [debouncedSearchText, months]);
    React.useEffect(() => {
        setTitle('Les clients non-fidèles')

    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            showLoader(true, true)
            getClientsNotBuying(startRow, filters)
                .then(response => {
                    setData(response.data)
                    setTotalItems(response.totalItems)
                    setTotalCount(Math.ceil(response.totalItems / pageSize))
                }).finally(() => {
                    showLoader();
                });
        }
    }, [])

    return (
        <>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Clients non-fidèles dans une période" Icon={PeopleOutlineIcon} />
                    <TextField
                        value={searchText}
                        onChange={({ target: { value } }) => {
                            setSearchText(value);
                        }}
                        placeholder="Rechercher..."
                        variant="outlined"
                        size="small"
                    />
                </Box>
                <Box width={150} mt={3}>
                    <FormControl variant="outlined" size="small" fullWidth>
                        <Select
                            value={months}
                            fullWidth
                            onChange={({ target: { value } }) => {
                                setMonths(value);
                            }}
                        >
                            {
                                [1, 3, 6, 12, 24].map(x => (
                                    <MenuItem key={x} value={x}>{x} Mois</MenuItem>
                                ))
                            }
                        </Select>
                    </FormControl>
                </Box>
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                        serverPagination
                        totalItems={totalItems}
                        pageCount={pageCount}
                        fetchData={fetchData}
                        filters={filters}
                    />
                </Box>
            </Paper>
        </>
    )
}


export const clientsNotBuyingColumns = () => ([
    {
        Header: 'Client',
        accessor: 'client',
        type: inputTypes.text.description,
        width: 180
    },
    {
        id: 'solde',
        Header: 'Solde',
        accessor: (props) => {
            return formatMoney(props.solde);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'turnover',
        Header: 'Total',
        accessor: (props) => {
            return formatMoney(props.turnover);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'margin',
        Header: 'Marge',
        accessor: (props) => {
            return formatMoney(props.margin);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
])

export default ClientsNotBuyingList;
