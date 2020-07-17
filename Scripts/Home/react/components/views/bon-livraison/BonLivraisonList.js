import Box from '@material-ui/core/Box'
import React from 'react'
import { getBonLivraisonListColumns } from '../../../utils/columnsBuilder'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import { useTitle } from '../../providers/TitleProvider'
import { getData, deleteData } from '../../../queries/crudBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import TitleIcon from '../../elements/misc/TitleIcon'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import { useModal } from 'react-modal-hook'
import AddIcon from '@material-ui/icons/Add';
import PrintBL from '../../elements/dialogs/documents-print/PrintBL'
import { useSite } from '../../providers/SiteProvider'
import { useLoader } from '../../providers/LoaderProvider'
import { useAuth } from '../../providers/AuthProvider'

const DOCUMENT = 'BonLivraisons'
const EXPAND = ['Client($select=Id,Name)', 'TypePaiement', 'BonLivraisonItems']

const BonLivraisonList = () => {
    const {
        canUpdateBonLivraisons,
        canDeleteBonLivraisons,
        isAdmin,
    } = useAuth();
    const { showLoader } = useLoader();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const [searchText, setSearchText] = React.useState('');
    const debouncedSearchText = useDebounce(searchText);
    const filters = React.useMemo(() => {
        return {
            and: [
                // { IdSite: siteId },
                {
                    or: {
                        'Client/Name': {
                            contains: debouncedSearchText
                        },
                        'NumBon': {
                            contains: debouncedSearchText
                        },
                        'User': {
                            contains: debouncedSearchText
                        }
                    }
                }
            ]
        }
    }, [debouncedSearchText]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const history = useHistory();
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getBonLivraisonListColumns({canUpdateBonLivraisons, canDeleteBonLivraisons, isAdmin}),
        [canUpdateBonLivraisons, canDeleteBonLivraisons, isAdmin]
    );
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBL
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
        setTitle('Bon de livraison')
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
                showLoader(false)
            })
    }

    const deleteRow = React.useCallback(async (id) => {
        showLoader(true, true)
        const response = await deleteData(DOCUMENT, id);
        console.log({ response });
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer le document sélectionné !'
            });
        }
        showLoader(false)
    }, [])

    const updateRow = React.useCallback(async (id) => {
        history.push(`BonLivraison?BonLivraisonId=${id}`);
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
                showLoader(false)
            });
        }
    }, [])

    const print = React.useCallback((document) => {
        setDocumentToPrint(document);
        showModal();
    }, [])

    return (
        <>
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => history.push('/BonLivraison')}
                >
                    Nouveau bon de livraison
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des bons de livraison" Icon={DescriptionOutlinedIcon} />
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

export default BonLivraisonList;
