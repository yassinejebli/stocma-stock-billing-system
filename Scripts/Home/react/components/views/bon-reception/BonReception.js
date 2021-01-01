import { Button, TextField, Switch, FormControlLabel, Dialog } from '@material-ui/core'
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
import PrintBR from '../../elements/dialogs/documents-print/PrintBR'
import qs from 'qs'
import { useSite } from '../../providers/SiteProvider'
import { getArticleAchatByBarCode } from '../../../queries/articleQueries'
import { bonReceptionColumns } from '../../elements/table/columns/bonReceptionColumns'
import { useSettings } from '../../providers/SettingsProvider'
import SuiviAchats from '../achats/suivi/SuiviAchats'
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';
import BonReceptionAutocomplete from '../../elements/bon-reception-autocomplete/BonReceptionAutocomplete'
import BarcodeReader from 'react-barcode-reader'
import { convertLowercaseNumbersFR, looseFocus } from '../../../utils/miscUtils'
import BarCodeScanning from '../../elements/animated-icons/BarCodeScanning'

const DOCUMENT = 'BonReceptions'
const DOCUMENT_ITEMS = 'BonReceptionItems'
const DOCUMENT_OWNER = 'Fournisseur'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const defaultErrorMsg = 'Ce champs est obligatoire.'
const audioSuccess = new Audio('/Content/mp3/beep-success.mp3')
const audioFailure = new Audio('/Content/mp3/beep-failure.mp3')
const BonReception = () => {
    const {
        barcode,
        barcodeModule,
        suiviModule,
    } = useSettings();
    const { siteId, company, useVAT } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [data, setData] = React.useState([emptyLine]);
    const [fournisseur, setFournisseur] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [selectedBonReception, setSelectedBonReception] = React.useState(null);
    const [selectedArticleForSuivi, setSelectedArticleForSuivi] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const BonCommandeId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonCommandeId;
    const [bonReceptionId, setBonReceptionId] = React.useState(qs.parse(location.search, { ignoreQueryPrefix: true }).BonReceptionId);
    const [isEditMode, setIsEditMode] = React.useState(Boolean(bonReceptionId));
    const [barcodeScannerEnabled, setBarcodeScannerEnabled] = React.useState(false);

    const columns = React.useMemo(
        () => bonReceptionColumns({ suiviModule }),
        []
    )
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);
    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBR
                onExited={onExited}
                open={open}
                document={savedDocument}
                onClose={() => {
                    setSavedDocument(null);
                    hideModal();
                    setTitle('Bon de réception')
                }}
            />)
    }, [savedDocument]);

    const [showModalSuiviAchats, hideModalSuiviAchats] = useModal(({ in: open, onExited }) => {
        return (
            <Dialog
                onExited={onExited}
                open={open}
                maxWidth="md"
                fullWidth
                onClose={() => {
                    hideModalSuiviAchats();
                }}
            >
                <SuiviAchats
                    fournisseur={fournisseur}
                    article={selectedArticleForSuivi}
                />
            </Dialog>)
    }, [fournisseur, selectedArticleForSuivi]);

    React.useEffect(() => {
        setData([emptyLine])
    }, [siteId])

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setBarcodeScannerEnabled(barcode?.Enabled && barcodeModule?.Enabled);
    }, [barcode])

    React.useEffect(() => {
        setTitle('Bon de réception')
        if (isEditMode && bonReceptionId) {
            setLoading(true);
            getSingleData(DOCUMENT, bonReceptionId, [DOCUMENT_OWNER, DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    setFournisseur(response.Fournisseur);
                    setDate(response.Date);
                    setData(response.BonReceptionItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu
                    })));
                    setNumDoc(response.NumBon);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        } else if (BonCommandeId) {
            setLoading(true);
            getSingleData('BonCommandes', BonCommandeId, [DOCUMENT_OWNER, 'BonCommandeItems' + '/' + 'Article'])
                .then(response => {
                    setFournisseur(response.Fournisseur);
                    setData(response.BonCommandeItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                    })));
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        }
    }, [isEditMode, bonReceptionId])

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
        const Id = isEditMode ? bonReceptionId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            NumBon: numDoc,
            BonReceptionItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdBonReception: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id
            })),
            IdFournisseur: fournisseur.Id,
            Date: date
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);
        console.log({ response, isEditMode });

        if (response?.Id) {
            console.log({useVAT})
            if(!useVAT){
                setSavedDocument(response)
                showModal();
            }
            resetData();
            showSnackBar();
            history.replace('/BonReception')
        } else {
            showSnackBar({
                error: true
            });
        }
    }

    const resetData = () => {
        setFournisseur(null);
        setNumDoc('');
        setDate(new Date());
        setData([]);
        addNewRow();
        setSelectedBonReception(null);
    }

    const openSuiviAchats = (row) => {
        setSelectedArticleForSuivi(row?.Article);
        showModalSuiviAchats();
    }

    const convertPricesInTTC = () => {
        setData(_data => {
            const articles = _data.filter(x => x.Article && x.Pu);
            return articles.map(x => ({
                ...x,
                Pu: Number(x.Pu) + (Number(x.Pu) * x.Article.TVA / 100)
            }))
        })
    }

    return (
        <>
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
                        const response = await getArticleAchatByBarCode(uppercaseBarCode, fournisseur?.Id, siteId)
                        if (response?.Id) {
                            if (data.find(x => x.Article?.Id === response?.Id)) {
                                setData(_data => _data.map(x => (x.Article?.Id === response?.Id ? { ...x, Qte: Number(x.Qte) + 1 } : x)))
                            } else {
                                setData(_data => ([{
                                    Article: response,
                                    Qte: 1,
                                    Pu: response.PA
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
                    onClick={() => history.push('/BonReceptionList')}
                >
                    Liste des bons de réception
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <FournisseurAutocomplete
                        disabled={isEditMode}
                        value={fournisseur}
                        onChange={(_, value) => {
                            setFournisseur(value)
                            looseFocus()
                        }}
                        errorText={errors.fournisseur}
                    />
                    <TextField
                        value={numDoc}
                        onChange={({ target: { value } }) => setNumDoc(value)}
                        variant="outlined"
                        size="small"
                        placeholder="N#"
                        helperText={errors.numDoc}
                        error={Boolean(errors.numDoc)}
                    />
                    <DatePicker
                        value={date}
                        onChange={(_date) => {
                            setDate(_date)
                            looseFocus();
                        }}
                    />
                </Box>
                <Box width={240} mt={2}>
                    <BonReceptionAutocomplete
                        withoutMultiple
                        value={selectedBonReception}
                        onChange={(_, value) => {
                            const cleared = !Boolean(value?.Id);
                            setSelectedBonReception(value);
                            setBonReceptionId(value?.Id);
                            setIsEditMode(!cleared);
                            if (cleared)
                                resetData();
                            looseFocus();

                        }}
                    />
                </Box>
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
                        owner={fournisseur}
                        updateMyData={updateMyData}
                        customAction={openSuiviAchats}
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

export default BonReception;
