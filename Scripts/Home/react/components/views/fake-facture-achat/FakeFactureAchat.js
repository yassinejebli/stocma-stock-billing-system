import { Button, TextField, Switch, FormControlLabel } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
import AddButton from '../../elements/button/AddButton'
import DescriptionIcon from '@material-ui/icons/Description';
import FournisseurAutocomplete from '../../elements/fournisseur-autocomplete/FournisseurAutocomplete'
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
import PrintFakeFactureAchat from '../../elements/dialogs/documents-print/PrintFakeFactureAchat'
import qs from 'qs'
import { fakeFactureAchatColumns } from '../../elements/table/columns/fakeFactureAchatColumns'
import { paymentMethods } from '../devis/Devis'
import Autocomplete from '@material-ui/lab/Autocomplete'
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';

const DOCUMENT = 'FakeFactureFs'
const DOCUMENT_ITEMS = 'FakeFactureFItems'
const DOCUMENT_OWNER = 'Fournisseur'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

const FakeFactureAchat = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [data, setData] = React.useState([emptyLine]);
    const [fournisseur, setFournisseur] = React.useState(null);
    const [chequeNumber, setChequeNumber] = React.useState('');
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [paymentType, setPaymentType] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const FakeFactureAchatId = qs.parse(location.search, { ignoreQueryPrefix: true }).FactureAchatId;
    const isEditMode = Boolean(FakeFactureAchatId);

    const columns = React.useMemo(
        () => fakeFactureAchatColumns(),
        []
    )
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintFakeFactureAchat
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
        setTitle("Facture d'achat")
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, FakeFactureAchatId, [DOCUMENT_OWNER, 'TypePaiement', DOCUMENT_ITEMS + '/' + 'ArticleFacture'])
                .then(response => {
                    setFournisseur(response.Fournisseur);
                    setChequeNumber(response.Comment);
                    setDate(response.Date);
                    setData(response.FakeFactureFItems?.map(x => ({
                        Article: x.ArticleFacture,
                        Qte: x.Qte,
                        Pu: x.Pu
                    })));
                    setPaymentType(response.TypePaiement);
                    setNumDoc(response.NumBon);
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
        if (!fournisseur) {
            _errors['fournisseur'] = defaultErrorMsg;
        }

        if (!date) {
            _errors['date'] = defaultErrorMsg;
        }

        if (!numDoc) {
            _errors['numDoc'] = defaultErrorMsg;
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
        });

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!areDataValid()) return;
        const expand = [DOCUMENT_ITEMS, DOCUMENT_OWNER];
        const Id = isEditMode ? FakeFactureAchatId : uuidv4();
        const preparedData = {
            Id: Id,
            NumBon: numDoc,
            FakeFactureFItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdFakeFactureF: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticleFacture: d.Article.Id
            })),
            IdTypePaiement: paymentType?.Id,
            IdFournisseur: fournisseur.Id,
            Comment: chequeNumber,
            Date: date
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);

        if (response?.Id) {
            setSavedDocument(response)
            resetData();
            showSnackBar();
            // showModal();
            history.replace('/_FactureAchat')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur!'
            });
        }
    }

    const resetData = () => {
        setFournisseur(null);
        setNumDoc('');
        setDate(new Date());
        setData([]);
        setPaymentType(null);
        addNewRow();
    }

    const convertPricesInTTC = () => {
        setData(_data=>{
            const articles = _data.filter(x=>x.Article && x.Pu);
            return articles.map(x=>({
                ...x,
                Pu: Number(x.Pu) + (Number(x.Pu) * x.Article.TVA/100)
            }))
        })
    }

    return (
        <>
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="space-between">
                <Button
                    variant="contained"
                    color="secondary"
                    startIcon={<MonetizationOnIcon />}
                    onClick={convertPricesInTTC}
                >
                    Convertir les prix en TTC
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<DescriptionIcon />}
                    onClick={() => history.push('/_FactureAchatList')}
                >
                    Liste des factures d'achat
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <FournisseurAutocomplete
                        disabled={isEditMode}
                        value={fournisseur}
                        onChange={(_, value) => setFournisseur(value)}
                        errorText={errors.fournisseur}
                    />
                    <TextField
                        value={numDoc}
                        onChange={({ target: { value } }) => setNumDoc(value)}
                        variant="outlined"
                        size="small"
                        label="N#"
                        helperText={errors.numDoc}
                        error={Boolean(errors.numDoc)}
                    />
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box mt={2} width={240}>
                    <TypePaiementAutocomplete
                        onChange={(_, value) => setPaymentType(value)}
                        value={paymentType}
                    />
                </Box>
                {paymentType?.IsBankRelated && <Box mt={2} width={240}><TextField
                    value={chequeNumber}
                    onChange={({ target: { value } }) => setChequeNumber(value)}
                    variant="outlined"
                    fullWidth
                    size="small"
                    label="Numéro de chèque/effet"
                /></Box>}
                <Box mt={4}>
                    <Box>
                        <AddButton tabIndex={-1} disableFocusRipple disableRipple onClick={addNewRow}>
                            Ajouter une ligne
                        </AddButton>
                    </Box>
                    <Table
                        columns={columns}
                        data={data}
                        owner={fournisseur}
                        updateMyData={updateMyData}
                        deleteRow={deleteRow}
                        skipPageReset={skipPageReset}
                        addNewRow={addNewRow}
                    />
                    {errors.table && <Error>
                        {errors.table}
                    </Error>}
                    <TotalText total={total} />
                    <Box mt={4} display="flex" justifyContent="flex-end">
                        <Button variant="contained" color="primary" onClick={save}>
                            Enregistrer
                         </Button>
                    </Box>

                </Box>
            </Paper>
        </>
    )
}

export default FakeFactureAchat;
