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
import { TextField } from '@material-ui/core'
import { useHistory } from 'react-router-dom'
import useDebounce from '../../../hooks/useDebounce'
import { useModal } from 'react-modal-hook'
import { useSite } from '../../providers/SiteProvider'
import { factureListColumns } from '../../elements/table/columns/factureColumns'
import PrintFacture from '../../elements/dialogs/documents-print/PrintFacture'

const DOCUMENT = 'Factures'
const EXPAND = ['Client', 'BonLivraisons/BonLivraisonItems/Article']

const FakeFactureClientList = () => {
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
                        'NumBon': {
                            contains: debouncedSearchText
                        }
                    }
                }
            ]
        }
    }, [debouncedSearchText, siteId]);
const [loading, setLoading] = React.useState(false);
const [totalItems, setTotalItems] = React.useState(0);
const [pageCount, setTotalCount] = React.useState(0);
const [documentToPrint, setDocumentToPrint] = React.useState(null);
const history = useHistory();
const fetchIdRef = React.useRef(0);
const columns = React.useMemo(
    () => factureListColumns(),
    []
);
const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
    return (
        <PrintFacture
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
    setTitle('Facture')
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
    console.log({ response });
    if (response.ok) {
        showSnackBar();
        refetchData();
    } else {
        showSnackBar({
            error: true,
            text: 'Impossible de supprimer la facture sélectionnée !'
        });
    }
    setLoading(false);
}, [])

const updateRow = React.useCallback(async (id) => {
    history.push(`Facture?FactureId=${id}`);
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

return (
    <>
        <Loader loading={loading} />
        <Paper>
            <Box display="flex" justifyContent="space-between" alignItems="center">
                <TitleIcon noBorder title="Liste des factures (client)" Icon={DescriptionOutlinedIcon} />
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

export default FakeFactureClientList;
