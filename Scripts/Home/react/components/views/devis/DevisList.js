import Box from '@material-ui/core/Box'
import React from 'react'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import TitleIcon from '../../elements/misc/TitleIcon'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import AddIcon from '@material-ui/icons/Add';
import { TextField, Button } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import { useModal } from 'react-modal-hook'
import { useSite } from '../../providers/SiteProvider'
import { devisListColumns } from '../../elements/table/columns/devisColumns'
import PrintDevis from '../../elements/dialogs/documents-print/PrintDevis'
import { useLoader } from '../../providers/LoaderProvider'

const DOCUMENT = 'Devises'
const EXPAND = ['Client($select=Id,Name)', 'DevisItems']

const DevisList = () => {
    const { showLoader } = useLoader();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            and: [
                { IdSite: siteId },
                {
                    or: {
                        'Client/Name': {
                            contains: debouncedSearchText
                        },
                        'ClientName': {
                            contains: debouncedSearchText
                        },
                        'NumBon': {
                            contains: debouncedSearchText
                        }
                    }
                }
            ]
        }
    }, [debouncedSearchText, siteId]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const history = useHistory();
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => devisListColumns(),
        []
    );
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintDevis
                onExited={onExited}
                open={open}
                document={documentToPrint}
                onClose={() => {
                    setDocumentToPrint(null);
                    hideModal();
                }}
            />
        )
    }, [documentToPrint]);

    React.useEffect(() => {
        setTitle('Devis')
    }, []);

    const refetchData = () => {
        showLoader(true, true)
        getData(DOCUMENT, {},
            filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
            }).catch((err) => {
                console.log({ err });
            }).finally(() => {
                showLoader()

            })
    }

    const deleteRow = React.useCallback(async (id) => {
        const response = await deleteData(DOCUMENT, id);
        showLoader(true, true)
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le devis sélectionné !'
            });
        }
        showLoader()
    }, [])

    const updateRow = React.useCallback((id) => {
        history.push(`Devis?DevisId=${id}`);
    }, []);


    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        // setPageIndex(pageIndex);
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            // const endRow = startRow + pageSize
            showLoader(true, true)
            getData(DOCUMENT, {
                $skip: startRow
            }, filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            }).finally(() => {
                showLoader()

            })
        }
    }, [])

    const print = React.useCallback((document) => {
        setDocumentToPrint(document);
        showModal();
    }, [])

    const convertToBL = React.useCallback((id) => {
        history.push('/BonLivraison?DevisId=' + id);
    }, [])

    return (
        <>
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => history.push('/Devis')}
                >
                    Nouveau devis
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des devis" Icon={DescriptionOutlinedIcon} />
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
                        print={print}
                        convert={convertToBL}
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

export default DevisList;
