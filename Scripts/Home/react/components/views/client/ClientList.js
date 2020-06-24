import Box from '@material-ui/core/Box'
import React from 'react'
import { getClientColumns } from '../../../utils/columnsBuilder'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import ClientForm from '../../elements/forms/ClientForm'
import TitleIcon from '../../elements/misc/TitleIcon'
import PeopleAltOutlinedIcon from '@material-ui/icons/PeopleAltOutlined';
import { TextField, Button } from '@material-ui/core'
import useDebounce from '../../../hooks/useDebounce'
import { useSite } from '../../providers/SiteProvider'
import AddIcon from '@material-ui/icons/Add';

const TABLE = 'Clients';

const ClientList = () => {
    const { showSnackBar } = useSnackBar();
    const {useVAT} = useSite();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            or: [
                {
                    Name: {
                        contains: debouncedSearchText
                    }
                },
                {
                    ICE: {
                        contains: debouncedSearchText
                    }
                }
            ]
        }
    }, [debouncedSearchText]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [selectedRow, setSelectedRow] = React.useState(null);
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getClientColumns({useVAT: useVAT}),
        [useVAT]
    )
    const [showClientModal, hideClientModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideClientModal}>
            <ClientForm 
                onSuccess={()=>{
                    refetchData();
                }}
             />
        </SideDialogWrapper>
    ), [filters]);
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
            <ClientForm onSuccess={() => {
                refetchData();
                hideModal();
            }} data={selectedRow} />
        </SideDialogWrapper>
    ), [selectedRow]);

    React.useEffect(() => {
        setTitle('Clients')
    }, []);


    const refetchData = React.useCallback(() => {
        getData(TABLE, {}, filters).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    },[filters])

    const deleteRow = React.useCallback(async (id) => {
        setLoading(true);
        const response = await deleteData(TABLE, id);
        console.log({ response });
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le client sélectionné !'
            });
        }
        setLoading(false);
    }, [filters]);

    const updateRow = React.useCallback(async (row) => {
        setSelectedRow(row);
        showModal();
    }, []);


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
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={showClientModal}
                >
                    Nouveau client
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des clients" Icon={PeopleAltOutlinedIcon} />
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

export default ClientList;
