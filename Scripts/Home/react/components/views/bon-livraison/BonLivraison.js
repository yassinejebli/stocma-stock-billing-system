import { Button, TextField, Switch, FormControlLabel, Dialog } from '@material-ui/core'
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
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'
import BarCodeScanning from '../../elements/animated-icons/BarCodeScanning'
import BonLivraisonUnsavedList, { AUTO_SAVED_BL } from './BonLivraisonUnsavedList'
import { getArticleByBarCode } from '../../../queries/articleQueries'
import BarcodeReader from 'react-barcode-reader'
import { looseFocus, convertLowercaseNumbersFR } from '../../../utils/miscUtils'
import SuiviVentes from '../ventes/suivi/SuiviVentes'
import BonLivraisonAutocomplete from '../../elements/bon-livraison-autocomplete/BonLivraisonAutocomplete'
import { useAuth } from '../../providers/AuthProvider'

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

const audioSuccess = new Audio('/Content/mp3/beep-success.mp3')
const audioFailure = new Audio('/Content/mp3/beep-failure.mp3')

const BonLivraison = () => {
    const {
        canUpdateBonLivraisons
    } = useAuth();
    const {
        BLDiscount,
        setBLDiscount,
        BLPayment,
        barcode,
        barcodeModule,
        restoreBLModule,
        suiviModule,
    } = useSettings();
    const { siteId, site, hasMultipleSites, company } = useSite();
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
    const [selectedBonLivraison, setSelectedBonLivraison] = React.useState(null);
    const [paymentType, setPaymentType] = React.useState(null);
    const [selectedArticleForSuivi, setSelectedArticleForSuivi] = React.useState(null);
    const location = useLocation();
    const DevisId = qs.parse(location.search, { ignoreQueryPrefix: true }).DevisId;
    const [bonLivraisonId, setBonLivraisonId] = React.useState(qs.parse(location.search, { ignoreQueryPrefix: true }).BonLivraisonId)
    const [isEditMode, setIsEditMode] = React.useState(Boolean(bonLivraisonId))
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
    const [result, setResult] = React.useState('')

    const columns = React.useMemo(
        () => getBonLivraisonColumns({ BLDiscount, hasMultipleSites, suiviModule, multiplyPA: company.Name === 'EAS' }),
        [BLDiscount, hasMultipleSites, company]
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
                        setData([...document.data, emptyLine]);
                        setDate(new Date(document.date));
                        setPaymentType(document.paymentType);
                        hideModalUnsavedDocs();
                    }}
                />
            </Dialog>)
    }, []);

    const [showModalSuiviVentes, hideModalSuiviVentes] = useModal(({ in: open, onExited }) => {
        return (
            <Dialog
                onExited={onExited}
                open={open}
                maxWidth="md"
                fullWidth
                onClose={() => {
                    hideModalSuiviVentes();
                    setTitle('Bon de livraison')
                }}
            >
                <SuiviVentes
                    client={client}
                    article={selectedArticleForSuivi}
                />
            </Dialog>)
    }, [client, selectedArticleForSuivi]);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setBarcodeScannerEnabled(barcode?.Enabled && barcodeModule?.Enabled);
    }, [barcode])

    React.useEffect(() => {
        if (!isEditMode && restoreBLModule?.Enabled) {
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
        if (isEditMode && bonLivraisonId) {
            setLoading(true);
            getSingleData(DOCUMENT, bonLivraisonId, [DOCUMENT_OWNER, 'TypePaiement', 'Site', DOCUMENT_ITEMS + '/' + 'Article/ArticleSites'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData([...response.BonLivraisonItems?.sort((a,b)=>b.Index - a.Index)?.map(x => ({
                        Article: {
                            ...x.Article,
                            QteStock: x.Article.ArticleSites?.find(y=>y.IdSite === siteId)?.QteStock
                        },
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Site: { Id: 1 },
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })), emptyLine]);
                    setNumDoc(response.NumBon);
                    setPaymentType(response.TypePaiement);
                    setRef(response.Ref);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        } else if (DevisId) {
            setLoading(true);
            getSingleData('Devises', DevisId, [DOCUMENT_OWNER, 'TypePaiement', 'DevisItems' + '/' + 'Article/ArticleSites'])
                .then(response => {
                    if (response.WithDiscount)
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: true }));
                    else
                        setBLDiscount(_docSetting => ({ ..._docSetting, Enabled: false }));
                    setClient(response.Client);
                    setData(response.DevisItems?.map(x => ({
                        Article: {
                            ...x.Article,
                            QteStock: x.Article.ArticleSites?.find(y=>y.IdSite === siteId)?.QteStock
                        },
                        Qte: x.Qte,
                        Pu: x.Pu,
                        Site: { Id: 1 },
                        Discount: x.Discount ? x.Discount + (x.PercentageDiscount ? '%' : '') : ''
                    })));
                    setPaymentType(response.TypePaiement);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        }
    }, [bonLivraisonId, isEditMode])

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

            if (_row.Pu <= _row.Article.PA) {
                _errors['table'] = 'Vérifier les prix de ventes.';
                return;
            }
        });

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const save = async () => {
        if (!areDataValid()) return;

        const expand = ['TypePaiement', DOCUMENT_ITEMS, DOCUMENT_OWNER];
        const Id = isEditMode ? bonLivraisonId : uuidv4();
        let oldDate = null;
        if (!isEditMode) {
            oldDate = new Date(date.getTime());
            const currentDateTime = new Date();
            date.setHours(currentDateTime.getHours(), currentDateTime.getMinutes(), currentDateTime.getSeconds());
        }

        const preparedData = {
            Id: Id,
            IdSite: siteId,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            IdTypePaiement: paymentType?.Id,
            WithDiscount: discount > 0,
            BonLivraisonItems: data.filter(x => x.Article).map((d, i) => ({
                Id: uuidv4(),
                IdBonLivraison: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id,
                IdSite: d.Site.Id,
                Discount: parseFloat(d.Discount),
                PercentageDiscount: (/^\d+(\.\d+)?%$/.test(d.Discount)),
                Index: i,
            })),
            IdClient: client.Id,
            Date: date
        };

        //return console.log({preparedData})
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
        setIsEditMode(false);
        setTimeout(() => {
            setDate(new Date());
        }, 1000)
        setData([]);
        addNewRow();
        setSelectedBonLivraison(null)
    }

    const openSuiviVentes = (row) => {
        setSelectedArticleForSuivi(row?.Article);
        showModalSuiviVentes();
    }

    return (
        <>
        {result}
        <div id="quagga">
             
        </div>
            {barcodeModule?.Enabled && <BarcodeReader
                onError={console.error}
                onScan={async (result) => {
                    console.log({ result })
                    // const firstChar = result.charAt(0);
                    let uppercaseBarCode = company?.Name === "AQK" ? result.substring(1) : result;
                    //TODO: review this
                    uppercaseBarCode = convertLowercaseNumbersFR(uppercaseBarCode).toUpperCase();
                    console.log({ uppercaseBarCode })

                    if (barcodeScannerEnabled && result) {
                        setLoading(true)
                        const response = await getArticleByBarCode(uppercaseBarCode, client?.Id, siteId)
                        if (response?.Id) {
                            if (data.find(x => x.Article?.Id === response?.Id)) {
                                setData(_data => _data.map(x => (x.Article?.Id === response?.Id ? { ...x, Qte: Number(x.Qte) + 1 } : x)))
                            } else {
                                setData(_data => ([{
                                    Article: response,
                                    Qte: 1,
                                    Site: site,
                                    Pu: response.PVD
                                }, ..._data]))
                            }
                            audioSuccess.play();
                        }
                        else {
                            audioFailure.play();
                            showSnackBar({
                                error: true,
                                text: "L'article scanné n'existe pas dans la base de données!"
                            })
                        }

                        setLoading(false)
                    }
                    if (!barcodeScannerEnabled) {
                        audioFailure.play();
                        showSnackBar({
                            error: true,
                            text: "Vous devez activer la lecture de code à barres!"
                        })
                    }

                }}
            />}
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="space-between">
                {!isEditMode && restoreBLModule?.Enabled && <Button
                    variant="contained"
                    color="secondary"
                    startIcon={<RestoreIcon />}
                    onClick={showModalUnsavedDocs}
                >
                    récupérer un BL non enregistré
                </Button>}
                <Button
                    style={{
                        marginLeft: 'auto'
                    }}
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
                        onChange={(_, value) => {
                            setClient(value);
                            looseFocus();
                        }}
                        errorText={errors.client}
                    />

                    <DatePicker
                        value={date}
                        onChange={(_date) => {
                            setDate(_date)
                            looseFocus();
                        }}
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
                            onChange={(_, value) => {
                                setPaymentType(value);
                                looseFocus();
                            }}
                            value={paymentType}
                        />
                    </Box>}
                </Box>
                {canUpdateBonLivraisons && <Box width={240} mt={2}>
                    <BonLivraisonAutocomplete
                        withoutMultiple
                        value={selectedBonLivraison}
                        onChange={(_, value) => {
                            const cleared = !Boolean(value?.Id);
                            setSelectedBonLivraison(value);
                            setBonLivraisonId(value?.Id);
                            setIsEditMode(!cleared);
                            if (cleared)
                                resetData();
                            looseFocus();
                        }}
                    />
                </Box>}
                <Box mt={4}>
                    {barcodeModule?.Enabled && <Box mb={2}>
                        <FormControlLabel
                            control={<Switch
                                checked={barcodeScannerEnabled}
                                onChange={(_, checked) => {
                                    setBarcodeScannerEnabled(checked);
                                    looseFocus();
                                }} />}
                            label={<BarCodeScanning scanning={barcodeScannerEnabled} />}
                        />
                    </Box>}
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
                        customAction={openSuiviVentes}
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
