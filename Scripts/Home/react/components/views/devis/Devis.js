import { Button, TextField, InputAdornment } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
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
import qs from 'qs'
import { useSite } from '../../providers/SiteProvider'
import { devisColumns } from '../../elements/table/columns/devisColumns'
import PrintDevis from '../../elements/dialogs/documents-print/PrintDevis'
import Autocomplete from '@material-ui/lab/Autocomplete'
import { useSettings } from '../../providers/SettingsProvider'
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'

const DOCUMENT = 'Devises'
const DOCUMENT_ITEMS = 'DevisItems'
const DOCUMENT_OWNER = 'Client'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

export const paymentMethods = [
    {
        Id: '399d159e-9ce0-4fcc-957a-08a65bbeecb2',
        Name: 'Espéce',
    },
    {
        Id: '399d159e-9ce0-4fcc-957a-08a65bbeecb3',
        Name: 'Chéque',
        isBankRelatedItem: true,
    },
    {
        Id: '399d159e-9ce0-4fcc-957a-08a65bbeecb4',
        Name: 'Effet',
        isBankRelatedItem: true,
    },
]

export const deliveryTypes = ['Vos soins', 'Nos soins']

const Devis = () => {
    const { siteId } = useSite();
    const {
        devisDiscount,
        setDevisDiscount,
        devisValidity,
        devisPayment,
        devisTransport,
        devisDeliveryTime
    } = useSettings();
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
    const [validity, setValidity] = React.useState('');
    const [deliveryTime, setDeliveryTime] = React.useState('');
    const [deliveryType, setDeliveryType] = React.useState(null);
    const [paymentType, setPaymentType] = React.useState(null);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const [clientName, setClientName] = React.useState('');
    const location = useLocation();
    const DevisId = qs.parse(location.search, { ignoreQueryPrefix: true }).DevisId;
    const isEditMode = Boolean(DevisId);

    const columns = React.useMemo(
        () => devisColumns({ devisDiscount }),
        [devisDiscount]
    );
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
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintDevis
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
        if (!devisDiscount?.Enabled)
            setData(_data => _data.map(x => ({ ...x, Discount: '' })));
    }, [devisDiscount])

    React.useEffect(() => {
        setTitle('Devis')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, DevisId, [DOCUMENT_OWNER, 'TypePaiement', DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    if (response.WithDiscount)
                        setDevisDiscount(_docSetting => ({ ..._docSetting, Enabled: true }));
                    else
                        setDevisDiscount(_docSetting => ({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setClientName(response.ClientName);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData(response.DevisItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setNumDoc(response.NumBon);
                    setRef(response.Ref);
                    setDeliveryTime(response.DelaiLivrasion);
                    setValidity(response.ValiditeOffre);
                    setPaymentType(response.TypePaiement);
                    setDeliveryType(response.TransportExpedition);
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
        const Id = isEditMode ? DevisId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            WithDiscount: discount > 0,
            IdTypePaiement: paymentType?.Id,
            ValiditeOffre: validity,
            DelaiLivrasion: deliveryTime,
            TransportExpedition: deliveryType,
            DevisItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdDevis: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id,
                Discount: parseFloat(d.Discount),
                PercentageDiscount: (/^\d+(\.\d+)?%$/.test(d.Discount))
            })),
            IdClient: client.Id,
            ClientName: clientName,
            Date: date,
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);
        console.log({ response, isEditMode });

        if (response?.Id) {
            setSavedDocument(response)
            resetData();
            showSnackBar();
            showModal();
            history.replace('/Devis')
        } else {
            showSnackBar({
                error: true
            });
        }
    }

    const resetData = () => {
        setClient(null);
        setClientName('');
        setNote('');
        setDeliveryTime('');
        setValidity('');
        setPaymentType(null);
        setDeliveryType(null);
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
                    onClick={() => history.push('/DevisList')}
                >
                    Liste des devis
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <Box display="flex" flexWrap="wrap">
                        <Box mr={2}>
                            <ClientAutocomplete
                                disabled={isEditMode}
                                value={client}
                                onChange={(_, value) => {
                                    setClient(value);
                                    setClientName(value?.Name);
                                }}
                                errorText={errors.client}
                            />
                        </Box>
                        {isEditMode &&
                            <><Box mr={2} width={240}><TextField
                                value={ref}
                                onChange={({ target: { value } }) => setRef(value)}
                                variant="outlined"
                                fullWidth
                                size="small"
                                label="Référence"
                                type="number"
                            /></Box>
                                <Box width={240}> <TextField
                                    value={numDoc}
                                    disabled
                                    fullWidth
                                    onChange={({ target: { value } }) => setNumDoc(value)}
                                    variant="outlined"
                                    size="small"
                                    label="N#"
                                />
                                </Box>
                            </>
                        }
                    </Box>
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box mt={2} width={240}>
                    <TextField
                        value={clientName}
                        onChange={({ target: { value } }) => setClientName(value)}
                        variant="outlined"
                        fullWidth
                        size="small"
                        label="Nom du client sur le devis"
                    />
                </Box>
                <Box mt={2} display="flex" flexWrap="wrap">
                    {devisPayment?.Enabled && <Box mr={2} width={240}>
                        <TypePaiementAutocomplete
                            onChange={(_, value) => setPaymentType(value)}
                            value={paymentType}
                        />
                    </Box>}
                    {devisValidity?.Enabled && <Box width={240}>
                        <TextField
                            value={validity}
                            fullWidth
                            onChange={({ target: { value } }) => setValidity(value)}
                            variant="outlined"
                            size="small"
                            label="Validité de l'offre"
                        />
                    </Box>}
                </Box>
                <Box mt={2} display="flex">
                    {devisTransport?.Enabled && <Box mr={2} width={240}>
                        <Autocomplete
                            options={deliveryTypes}
                            disableClearable
                            autoHighlight
                            value={deliveryType}
                            onChange={(_, value) => setDeliveryType(value)}
                            getOptionSelected={(option, value) => option === value}
                            size="small"
                            renderInput={(params) => (
                                <TextField
                                    onChange={() => null}
                                    {...params}
                                    label="Transport / Expédition"
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
                    {devisDeliveryTime?.Enabled && <Box width={240}>
                        <TextField
                            value={deliveryTime}
                            fullWidth
                            onChange={({ target: { value } }) => setDeliveryTime(value)}
                            variant="outlined"
                            size="small"
                            label="Délai de livraision"
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
                            placeholder="Message à afficher sur le devis..."
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

export default Devis;