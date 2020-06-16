import { Button, TextField, InputAdornment } from '@material-ui/core'
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
import qs from 'qs'
import { bonCommandeColumns } from '../../elements/table/columns/bonCommandeColumns'
import PrintBonCommande from '../../elements/dialogs/documents-print/PrintBonCommande'

const DOCUMENT = 'BonCommandes'
const DOCUMENT_ITEMS = 'BonCommandeItems'
const DOCUMENT_OWNER = 'Fournisseur'
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

const BonCommande = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [ref, setRef] = React.useState(0);
    const [data, setData] = React.useState([emptyLine]);
    const [fournisseur, setFournisseur] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [note, setNote] = React.useState('');
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const BonCommandeId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonCommandeId;
    const isEditMode = Boolean(BonCommandeId);

    const columns = React.useMemo(
        () => bonCommandeColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);

    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBonCommande
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
        setTitle('Bon de commande')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, BonCommandeId, [DOCUMENT_OWNER, DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    setFournisseur(response.Fournisseur);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData(response.BonCommandeItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                    })));
                    setNumDoc(response.NumBon);
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
        const expand = [DOCUMENT_ITEMS, DOCUMENT_OWNER];
        const Id = isEditMode ? BonCommandeId : uuidv4();
        const preparedData = {
            Id: Id,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            BonCommandeItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdBonCommande: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id,
            })),
            IdFournisseur: fournisseur.Id,
            Date: date
        };

        // return console.log({preparedData});

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
            history.replace('/BonCommande')
        } else {
            showSnackBar({
                error: true
            });
        }
    }

    const resetData = () => {
        setFournisseur(null);
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
                    onClick={() => history.push('/BonCommandeList')}
                >
                    Liste des bons de commande
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <Box display="flex" flexWrap="wrap">
                        <Box mr={2}>
                            <FournisseurAutocomplete
                                disabled={isEditMode}
                                value={fournisseur}
                                onChange={(_, value) => setFournisseur(value)}
                                errorText={errors.fournisseur}
                            />
                        </Box>
                    </Box>
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box mt={2} display="flex" justifyContent="space-between">
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
                    <Box width={340}>
                        <TextField
                            value={note}
                            onChange={({ target: { value } }) => setNote(value)}
                            multiline
                            rows={3}
                            variant="outlined"
                            placeholder="Message à afficher sur le bon de commande..."
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

export default BonCommande;