import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useModal } from 'react-modal-hook'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog'
import SiteForm from '../../elements/forms/SiteForm'
import TitleIcon from '../../elements/misc/TitleIcon'
import StorefrontOutlinedIcon from '@material-ui/icons/StorefrontOutlined';
import { TextField, Button, FormControlLabel, Checkbox } from '@material-ui/core'
import useDebounce from '../../../hooks/useDebounce'
import { siteColumns } from '../../elements/table/columns/siteColumns'
import { useSite } from '../../providers/SiteProvider'
import AddIcon from '@material-ui/icons/Add';

const TABLE = 'Sites';

const SiteList = () => {
    const [showSiteModal, hideSiteModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideSiteModal}>
            <SiteForm />
        </SideDialogWrapper>
    ));
    const { fetchSites } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [showDisabledData, setShowDisabledData] = React.useState(false);
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return [
            {
                Name: {
                    contains: debouncedSearchText
                },
                Disabled: !showDisabledData ? false : undefined,
            }
        ]
    }, [debouncedSearchText, showDisabledData]);
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

    React.useEffect(() => {
        refetchData();
    }, [fetchSites])

    const refetchData = () => {
        getData(TABLE, {}, filters).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const deleteRow = React.useCallback(async (id) => {

        if (Number(localStorage.getItem('site')) === id) {
            showSnackBar({
                error: true,
                text: 'Vous devez changer le dépôt/magasin actuel pour pouvoir l\'archiver'
            });
            return;
        }

        setLoading(true);
        const response = await deleteData(TABLE, id);
        console.log({ response });
        if (response.ok) {
            showSnackBar();
            refetchData();
            fetchSites();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le site sélectionné !'
            });
        }
        setLoading(false);
    }, []);

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
                    onClick={showSiteModal}
                >
                    Nouveau magasin/dépôt
                </Button>
            </Box>
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
                <Box mt={2}>
                    <FormControlLabel
                        control={<Checkbox checked={showDisabledData} color="primary" onChange={event => setShowDisabledData(event.target.checked)} />}
                        label="Afficher les magasins désactivés"
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
