import Box from '@material-ui/core/Box'
import React from 'react'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField, Button } from '@material-ui/core'
import { useModal } from 'react-modal-hook';
import Paper from '../../elements/misc/Paper';
import { useTitle } from '../../providers/TitleProvider';
import useDebounce from '../../../hooks/useDebounce';
import TitleIcon from '../../elements/misc/TitleIcon';
import { getPaiementClientListColumns } from '../../elements/table/columns/paiementClientColumns';
import DatePicker from '../../elements/date-picker/DatePicker';
import Table from '../../elements/table/Table';
import { getData, deleteData, saveData } from '../../../queries/crudBuilder';
import ClientAutocomplete from '../../elements/client-autocomplete/ClientAutocomplete';
import PaiementClientForm from '../../elements/forms/PaiementClientForm';
import { partialUpdateData } from '../../../queries/crudBuilder'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog';
import AddIcon from '@material-ui/icons/Add';
import PrintBL from '../../elements/dialogs/documents-print/PrintBL';
import { useSnackBar } from '../../providers/SnackBarProvider';

const TABLE = 'Paiements';

const EXPAND = ['TypePaiement', 'Client', 'BonLivraison/Client'];

const PaiementClientList = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const { setTitle } = useTitle();
    const [client, setClient] = React.useState(null);
    const [searchText, setSearchText] = React.useState('');
    const [selectedRow, setSelectedRow] = React.useState(null);
    const [documentToPrint, setDocumentToPrint] = React.useState(null);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const debouncedSearchText = useDebounce(searchText);
    const [errors, setErrors] = React.useState({});
    const { showSnackBar } = useSnackBar();
    const [showBLModal, hideBLModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBL
                onExited={onExited}
                open={open}
                document={documentToPrint}
                onClose={() => {
                    setDocumentToPrint(null);
                    hideBLModal();
                }}
            />
        )
    }, [documentToPrint]);
    const [showNewPaiementModal, hideNewPaiementModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hideNewPaiementModal}>
            <PaiementClientForm
                onSuccess={() => {
                    refetchData();
                    // hideNewPaiementModal();
                }}
            />
        </SideDialogWrapper>
    ));
    const [showPaiementModal, hidePaiementModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hidePaiementModal}>
            {selectedRow && <PaiementClientForm
                paiement={selectedRow}
                onSuccess={() => {
                    refetchData();
                    hidePaiementModal();
                }}
            />}
        </SideDialogWrapper>
    ), [selectedRow]);
    const filters = React.useMemo(() => {
        return {
            'Date': { ge: dateFrom, le: dateTo },
            IdClient: client ? { eq: { type: 'guid', value: client.Id } } : undefined,
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
                    'BonLivraison/NumBon': {
                        contains: debouncedSearchText
                    }
                },
            ]
        }
    }, [debouncedSearchText, client, dateFrom, dateTo]);
    const [data, setData] = React.useState([]);
    const [totalItems, setTotalItems] = React.useState(0);
    const [pageCount, setTotalCount] = React.useState(0);
    const fetchIdRef = React.useRef(0);
    const columns = React.useMemo(
        () => getPaiementClientListColumns(),
        []
    )


    const areDataValid = () => {
        const _errors = [];
        if (!client) {
            _errors['client'] = defaultErrorMsg;
        }

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    React.useEffect(() => {
        setTitle('Liste des paiements')
    }, []);

    const refetchData = () => {
        getData(TABLE, {}, filters, EXPAND).then((response) => {
            setData(response.data);
            setTotalItems(response.totalItems);
        }).catch((err) => {
            console.log({ err });
        })
    }

    const fetchData = React.useCallback(({ pageSize, pageIndex, filters }) => {
        const fetchId = ++fetchIdRef.current;
        if (fetchId === fetchIdRef.current) {
            const startRow = pageSize * pageIndex;
            getData(TABLE, {
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
        showBLModal();
    }, [])

    const disableRow = React.useCallback(async (document) => {
        const response = await partialUpdateData(TABLE, {
            Hide: !document.Hide
        }, document.Id)
        if(response.ok)
            showSnackBar();
        else
            showSnackBar({
                error: true,
                text: 'Erreur !'
            })
        refetchData()
    }, [])

    const updateRow = React.useCallback(async (row) => {
        setSelectedRow(row);
        showPaiementModal();
    }, []);

    //Impaye
    const customAction = React.useCallback(async (row) => {
        const preparedData = {
            IdTypePaiement: '399d159e-9ce0-4fcc-957a-08a65bbeece1',
            IdClient: row.IdClient,
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
    }, []);

    const deleteRow = React.useCallback(async (id) => {
        const response = await deleteData(TABLE, id);
        if (response.ok) {
            showSnackBar();
            refetchData();
        } else {
            showSnackBar({
                error: true,
                text: 'Impossible de supprimer la ligne sélectionnée !'
            });
        }
    }, []);

    return (
        <>
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={showNewPaiementModal}
                >
                    Nouveau paiement
                </Button>
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
                    <ClientAutocomplete
                        value={client}
                        onChange={(_, value) => setClient(value)}
                        errorText={errors.client}
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

export default PaiementClientList;
