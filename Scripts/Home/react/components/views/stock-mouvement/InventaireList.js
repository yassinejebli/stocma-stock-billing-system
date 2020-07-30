import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import TitleIcon from '../../elements/misc/TitleIcon'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined'
import { TextField, Button, IconButton } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import AddIcon from '@material-ui/icons/Add'
import { useSite } from '../../providers/SiteProvider'
import { inputTypes } from '../../../types/input'
import { format } from 'date-fns';
import VisibilityIcon from '@material-ui/icons/Visibility';

const DOCUMENT = 'Inventaires'
const EXPAND = ['Site','InventaireItems']

const InventaireList = () => {
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            and: [
                {
                    or: {
                        'Titre': {
                            contains: debouncedSearchText
                        },
                    }
                }
            ]
        }
    }, [debouncedSearchText, siteId]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const history = useHistory();
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => inventaireListColumns(),
        []
    );

    React.useEffect(() => {
        setTitle('Liste des inventaires')
    }, []);

    const viewInventory = React.useCallback(async (id) => {
        history.push(`inventaire?InventaireId=${id}`);
    }, []);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        // setPageIndex(pageIndex);
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            // const endRow = startRow + pageSize
            getData(DOCUMENT, {
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
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => history.push('/inventaire')}
                >
                    Nouvel inventaire
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des inventaires" Icon={DescriptionOutlinedIcon} />
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
                        customAction={viewInventory}
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


const inventaireListColumns = () => ([
    // {
    //     Header: 'Magasin',
    //     accessor: 'Site.Name',
    //     type: inputTypes.text.description,
    //     width: 120
    // },
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
        width: 60
    },
    {
        Header: 'Titre',
        accessor: 'Titre',
        type: inputTypes.text.description,
        width: 60
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => customAction(original.Id)}>
                        <VisibilityIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
])

export default InventaireList;
