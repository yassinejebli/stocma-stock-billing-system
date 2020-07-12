import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import TitleIcon from '../../elements/misc/TitleIcon'
import LocalAtmIcon from '@material-ui/icons/LocalAtm';
import { TextField } from '@material-ui/core'
import { articlesMarginColumns } from '../../elements/table/columns/articleColumns'
import { useSite } from '../../providers/SiteProvider'
import { getMarginArticles } from '../../../queries/articleQueries'
import { useTitle } from '../../providers/TitleProvider'
import DatePicker from '../../elements/date-picker/DatePicker'
import useDebounce from '../../../hooks/useDebounce'
import { useLoader } from '../../providers/LoaderProvider'

const ArticlesMarginList = () => {
    const {showLoader} = useLoader()
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0,0,0,0);
    lastDayCurrentMonth.setHours(23,59,59,999);
    const { siteId } = useSite();
    const fetchIdRef = React.useRef(0);
    const { setTitle } = useTitle();
    const [data, setData] = React.useState([]);
    const [searchText, setSearchText] = React.useState('');
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const columns = React.useMemo(
        () => articlesMarginColumns(),
        []
    )
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            dateFrom,
            dateTo,
            searchText: debouncedSearchText
        }
    }, [debouncedSearchText, dateFrom, dateTo]);
    React.useEffect(() => {
        setTitle('Marge par article')
        
    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            showLoader(true, true)
            getMarginArticles(siteId, startRow, filters)
            .then(response => {
                setData(response.data)
                setTotalItems(response.totalItems)
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).finally(()=>{
                showLoader();
            });
        }
    }, [siteId])

    React.useEffect(() => {
        
    }, [searchText]);

    return (
        <>

            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Marge bénéficiaire par article" Icon={LocalAtmIcon} />
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
                <Box mt={3}>
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

export default ArticlesMarginList;
