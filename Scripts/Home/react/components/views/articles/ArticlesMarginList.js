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


const ArticlesMarginList = () => {
    const { siteId } = useSite();
    const { setTitle } = useTitle();
    const [data, setData] = React.useState([]);
    const [searchText, setSearchText] = React.useState('');
    const [loading, setLoading] = React.useState(false);
    const columns = React.useMemo(
        () => articlesMarginColumns(),
        []
    )

    React.useEffect(() => {
        setTitle('Marges & Articles')
        setLoading(true);
        getMarginArticles(siteId)
            .then(res => setData(res))
            .finally(() => setLoading(false));
    }, []);

    React.useEffect(()=>{

    }, [searchText]);

    return (
        <>
            <Loader loading={loading} />
            {/* <Box my={2} display="flex" justifyContent="center">
                <ArticlesStatistics />
            </Box> */}
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Marge des articles" Icon={LocalMallOutlinedIcon} />
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
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                    />
                </Box>
            </Paper>
        </>
    )
}

export default ArticlesMarginList;
