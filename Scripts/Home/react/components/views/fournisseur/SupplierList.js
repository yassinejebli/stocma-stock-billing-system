import Box from '@material-ui/core/Box'
import React from 'react'
import { getFournisseurColumns } from '../../../utils/columnsBuilder'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import FournisseurForm from '../../elements/forms/FournisseurForm'
import TitleIcon from '../../elements/misc/TitleIcon'
import PeopleAltOutlinedIcon from '@material-ui/icons/PeopleAltOutlined';
import { TextField } from '@material-ui/core'

const TABLE = 'Fournisseurs';

const defaultPageSize = 10;

const SupplierList = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [selectedRow, setSelectedRow] = React.useState();
    const [pageCount, setTotalCount] = React.useState(0);
    const [pageIndex, setPageIndex] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getFournisseurColumns(),
        []
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideModal}>
            <FournisseurForm onSuccess={()=>{
                refetchData();
                hideModal();
            }} data={selectedRow} />
        </SideDialogWrapper>
    ), [selectedRow]);

    React.useEffect(() => {
        setTitle('Fournisseurs')
    }, []);

    React.useEffect(()=>{
        refetchData();
        //reset page inex after filtering
        setPageIndex(0);
    }, [searchText]);

    const refetchData = () => {
        getData(TABLE, {
            $skip: pageIndex * defaultPageSize
        },{
            field: 'Name',
            value: searchText
        }).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const deleteRow = async (id) => {
        setLoading(true);
        const response = await deleteData(TABLE, id);
        console.log({ response });
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le fournisseur sélectionné !'
            });
        }
        setLoading(false);
    }

    const updateRow = async (row) => {
        setSelectedRow(row);
        showModal();
    }


    const fetchData = React.useCallback(({ pageSize, pageIndex }) => {
        const fetchId = ++fetchIdRef.current;
        setLoading(true);
        setPageIndex(pageIndex);
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
                $skip: startRow
            }).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
                setLoading(false);
            }).catch((err) => {
                console.log({ err });
                setLoading(false);
            });
        }
    }, [])

    return (
        <>
            <Loader loading={loading} />
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des fournisseurs" Icon={PeopleAltOutlinedIcon} />
                    <TextField
                        value={searchText}
                        onChange={({target: {value}}) => {
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
                        pageIndex={pageIndex}
                    />
                </Box>
            </Paper>
        </>
    )
}

export default SupplierList;
