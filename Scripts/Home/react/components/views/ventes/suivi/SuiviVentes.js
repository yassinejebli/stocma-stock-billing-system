import Box from '@material-ui/core/Box'
import React from 'react'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { getSuiviVentesColumns } from '../../../elements/table/columns/suiviVentesColumns'
import Paper from '../../../elements/misc/Paper'
import TitleIcon from '../../../elements/misc/TitleIcon'
import { useTitle } from '../../../providers/TitleProvider';
import { getData } from '../../../../queries/crudBuilder';
import useDebounce from '../../../../hooks/useDebounce';
import Table from '../../../elements/table/Table';

const TABLE = 'BonLivraisonItems';

const EXPAND = ['Article', 'BonLivraison/Client'];

const SuiviVentes = () => {
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            or: [
                {
                    'BonLivraison/Client/Name': {
                        contains: debouncedSearchText
                    }
                },
                {
                    'BonLivraison/NumBon': {
                        contains: debouncedSearchText
                    }
                },
            ]
        }
    }, [debouncedSearchText]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getSuiviVentesColumns(),
        []
    )

    React.useEffect(() => {
        setTitle('Suivi des ventes')
    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
                $skip: startRow
            }, filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            });
        }
    }, [])

    return (
        <>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Suivi des ventes" Icon={DescriptionOutlinedIcon} />
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

export default SuiviVentes;
