import Box from '@material-ui/core/Box'
import React from 'react'
import DescriptionOutlinedIcon from '@material-ui/icons/DescriptionOutlined';
import { TextField } from '@material-ui/core'
import { useModal } from 'react-modal-hook';
import Paper from '../../elements/misc/Paper';
import { useTitle } from '../../providers/TitleProvider';
import useDebounce from '../../../hooks/useDebounce';
import TitleIcon from '../../elements/misc/TitleIcon';
import { getBankPaiementsFournisseurListColumns } from '../../elements/table/columns/paiementFournisseurColumns';
import DatePicker from '../../elements/date-picker/DatePicker';
import Table from '../../elements/table/Table';
import { getData, deleteData, saveData } from '../../../queries/crudBuilder';
import FournisseurAutocomplete from '../../elements/fournisseur-autocomplete/FournisseurAutocomplete';
import PaiementFournisseurForm from '../../elements/forms/PaiementFournisseurForm';
import { partialUpdateData } from '../../../queries/crudBuilder'
import { SideDialogWrapper } from '../../elements/dialogs/SideWrapperDialog';
import { useSnackBar } from '../../providers/SnackBarProvider';
import SoldeText from '../../elements/texts/SoldeText';
import { useLoader } from '../../providers/LoaderProvider';
import { useHistory } from 'react-router-dom';

const TABLE = 'PaiementFs';

const EXPAND = ['TypePaiement', 'Fournisseur'];

const BankPaiementsFournisseurList = () => {
    const refreshCount = React.useRef(0)
    const { showLoader } = useLoader();
    const history = useHistory();
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const { setTitle } = useTitle();
    const [fournisseur, setFournisseur] = React.useState(null);
    const [searchText, setSearchText] = React.useState('');
    const [selectedRow, setSelectedRow] = React.useState(null);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const debouncedSearchText = useDebounce(searchText);
    const [errors, setErrors] = React.useState({});
    const { showSnackBar } = useSnackBar();
    const filters = React.useMemo(() => {
        return {
            'Date': { ge: dateFrom, le: dateTo },
            IdFournisseur: fournisseur ? { eq: { type: 'guid', value: fournisseur.Id } } : undefined,
            'TypePaiement/IsBankRelated': true,
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
                    'Credit': !isNaN(debouncedSearchText) ? Number(debouncedSearchText) : undefined
                },
            ]
        }
    }, [debouncedSearchText, fournisseur, dateFrom, dateTo]);
    
    const [showPaiementModal, hidePaiementModal] = useModal(({ in: open, onExited }) => (
        <SideDialogWrapper open={open} onExited={onExited} onClose={hidePaiementModal}>
            {selectedRow && <PaiementFournisseurForm
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
        () => getBankPaiementsFournisseurListColumns({ isFiltered: Boolean(!fournisseur) }),
        [fournisseur]
    )


    const areDataValid = () => {
        const _errors = [];
        if (!fournisseur) {
            _errors['fournisseur'] = defaultErrorMsg;
        }

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    React.useEffect(() => {
        setTitle('Liste des chèques / effets')
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


    const disableRow = React.useCallback(async (document) => {
        const response = await partialUpdateData(TABLE, {
            Hide: !document.Hide
        }, document.Id)
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
    }, []);

    const editDocument = React.useCallback((id) => {
        history.push(`BonLivraison?BonLivraisonId=${id}`);
    }, []);

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
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <TitleIcon noBorder title="Liste des chèques / effets" Icon={DescriptionOutlinedIcon} />
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
                        updateRow2={editDocument}
                        deleteRow={deleteRow}
                        disableRow={disableRow}
                        customAction={customAction}
                        totalItems={totalItems}
                        pageCount={pageCount}
                        fetchData={fetchData}
                        filters={filters}
                    />
                    {fournisseur && <Box mt={2}>
                        <SoldeText refresh={refreshCount.current} fournisseurId={fournisseur.Id} date={dateFrom} />
                    </Box>}
                </Box>
            </Paper>
        </>
    )
}

export default BankPaiementsFournisseurList;
