import { Button, TextField, Switch, FormControlLabel } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
import { getBonLivraisonColumns } from '../../../utils/columnsBuilder'
import AddButton from '../../elements/button/AddButton'
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
import PrintBL from '../../elements/dialogs/documents-print/PrintBL'
import qs from 'qs'
import { useSite } from '../../providers/SiteProvider'
import { useSettings } from '../../providers/SettingsProvider'
import Autocomplete from '@material-ui/lab/Autocomplete'
import { paymentMethods } from '../devis/Devis'

const DOCUMENT = 'BonLivraisons'
const DOCUMENT_ITEMS = 'BonLivraisonItems'
const DOCUMENT_OWNER = 'Client'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

const BonLivraison = () => {
    const {
        BLDiscount,
        setBLDiscount,
        BLPayment
    } = useSettings();
    console.log({ BLPayment, BLDiscount })
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [ref, setRef] = React.useState(0);
    const [data, setData] = React.useState([emptyLine]);
    const [client, setClient] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [note, setNote] = React.useState('');
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const [paymentType, setPaymentType] = React.useState(null);
    const location = useLocation();
    const DevisId = qs.parse(location.search, { ignoreQueryPrefix: true }).DevisId;
    const BonLivraisonId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonLivraisonId;
    const isEditMode = Boolean(BonLivraisonId);
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);
    const discount = data.reduce((sum, curr) => {
        const total = curr.Pu * curr.Qte;
        if (curr.Discount) {
            if (!isNaN(curr.Discount))
                sum += Number(curr.Discount)
            else if (/^\d+(\.\d+)?%$/.test(curr.Discount)) {
                sum += total * parseFloat(curr.Discount) / 100;
            }
        }
        return sum;
    }, 0);

    const columns = React.useMemo(
        () => getBonLivraisonColumns({ BLDiscount }),
        [BLDiscount]
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBL
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
        if (!BLDiscount?.Enabled)
            setData(_data => _data.map(x => ({ ...x, Discount: '' })));
    }, [BLDiscount])

    React.useEffect(() => {
        setTitle('Bon de livraison')
        //load bon de livraison
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, BonLivraisonId, [DOCUMENT_OWNER, 'TypePaiement', DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting=>({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting=>({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData(response.BonLivraisonItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setNumDoc(response.NumBon);
                    setPaymentType(response.TypePaiement);
                    setRef(response.Ref);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        }else if (DevisId){
            setLoading(true);
            getSingleData('Devises', DevisId, [DOCUMENT_OWNER, 'TypePaiement', 'DevisItems' + '/' + 'Article'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting=>({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting=>({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setData(response.DevisItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setPaymentType(response.TypePaiement);
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
        });

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!areDataValid()) return;

        const expand = [DOCUMENT_ITEMS, DOCUMENT_OWNER];
        const Id = isEditMode ? BonLivraisonId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            IdTypePaiement: paymentType?.Id,
            WithDiscount: discount > 0,
            BonLivraisonItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdBonLivraison: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id,
                Discount: parseFloat(d.Discount),
                PercentageDiscount: (/^\d+(\.\d+)?%$/.test(d.Discount))
            })),
            IdClient: client.Id,
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
            showModal();
            history.replace('/BonLivraison')
        } else {
            showSnackBar({
                error: true
            });
        }
    }

    const resetData = () => {
        setClient(null);
        setNote('');
        setDate(new Date());
        setData([]);
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
                    onClick={() => history.push('/BonLivraisonList')}
                >
                    Liste des bons de livraison
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <ClientAutocomplete
                        disabled={isEditMode}
                        value={client}
                        onChange={(_, value) => setClient(value)}
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
                <Box mt={2} display="flex" flexWrap="wrap">
                    {BLPayment?.Enabled && <Box mr={2} width={240}>
                        <Autocomplete
                            options={paymentMethods}
                            disableClearable
                            autoHighlight
                            value={paymentType}
                            onChange={(_, value) => setPaymentType(value)}
                            size="small"
                            getOptionLabel={(option) => option?.Name}
                            renderInput={(params) => (
                                <TextField
                                    onChange={() => null}
                                    {...params}
                                    label="Mode de paiement"
                                    variant="outlined"
                                    inputProps={{
                                        ...params.inputProps,
                                        autoComplete: 'new-password',
                                        type: 'search',
                                        margin: 'normal'
                                    }}
                                />
                            )}
                        />
                    </Box>}
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
                    <TotalText total={total} discount={discount} />
                    <Box width={340}>
                        <TextField
                            value={note}
                            onChange={({ target: { value } }) => setNote(value)}
                            multiline
                            rows={3}
                            variant="outlined"
                            placeholder="Message à afficher sur le bon..."
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

export default BonLivraison;
