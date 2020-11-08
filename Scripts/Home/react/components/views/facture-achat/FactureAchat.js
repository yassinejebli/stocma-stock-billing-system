import { Button, TextField } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
import DescriptionIcon from '@material-ui/icons/Description';
import FournisseurAutocomplete from '../../elements/fournisseur-autocomplete/FournisseurAutocomplete'
import DatePicker from '../../elements/date-picker/DatePicker'
import Error from '../../elements/misc/Error'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import TotalText from '../../elements/texts/TotalText'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { factureAchatColumns } from '../../elements/table/columns/factureAchatColumns'
import BonReceptionAutocomplete from '../../elements/bon-reception-autocomplete/BonReceptionAutocomplete'
import Autocomplete from '@material-ui/lab/Autocomplete'
import { paymentMethods } from '../devis/Devis'
import { useSettings } from '../../providers/SettingsProvider'
import TypePaiementAutocomplete from '../../elements/type-paiement-autocomplete/TypePaiementAutocomplete'

const DOCUMENT = 'FactureFs'
const DOCUMENT_OWNER = 'Fournisseur'

const defaultErrorMsg = 'Ce champs est obligatoire.'

const FactureAchat = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [fournisseur, setFournisseur] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [paymentType, setPaymentType] = React.useState(null);
    const [selectedBonReceptions, setSelectedBonReceptions] = React.useState([]);
    const location = useLocation();
    const FactureAchatId = qs.parse(location.search, { ignoreQueryPrefix: true }).FactureAchatId;
    const isEditMode = Boolean(FactureAchatId);
    const data = [].concat(...selectedBonReceptions.map(x => x.BonReceptionItems.map(y => ({
        Article: y.Article,
        Qte: y.Qte,
        Pu: y.Pu,
        Description: x.NumBon,
    }))));
    const discount = data.reduce((sum, curr) => {
        sum = curr.Pu * curr.Qte;
    }, 0);

    const columns = React.useMemo(
        () => factureAchatColumns(),
        []
    )
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setTitle("Facture d'achat")
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, FactureAchatId, [DOCUMENT_OWNER, 'TypePaiement', 'BonReceptions/BonReceptionItems/Article'])
                .then(response => {
                    console.log({ response })
                    setFournisseur(response.Fournisseur);
                    setDate(response.Date);
                    setSelectedBonReceptions(response.BonReceptions);
                    setNumDoc(response.NumBon);
                    setPaymentType(response.TypePaiement);
                }).catch(err => console.error(err))
                .finally(() => setLoading(false));
        }
    }, [])

    const updateMyData = (rowIndex, columnId, value) => {
        setSkipPageReset(true)
        // setData(old =>
        //     old.map((row, index) => {
        //         if (index === rowIndex) {
        //             return {
        //                 ...old[rowIndex],
        //                 [columnId]: value,
        //             }
        //         }

        //         return row
        //     })
        // )
    }

    const deleteRow = (rowIndex) => {
        // setData(_data => (_data.filter((_, i) => i !== rowIndex)));
    }

    const addNewRow = () => {
        // setData(_data => ([..._data, emptyLine]));
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
        const expand = [DOCUMENT_OWNER, 'BonReceptions/BonReceptionItems'];
        const Id = isEditMode ? FactureAchatId : uuidv4();
        const preparedData = {
            Id: Id,
            NumBon: numDoc,
            IdTypePaiement: paymentType?.Id,
            BonReceptions: selectedBonReceptions.map(x => {
                const { BonReceptionItems, ...bonReception } = x;
                return bonReception;
            }),
            FactureFItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdFactureF: Id,
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
            resetData();
            showSnackBar();
            history.replace('/FactureAchat')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur'
            });
        }
    }

    const resetData = () => {
        setFournisseur(null);
        setDate(new Date());
        setPaymentType(null);
        setNumDoc('');
        setSelectedBonReceptions([]);
    }

    return (
        <>
            <Loader loading={loading} />
            <Box mt={1} mb={2} display="flex" justifyContent="flex-end">
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<DescriptionIcon />}
                    onClick={() => history.push('/FactureAchatList')}
                >
                    Liste des factures d'achat
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <FournisseurAutocomplete
                        disabled={isEditMode}
                        value={fournisseur}
                        onChange={(_, value) => {
                            setFournisseur(value);
                            setSelectedBonReceptions([]);
                        }}
                        errorText={errors.fournisseur}
                    />
                    <TextField
                        value={numDoc}
                        onChange={({ target: { value } }) => setNumDoc(value)}
                        variant="outlined"
                        size="small"
                        label="N#"
                    />
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box width={220} mt={2}>
                    <BonReceptionAutocomplete
                        fournisseurId={fournisseur?.Id}
                        value={selectedBonReceptions}
                        onChange={(_, value) => {
                            console.log({value})
                            setSelectedBonReceptions(value)
                        }}
                    />
                </Box>
                <Box mt={2} display="flex" flexWrap="wrap">
                    <Box mr={2} width={240}>
                        <TypePaiementAutocomplete
                            onChange={(_, value) => setPaymentType(value)}
                            value={paymentType}
                        />
                    </Box>
                </Box>
                <Box mt={4}>
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
                    <TotalText total={total} discount={discount} />
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

export default FactureAchat;