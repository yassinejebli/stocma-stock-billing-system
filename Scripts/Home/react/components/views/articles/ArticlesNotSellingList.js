import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import TitleIcon from '../../elements/misc/TitleIcon'
import { TextField, FormControl, Select, MenuItem } from '@material-ui/core'
import { articlesNotSellingColumns } from '../../elements/table/columns/articleColumns'
import { useSite } from '../../providers/SiteProvider'
import { getArticlesNotSelling } from '../../../queries/articleQueries'
import { useTitle } from '../../providers/TitleProvider'
import useDebounce from '../../../hooks/useDebounce'
import { useLoader } from '../../providers/LoaderProvider'
import TrendingDownIcon from '@material-ui/icons/TrendingDown';

const ArticlesNotSellingList = () => {
    const { showLoader } = useLoader()
    const { siteId } = useSite();
    const fetchIdRef = React.useRef(0);
    const { setTitle } = useTitle();
    const [data, setData] = React.useState([]);
    const [months, setMonths] = React.useState(12);
    const [searchText, setSearchText] = React.useState('');
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const columns = React.useMemo(
        () => articlesNotSellingColumns(),
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
        setTitle('Les articles non-vendus')

    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            showLoader(true, true)
            getArticlesNotSelling(siteId, startRow, filters)
                .then(response => {
                    setData(response.data)
                    setTotalItems(response.totalItems)
                    setTotalCount(Math.ceil(response.totalItems / pageSize))
                }).finally(() => {
                    showLoader();
                });
        }
    }, [siteId])

    return (
        <>

            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder description="Trié par valeur (Qte en stock  x  P.A)" title="Les articles non-vendus" Icon={TrendingDownIcon} />
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
                <Box mt={3} width={160}>
                    <FormControl variant="outlined" size="small" fullWidth>
                        <Select
                            value={months}
                            fullWidth
                            onChange={({ target: { value } }) => {
                                setMonths(value);
                            }}
                        >
                            {
                                [1, 3, 6, 12, 24, 48].map(x => (
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

export default ArticlesNotSellingList;
