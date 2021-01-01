import Box from '@material-ui/core/Box'
import React from 'react'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { useModal } from 'react-modal-hook';
import Paper from '../../elements/misc/Paper';
import { useTitle } from '../../providers/TitleProvider';
import useDebounce from '../../../hooks/useDebounce';
import TitleIcon from '../../elements/misc/TitleIcon';
import { getPaiementFournisseurListColumns } from '../../elements/table/columns/paiementFournisseurFactureColumns';
import DatePicker from '../../elements/date-picker/DatePicker';
import Table from '../../elements/table/Table';
import { getData, deleteData, saveData } from '../../../queries/crudBuilder';
import FournisseurAutocomplete from '../../elements/fournisseur-autocomplete/FournisseurAutocomplete';
import { partialUpdateData } from '../../../queries/crudBuilder'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog';
import AddIcon from '@material-ui/icons/Add';
import { useSnackBar } from '../../providers/SnackBarProvider';
import { useLoader } from '../../providers/LoaderProvider';
import { getPrintSituationGlobaleFournisseursURL } from '../../../utils/urlBuilder';
import IframeDialog from '../../elements/dialogs/IframeDialog';
import PaiementFactureFournisseurForm from '../../elements/forms/PaiementFactureFournisseurForm';
import { useSite } from '../../providers/SiteProvider';
import PrintFournisseurAccountSummary from '../../elements/dialogs/documents-print/PrintFournisseurAccountSummary';
import PrintIcon from '@material-ui/icons/Print';

const TABLE = 'PaiementFactureFs';

const EXPAND = ['TypePaiement', 'Fournisseur($select=Id,Name)', 'FactureF/Fournisseur'];

