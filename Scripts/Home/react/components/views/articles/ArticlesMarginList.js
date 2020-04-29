import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import TitleIcon from '../../elements/misc/TitleIcon'
import LocalMallOutlinedIcon from '@material-ui/icons/LocalMallOutlined'
import { TextField } from '@material-ui/core'
import { articlesMarginColumns } from '../../elements/table/columns/articleColumns'
import { useSite } from '../../providers/SiteProvider'
import { getMarginArticles } from '../../../queries/articleQueries'
import { useTitle } from '../../providers/TitleProvider'
import DatePicker from '../../elements/date-picker/DatePicker'


const ArticlesMarginList = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    const { siteId } = useSite();
    const { setTitle } = useTitle();
    const [data, setData] = React.useState([]);
    const [searchText, setSearchText] = React.useState('');
    const filteredData = data.filter(x=>x.Article.toLocaleLowerCase().includes(searchText.toLocaleLowerCase()))
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const [loading, setLoading] = React.useState(false);
    const columns = React.useMemo(
        () => articlesMarginColumns(),
        []
    )

    React.useEffect(() => {
        setTitle('Marges & Articles')
        setLoading(true);
        getMarginArticles(siteId, dateFrom, dateTo)
            .then(res => setData(res))
            .finally(() => setLoading(false));
    }, [siteId, dateFrom, dateTo]);

    React.useEffect(() => {
        
    }, [searchText]);

    return (
        <>
            <Loader loading={loading} />
            {/* <Box my={2} display="flex" justifyContent="center">
                <ArticlesStatistics />
            </Box> */}
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Marge bénéficiaire par article" Icon={LocalMallOutlinedIcon} />
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
                        data={filteredData}
                    />
                </Box>
            </Paper>
        </>
    )
}

export default ArticlesMarginList;
