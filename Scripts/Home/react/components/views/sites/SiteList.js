import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData, getAllData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import SiteForm from '../../elements/forms/SiteForm'
import TitleIcon from '../../elements/misc/TitleIcon'
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';
import { TextField } from '@material-ui/core'
import useDebounce from '../../../hooks/useDebounce'
import { siteColumns } from '../../elements/table/columns/siteColumns'
import { useSite } from '../../providers/SiteProvider'

const TABLE = 'Sites';

const SiteList = () => {
    const {setSites} = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return [
            {
                field: 'Name',
                value: debouncedSearchText
            }
        ]
    }, [debouncedSearchText]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [selectedRow, setSelectedRow] = React.useState();
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => siteColumns(),
        []
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
            <SiteForm onSuccess={() => {
                refetchData();
                hideModal();
            }} data={selectedRow} />
        </SideDialogWrapper>
    ), [selectedRow]);

    React.useEffect(() => {
        setTitle('Dépôts & Magasins')
    }, []);

    React.useEffect(()=>{
        refetchData();
    },[setSites])

    const refetchData = () => {
        getData(TABLE, {}, filters).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const deleteRow = React.useCallback(async (id) => {
        setLoading(true);
        const response = await deleteData(TABLE, id);
        console.log({ response });
        if (response.ok) {
            showSnackBar();
            refetchData();
            refreshSites();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le site sélectionné !'
            });
        }
        setLoading(false);
    },[]);

    const refreshSites = () => {
        getAllData('Sites')
            .then(res => setSites(res))
            .catch(err => console.error(err));
    }

    const updateRow = React.useCallback(async (row) => {
        setSelectedRow(row);
        showModal();
    },[]);


    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
                $skip: startRow
            }, filters).then((response) => {
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
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des dépôt/magasins" Icon={StorefrontOutlinedIcon} />
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
                        deleteRow={deleteRow}
                        updateRow={updateRow}
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

export default SiteList;
