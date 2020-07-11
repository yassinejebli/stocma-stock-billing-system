import { Button, TextField } from '@material-ui/core'
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
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { bonAvoirAchatColumns } from '../../elements/table/columns/bonAvoirAchatColumns'
import { useSite } from '../../providers/SiteProvider'

const DOCUMENT = 'BonAvoirs'
const DOCUMENT_ITEMS = 'BonAvoirItems'
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

const BonAvoirAchat = () => {
    const { siteId, hasMultipleSites } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [numDoc, setNumDoc] = React.useState('');
    const [ref, setRef] = React.useState(0);
    const [data, setData] = React.useState([emptyLine]);
    const [fournisseur, setFournisseur] = React.useState(null);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const BonAvoirAchatId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonAvoirAchatId;
    const isEditMode = Boolean(BonAvoirAchatId);

    const columns = React.useMemo(
        () => bonAvoirAchatColumns({hasMultipleSites}),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setTitle('Bon d\'avoir (achat)')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, BonAvoirAchatId, [DOCUMENT_OWNER, DOCUMENT_ITEMS + '/' + 'Article', 'Site'])
                .then(response => {
                    setFournisseur(response.Fournisseur);
                    setDate(response.Date);
                    setData(response.BonAvoirItems?.map(x => ({
                        Article: x.Article,
                        Site: x.Site,
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

        if (!numDoc) {
            _errors['numDoc'] = defaultErrorMsg;
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
        const Id = isEditMode ? BonAvoirAchatId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            NumBon: numDoc,
            Ref: ref,
            BonAvoirItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdBonAvoir: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdArticle: d.Article.Id,
                IdSite: d.Site.Id,
            })),
            IdFournisseur: fournisseur.Id,
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
            history.replace('/bon-avoir-achat')
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
                    onClick={() => history.push('/liste-bon-avoir-achat')}
                >
                    Liste des bons d'avoir (achat)
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
                    <Box width={240}>
                        <TextField
                            value={numDoc}
                            fullWidth
                            onChange={({ target: { value } }) => setNumDoc(value)}
                            variant="outlined"
                            size="small"
                            label="N#"
                            error={Boolean(errors.numDoc)}
                            helperText={errors.numDoc}
                        />
                    </Box>
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

export default BonAvoirAchat;