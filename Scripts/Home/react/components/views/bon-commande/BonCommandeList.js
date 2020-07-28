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
import { TextField, Button } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import { useModal } from 'react-modal-hook'
import PrintBonCommande from '../../elements/dialogs/documents-print/PrintBonCommande'
import { bonCommandeListColumns } from '../../elements/table/columns/bonCommandeColumns'
import AddIcon from '@material-ui/icons/Add';

const DOCUMENT = 'BonCommandes'
const EXPAND = ['Fournisseur($select=Name)', 'BonCommandeItems']

const BonCommandeList = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            and: [
                {
                    or: {
                        'Fournisseur/Name': {
                            contains: debouncedSearchText
                        },
                        'NumBon': {
                            contains: debouncedSearchText
                        }
                    }
                }
            ]
        }
    }, [debouncedSearchText]);
    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const history = useHistory();
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => bonCommandeListColumns(),
        []
    );
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBonCommande
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
        setTitle('Bon de commande')
    }, []);

    const refetchData = () => {
        getData(DOCUMENT, {},
            filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
            }).catch((err) => {
                console.log({ err });
            })
    }

    const deleteRow = React.useCallback(async (id) => {
        setLoading(true);
        const response = await deleteData(DOCUMENT, id);
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le document sélectionné !'
            });
        }
        setLoading(false);
    }, [])

    const updateRow = React.useCallback(async (id) => {
        history.push(`BonCommande?BonCommandeId=${id}`);
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

    const print = React.useCallback((document) => {
        setDocumentToPrint(document);
        showModal();
    }, [])

    
    const convertToBR = React.useCallback((id) => {
        history.push('/BonReception?BonCommandeId=' + id);
    }, [])
    
    return (
        <>
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => history.push('/BonCommande')}
                >
                    Nouveau bon de commande
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des bons de commande" Icon={DescriptionOutlinedIcon} />
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
                        convert={convertToBR}
                        print={print}
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

export default BonCommandeList;
