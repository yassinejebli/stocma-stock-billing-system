﻿import { Button, TextField, Switch, FormControlLabel, Dialog } from '@material-ui/core'
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
import RestoreIcon from '@material-ui/icons/Restore';
import PrintBL from '../../elements/dialogs/documents-print/PrintBL'
import qs from 'qs'
import { useSite } from '../../providers/SiteProvider'
import { useSettings } from '../../providers/SettingsProvider'
import { useAuth } from '../../providers/AuthProvider'
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'
import BarCodeScanning from '../../elements/animated-icons/BarCodeScanning'
import BonLivraisonUnsavedList, { AUTO_SAVED_BL } from './BonLivraisonUnsavedList'

const DOCUMENT = 'BonLivraisons'
const DOCUMENT_ITEMS = 'BonLivraisonItems'
const DOCUMENT_OWNER = 'Client'
const emptyLine = {
    Article: null,
    Site: null,
    Qte: 1,
    Pu: '',
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

const BonLivraison = () => {
    const {
        BLDiscount,
        setBLDiscount,
        BLPayment
    } = useSettings();
    const { siteId, hasMultipleSites } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [ref, setRef] = React.useState(0);
    const [barcodeScannerEnabled, setBarcodeScannerEnabled] = React.useState(false);
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
        () => getBonLivraisonColumns({ BLDiscount, hasMultipleSites }),
        [BLDiscount, hasMultipleSites]
    )
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBL
                onExited={onExited}
                open={open}
                typePaiement={!isEditMode ? savedDocument?.TypePaiement : null}
                document={savedDocument}
                onClose={() => {
                    setSavedDocument(null);
                    hideModal();
                }}
            />)
    }, [savedDocument]);

    const [showModalUnsavedDocs, hideModalUnsavedDocs] = useModal(({ in: open, onExited }) => {
        return (
            <Dialog
                onExited={onExited}
                open={open}
                maxWidth="sm"
                fullWidth
                onClose={() => {
                    hideModalUnsavedDocs();
                }}
            >
                <BonLivraisonUnsavedList
                    onImport={(document) => {
                        setClient(document.client);
                        setData(document.data);
                        setDate(new Date(document.date));
                        setPaymentType(document.paymentType);
                        hideModalUnsavedDocs();
                    }}
                />
            </Dialog>)
    }, []);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        if (!isEditMode) {
            try {
                let savedDocuments = [];
                const autoSavedBonLivraisons = localStorage.getItem(AUTO_SAVED_BL);
                if (autoSavedBonLivraisons) {
                    const savedDocumentsStorage = JSON.parse(autoSavedBonLivraisons);
                    let currentSavedDocument = savedDocumentsStorage.find(x => x.date === date.getTime());
                    const otherSavedDocuments = savedDocumentsStorage.filter(x => x.date !== date.getTime() && x.data.filter(x => x.Article).length > 0);

                    if (currentSavedDocument) {
                        currentSavedDocument.data = data;
                        currentSavedDocument.client = client;
                        currentSavedDocument.paymentType = paymentType;
                    } else if (data.filter(x => x.Article).length > 0) {
                        currentSavedDocument = {
                            client,
                            data,
                            paymentType,
                            date: date.getTime()
                        }
                    }

                    savedDocuments = [...otherSavedDocuments];
                    if (currentSavedDocument) {
                        savedDocuments.push(currentSavedDocument)
                    }
                } else {
                    if (data.filter(x => x.Article).length > 0)
                        savedDocuments = [{
                            client,
                            data,
                            paymentType,
                            date: date.getTime()
                        }];
                }
                localStorage.setItem(AUTO_SAVED_BL, JSON.stringify(savedDocuments));
            } catch {
                localStorage.removeItem(AUTO_SAVED_BL);
            }
        }
    }, [data, client, date])

    React.useEffect(() => {
        if (!BLDiscount?.Enabled)
            setData(_data => _data.map(x => ({ ...x, Discount: '' })));
    }, [BLDiscount])

    React.useEffect(() => {
        setTitle('Bon de livraison')
        //load bon de livraison
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, BonLivraisonId, [DOCUMENT_OWNER, 'TypePaiement', DOCUMENT_ITEMS + '/' + 'Article', 'Site'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData(response.BonLivraisonItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Site: x.Site,
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setNumDoc(response.NumBon);
                    setPaymentType(response.TypePaiement);
                    setRef(response.Ref);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        } else if (DevisId) {
            setLoading(true);
            getSingleData('Devises', DevisId, [DOCUMENT_OWNER, 'TypePaiement', 'DevisItems' + '/' + 'Article'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: false }));
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

        const expand = ['TypePaiement', DOCUMENT_ITEMS, DOCUMENT_OWNER];
        const Id = isEditMode ? BonLivraisonId : uuidv4();
        let oldDate = null;
        if (!isEditMode) {
            oldDate = new Date(date.getTime());
            const currentDateTime = new Date();
            date.setHours(currentDateTime.getHours(), currentDateTime.getMinutes(), currentDateTime.getSeconds());
        }
        console.log('after', { oldDate })

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
                IdSite: d.Site.Id,
                Discount: parseFloat(d.Discount),
                PercentageDiscount: (/^\d+(\.\d+)?%$/.test(d.Discount))
            })),
            IdClient: client.Id,
            Date: date
        };

        setLoading(true);
        let response = isEditMode ? await updateData(DOCUMENT, preparedData, Id, expand)
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);

        if (isEditMode && response.ok) {
            response = await response.json()
        }

        if (response?.Id) {
            setSavedDocument(response)
            showSnackBar();
            showModal();
            resetData();
            history.replace('/BonLivraison');
            const autoSavedBonLivraisons = localStorage.getItem(AUTO_SAVED_BL);
            if (autoSavedBonLivraisons && !isEditMode) {
                const savedDocumentsStorage = JSON.parse(autoSavedBonLivraisons);
                const otherSavedDocuments = savedDocumentsStorage.filter(x => x.date !== oldDate?.getTime() && x.data.filter(x => x.Article).length > 0);
                localStorage.setItem(AUTO_SAVED_BL, JSON.stringify(otherSavedDocuments))
            }
        } else {
            if (response.status === 406)
                showSnackBar({
                    error: true,
                    text: `${client?.Name} a dépassé la limite de crédit ${client?.Plafond} DH`
                });
            else
                showSnackBar({
                    error: true,
                    text: "Erreur!"
                });
        }
    }

    const resetData = () => {
        setClient(null);
        setNote('');
        setPaymentType(null);
        setTimeout(() => {
            setDate(new Date());
        }, 1000)
        setData([]);
        addNewRow();
    }

    return (
        <>
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="space-between">
                <Button
                    variant="contained"
                    color="secondary"
                    startIcon={<RestoreIcon />}
                    onClick={showModalUnsavedDocs}
                >
                    récupérer un BL non enregistré
                </Button>
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

                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                {isEditMode && <Box width={240} mt={2}>
                    <TextField
                        value={ref}
                        onChange={({ target: { value } }) => setRef(value)}
                        variant="outlined"
                        size="small"
                        fullWidth
                        label="Référence"
                        type="number"
                    />
                </Box>}
                {isEditMode && <Box width={240} mt={2}><TextField
                    value={numDoc}
                    disabled
                    fullWidth
                    onChange={({ target: { value } }) => setNumDoc(value)}
                    variant="outlined"
                    size="small"
                    label="N#"
                />
                </Box>}
                <Box mt={2} display="flex" flexWrap="wrap">
                    {BLPayment?.Enabled && <Box mr={2} width={240}>
                        <TypePaiementAutocomplete
                            onChange={(_, value) => setPaymentType(value)}
                            value={paymentType}
                        />
                    </Box>}
                </Box>
                <Box mt={4}>
                    <Box mb={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={barcodeScannerEnabled}
                                onChange={(_, checked) => setBarcodeScannerEnabled(checked)} />}
                            label={<BarCodeScanning scanning={barcodeScannerEnabled} />}
                        />
                    </Box>
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
