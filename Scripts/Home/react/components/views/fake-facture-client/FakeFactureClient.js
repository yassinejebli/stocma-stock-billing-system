import { Button, TextField } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
import DescriptionIcon from '@material-ui/icons/Description';
import ClientAutocomplete from '../../elements/client-autocomplete/ClientAutocomplete'
import DatePicker from '../../elements/date-picker/DatePicker'
import Error from '../../elements/misc/Error'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import TotalText from '../../elements/texts/TotalText'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { useModal } from 'react-modal-hook'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { fakeFactureColumns } from '../../elements/table/columns/fakeFactureColumns'
import PrintFakeFacture from '../../elements/dialogs/documents-print/PrintFakeFacture'
import { useSettings } from '../../providers/SettingsProvider'
import AddButton from '../../elements/button/AddButton'
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'

const DOCUMENT = 'FakeFactures'
const DOCUMENT_OWNER = 'Client'

const defaultErrorMsg = 'Ce champs est obligatoire.'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const FakeFacture = () => {
    const {
        facturePayment,
        factureCheque
    } = useSettings();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [data, setData] = React.useState([emptyLine]);

    const [numDoc, setNumDoc] = React.useState('');
    const [ref, setRef] = React.useState(0);
    const [client, setClient] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [dueDate, setDueDate] = React.useState(null);
    const [note, setNote] = React.useState('');
    const [chequeNumber, setChequeNumber] = React.useState('');
    const [clientName, setClientName] = React.useState('');
    const [clientICE, setClientICE] = React.useState('');
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const [paymentType, setPaymentType] = React.useState(null);
    const location = useLocation();
    const FakeFactureId = qs.parse(location.search, { ignoreQueryPrefix: true }).FactureId;
    const isEditMode = Boolean(FakeFactureId);


    const columns = React.useMemo(
        () => fakeFactureColumns(),
        []
    )
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintFakeFacture
                onExited={onExited}
                open={open}
                document={savedDocument}
                onClose={() => {
                    setSavedDocument(null);
                    hideModal();
                }}
            />)
    }, [savedDocument]);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setTitle('Facture')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, FakeFactureId, [DOCUMENT_OWNER, 'TypePaiement', 'FakeFactureItems/ArticleFacture'])
                .then(response => {
                    setClient(response.Client);
                    setDate(response.Date);
                    setDueDate(response.DateEcheance);
                    setChequeNumber(response.Comment);
                    setNote(response.Note);
                    setData(response.FakeFactureItems?.map(x => ({
                        Article: x.ArticleFacture,
                        Qte: x.Qte,
                        Pu: x.Pu,
                        // Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setClientName(response.ClientName);
                    setClientICE(response.ClientICE);
                    setNumDoc(response.NumBon);
                    setPaymentType(response.TypePaiement);
                    setRef(response.Ref);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        }
    }, [])

    const updateMyData = (rowIndex, columnId, value) => {
        setSkipPageReset(true)
        setData(old =>
            old.map((row, index) => {
                if (index === rowIndex) {
                    return {
                        ...old[rowIndex],
                        [columnId]: value,
                    }
                }

                return row
            })
        )
    }

    const deleteRow = (rowIndex) => {
        setData(_data => (_data.filter((_, i) => i !== rowIndex)));
    }

    const addNewRow = () => {
        setData(_data => ([..._data, emptyLine]));
    }

    const areDataValid = () => {
        const _errors = [];
        const filteredData = data.filter(x => x.Article);
        if (!client) {
            _errors['client'] = defaultErrorMsg;
        }

        if (!date) {
            _errors['date'] = defaultErrorMsg;
        }

        if (filteredData.length < 1) {
            _errors['table'] = 'Ajouter des articles.';
        }

        filteredData.forEach((_row, _index) => {
            if (!_row.Article
                || !_row.Qte
                || !_row.Pu
                || Number(_row.Pu) < 0
                || Number(_row.Qte) <= 0
            ) {
                _errors['table'] = 'Compléter les lignes.';
                return;
            }

            if (_row.Pu <= _row.Article.PA && !isEditMode) {
                _errors['table'] = 'Vérifier les prix de ventes.';
                return;
            }
        });

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!areDataValid()) return;
        const expand = [DOCUMENT_OWNER, 'FakeFactureItems/ArticleFacture'];
        const Id = isEditMode ? FakeFactureId : uuidv4();
        const preparedData = {
            Id: Id,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            Comment: chequeNumber,
            IdTypePaiement: paymentType?.Id,
            ClientName: clientName,
            ClientICE: clientICE,
            FakeFactureItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdFakeFacture: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticleFacture: d.Article.Id
            })),
            IdClient: client.Id,
            Date: date,
            DateEcheance: dueDate,
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);

        if (response?.Id) {
            setSavedDocument(response)
            resetData();
            showSnackBar();
            showModal();
            history.replace('/_Facture')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur'
            });
        }
    }

    const resetData = () => {
        setClient(null);
        setClientName('');
        setClientICE('');
        setNote('');
        setData([]);
        setDate(new Date());
        setPaymentType(null);
        addNewRow();
    }

    return (
        <>
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<DescriptionIcon />}
                    onClick={() => history.push('/_FactureList')}
                >
                    Liste des factures
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <ClientAutocomplete
                        disabled={isEditMode}
                        value={client}
                        onChange={(_, value) => {
                            setClient(value);
                            setClientName(value?.Name);
                            setClientICE(value?.ICE||'');
                        }}
                        errorText={errors.client}
                    />
                    {isEditMode &&
                        <><TextField
                            value={ref}
                            onChange={({ target: { value } }) => setRef(value)}
                            variant="outlined"
                            size="small"
                            label="Référence"
                            type="number"
                        />
                            <TextField
                                value={numDoc}
                                disabled
                                onChange={({ target: { value } }) => setNumDoc(value)}
                                variant="outlined"
                                size="small"
                                label="N#"
                            />
                        </>
                    }
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box mt={2} display="flex" justifyContent="space-between">
                    <Box width={240}>
                        <TextField
                            value={clientName}
                            onChange={({ target: { value } }) => setClientName(value)}
                            variant="outlined"
                            fullWidth
                            size="small"
                            label="Nom du société"
                        />
                    </Box>
                    <DatePicker
                        label="Date d'échéance"
                        value={dueDate}
                        onChange={(_date) => setDueDate(_date)}
                        clearable
                    />
                </Box>
                <Box mt={2} width={240}>
                    <TextField
                        value={clientICE}
                        onChange={({ target: { value } }) => setClientICE(value)}
                        variant="outlined"
                        fullWidth
                        size="small"
                        label="ICE"
                    />
                </Box>
                <Box mt={2} display="flex" flexWrap="wrap">
                    {facturePayment?.Enabled && <Box mr={2} width={240}>
                        <TypePaiementAutocomplete
                            onChange={(_, value) => setPaymentType(value)}
                            value={paymentType}
                        />
                    </Box>}
                    {factureCheque?.Enabled && paymentType?.IsBankRelated && <Box width={240}><TextField
                        value={chequeNumber}
                        onChange={({ target: { value } }) => setChequeNumber(value)}
                        variant="outlined"
                        fullWidth
                        size="small"
                        label="Numéro de chèque/effet"
                    /></Box>}
                </Box>

                <Box mt={4}>
                    <Box>
                        <AddButton tabIndex={-1} disableFocusRipple disableRipple onClick={addNewRow}>
                            Ajouter une ligne
                        </AddButton>
                    </Box>
                    <Table
                        columns={columns}
                        data={data}
                        owner={client}
                        updateMyData={updateMyData}
                        deleteRow={deleteRow}
                        skipPageReset={skipPageReset}
                        addNewRow={addNewRow}
                    />
                    {errors.table && <Error>
                        {errors.table}
                    </Error>}
                    <TotalText total={total} />
                    <Box width={340}>
                        <TextField
                            value={note}
                            onChange={({ target: { value } }) => setNote(value)}
                            multiline
                            rows={3}
                            variant="outlined"
                            placeholder="Message à afficher sur la facture..."
                            size="small"
                            fullWidth
                        />
                    </Box>
                    <Box display="flex" justifyContent="flex-end">
                        <Button variant="contained" color="primary" onClick={save}>
                            Enregistrer
                         </Button>
                    </Box>

                </Box>
            </Paper>
        </>
    )
}

export default FakeFacture;