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
import { bonAvoirVenteColumns } from '../../elements/table/columns/bonAvoirVenteColumns'
import PrintBonAvoirVente from '../../elements/dialogs/documents-print/PrintBonAvoirVente'
import { useSite } from '../../providers/SiteProvider'

const DOCUMENT = 'BonAvoirCs'
const DOCUMENT_ITEMS = 'BonAvoirCItems'
const DOCUMENT_OWNER = 'Client'
const emptyLine = {
    Article: null,
    Site: null,
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

const BonAvoirVente = () => {
    const { siteId, hasMultipleSites } = useSite();
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
    const location = useLocation();
    const BonAvoirVenteId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonAvoirVenteId;
    const isEditMode = Boolean(BonAvoirVenteId);

    const columns = React.useMemo(
        () => bonAvoirVenteColumns({hasMultipleSites}),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += curr.Pu * curr.Qte
    ), 0);

    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintBonAvoirVente
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
        setTitle('Bon d\'avoir (vente)')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, BonAvoirVenteId, [DOCUMENT_OWNER, DOCUMENT_ITEMS + '/' + 'Article', 'Site'])
                .then(response => {
                    setClient(response.Client);
                    setDate(response.Date);
                    setNote(response.Note);
                    setData(response.BonAvoirCItems?.map(x => ({
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
        const Id = isEditMode ? BonAvoirVenteId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            Note: note,
            NumBon: numDoc,
            Ref: ref,
            BonAvoirCItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdBonAvoirC: Id,
                Qte: d.Qte,
                Pu: d.Pu,
                IdSite: d.Site.Id,
                IdArticle: d.Article.Id,
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
            history.replace('/bon-avoir-vente')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur!'
            });
        }
    }

    const resetData = () => {
        setClient(null);
        setNote('');
        setTimeout(() => {
            setDate(new Date());
        }, 1000)
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
                    onClick={() => history.push('/liste-bon-avoir-vente')}
                >
                    Liste des bons d'avoir (vente)
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <Box display="flex" flexWrap="wrap">
                        <Box mr={2}>
                            <ClientAutocomplete
                                disabled={isEditMode}
                                value={client}
                                onChange={(_, value) => setClient(value)}
                                errorText={errors.client}
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
                            placeholder="Message à afficher sur le bon d'avoir..."
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

export default BonAvoirVente;