const PaiementFactureFournisseurList = () => {
    const refreshCount = React.useRef(0)
    const { showLoader } = useLoader();
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const { setTitle } = useTitle();
    const [fournisseur, setFournisseur] = React.useState(null);
    const [searchText, setSearchText] = React.useState('');
    const [summaryFournisseurIdToPrint, setSummaryFournisseurIdToPrint] = React.useState(null);
    const [selectedRow, setSelectedRow] = React.useState(null);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const debouncedSearchText = useDebounce(searchText);
    const [errors, setErrors] = React.useState({});
    const { showSnackBar } = useSnackBar();
    const { useVAT } = useSite();
    const filters = React.useMemo(() => {
        return {
            'Date': { ge: dateFrom, le: dateTo },
            IdFournisseur: fournisseur ? { eq: { type: 'guid', value: fournisseur.Id } } : undefined,
            or: [
                {
                    'Comment': {
                        contains: debouncedSearchText
                    }
                },
                {
                    'TypePaiement/Name': {
                        contains: debouncedSearchText
                    }
                },
                {
                    'FactureF/NumBon': {
                        contains: debouncedSearchText
                    }
                },
                {
                    'Credit': !isNaN(debouncedSearchText) ? Number(debouncedSearchText) : undefined
                },
            ]
        }
    }, [debouncedSearchText, fournisseur, dateFrom, dateTo]);
    const [showAccountSummaryModal, hideAccountSummaryModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintFournisseurAccountSummary
                onExited={onExited}
                open={open}
                fournisseurId={summaryFournisseurIdToPrint}
                dateFrom={dateFrom}
                dateTo={dateTo}
                onClose={() => {
                    setSummaryFournisseurIdToPrint(null);
                    hideAccountSummaryModal();
                }}
            />
        )
    }, [summaryFournisseurIdToPrint, dateFrom, dateTo]);
    const [showNewPaiementModal, hideNewPaiementModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideNewPaiementModal}>
            <PaiementFactureFournisseurForm
                onSuccess={() => {
                    refetchData();
                    // hideNewPaiementModal();
                }}
            />
        </SideDialogWrapper>
    ), [filters, fournisseur]);
    const [showPaiementModal, hidePaiementModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hidePaiementModal}>
            {selectedRow && <PaiementFactureFournisseurForm
                paiement={selectedRow}
                onSuccess={() => {
                    refetchData();
                    hidePaiementModal();
                }}
            />}
        </SideDialogWrapper>
    ), [selectedRow]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getPaiementFournisseurListColumns({ isFiltered: Boolean(!fournisseur) }),
        [fournisseur]
    )
    const [showPrintSituationGlobale, hidePrintSituationGlobale] = useModal(({ in: open, onExited }) => {

        return (
            <IframeDialog
                onExited={onExited}
                open={open}
                onClose={hidePrintSituationGlobale}
                src={getPrintSituationGlobaleFournisseursURL()}>
            </IframeDialog>
        )
    }, []);

    React.useEffect(() => {
        setTitle('Liste des paiements')
    }, []);

    const refetchData = React.useCallback(() => {
        console.log({ filters })
        showLoader(true, true);
        getData(TABLE, {}, filters, EXPAND).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        }).finally(() => {
            refreshCount.current += 1;
            showLoader();

        })
    }, [filters]);

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            showLoader(true, true);
            getData(TABLE, {
                $skip: startRow
            }, filters, EXPAND).then((response) => {
                setData(response.data);
                setTotalItems(response.totalItems);
                setTotalCount(Math.ceil(response.totalItems / pageSize))
            }).catch((err) => {
                console.log({ err });
            }).finally(() => {
                showLoader();
            });
        }
    }, [])

    const print = React.useCallback((document) => {
        // setDocumentToPrint(document);
        // showBRModal();
    }, [])

    const disableRow = React.useCallback(async (row) => {
        const response = await partialUpdateData(TABLE, {
            Hide: !row.Hide
        }, row.Id)
        if (response.ok)
            showSnackBar();
        else
            showSnackBar({
                error: true,
                text: 'Erreur !'
            })
        refetchData();
    }, [filters])

    const updateRow = React.useCallback(async (row) => {
        setSelectedRow(row);
        showPaiementModal();
    }, []);

    const printAccountSummary = () => {
        setSummaryFournisseurIdToPrint(fournisseur?.Id);
        showAccountSummaryModal();
    }

    //Impaye
    const customAction = React.useCallback(async (row) => {
        const preparedData = {
            IdTypePaiement: '399d159e-9ce0-4fcc-957a-08a65bbeece1',
            IdFournisseur: row.IdFournisseur,
            Debit: row.Credit,
            Date: new Date(),
            DateEcheance: row.DateEcheance,
            Comment: row.Comment
        }
        const response = await saveData(TABLE, preparedData)
        if (response?.Id)
            showSnackBar();
        else
            showSnackBar({
                error: true,
                text: 'Erreur !'
            })
        refetchData()
    }, [filters]);

    const deleteRow = React.useCallback(async (id) => {
        const response = await deleteData(TABLE, id);
        if (response.ok) {
            showSnackBar();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer la ligne sélectionnée !'
            });
        }
        refetchData();
    }, [filters]);

    return (
        <>
            <Box mt={1} mb={2} display="flex" justifyContent="space-between">
                {/* TODO: remove useVAT condition */}
                {fournisseur && useVAT && dateFrom && dateTo && <Button
                    variant="contained"
                    color="primary"
                    startIcon={<PrintIcon />}
                    onClick={printAccountSummary}
                >
                    Imprimer la situation du fournisseur
                </Button>}
                <Box ml="auto">
                    {/* <Button
                        variant="contained"
                        color="secondary"
                        style={{
                            marginRight: 8
                        }}
                        startIcon={<ListAltIcon />}
                        onClick={showPrintSituationGlobale}
                    >
                        Situation Globale
                    </Button> */}
                    <Button
                        variant="contained"
                        color="primary"
                        startIcon={<AddIcon />}
                        onClick={showNewPaiementModal}
                    >
                        Nouveau paiement
                </Button>
                </Box>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des paiements" Icon={DescriptionOutlinedIcon} />
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
                <Box width={240} mt={3}>
                    <FournisseurAutocomplete
                        disableClearable={false}
                        value={fournisseur}
                        onChange={(_, value) => setFournisseur(value)}
                        errorText={errors.fournisseur}
                    />
                </Box>
                <Box mt={3}>
                    <DatePicker
                        value={dateFrom}
                        label="Date de début"
                        onChange={(date) => {
                            date && date.setHours(0, 0, 0, 0);
                            setDateFrom(date)
                        }}
                    />
                    <DatePicker
                        style={{
                            marginLeft: 12
                        }}
                        value={dateTo}
                        label="Date de fin"
                        onChange={(date) => {
                            date && date.setHours(23, 59, 59, 999);
                            setDateTo(date)
                        }}
                    />
                </Box>
                <Box mt={4}>
                    <Table
                        columns={columns}
                        data={data}
                        serverPagination
                        updateRow={updateRow}
                        deleteRow={deleteRow}
                        disableRow={disableRow}
                        customAction={customAction}
                        print={print}
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

export default PaiementFactureFournisseurList;
