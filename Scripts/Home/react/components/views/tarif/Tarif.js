import { Button, TextField, InputAdornment } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import { v4 as uuidv4 } from 'uuid'
import { saveData, updateData, getSingleData } from '../../../queries/crudBuilder'
import AddButton from '../../elements/button/AddButton'
import DescriptionIcon from '@material-ui/icons/Description';
import DatePicker from '../../elements/date-picker/DatePicker'
import Error from '../../elements/misc/Error'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { useModal } from 'react-modal-hook'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { tarifColumns } from '../../elements/table/columns/tarifColumns'
import PrintTarif from '../../elements/dialogs/documents-print/PrintTarif'
import ArticleCategoriesAutocomplete from '../../elements/article-categories-autocomplete/ArticleCategoriesAutocomplete'

const DOCUMENT = 'Tarifs'
const DOCUMENT_ITEMS = 'TarifItems'
const emptyLine = {
    Article: null,
    Pu2: '',
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

const Tarif = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [ref, setRef] = React.useState('');
    const [data, setData] = React.useState([emptyLine]);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const TarifId = qs.parse(location.search, { ignoreQueryPrefix: true }).TarifId;
    const isEditMode = Boolean(TarifId);
    const [selectedCategory, setSelectedCategory] = React.useState(null);

    const columns = React.useMemo(
        () => tarifColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);

    const [showModal, hideModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintTarif
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
        setTitle('Tarif')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, TarifId, [DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    setDate(response.Date);
                    setData(response.TarifItems?.map(x => ({
                        Article: x.Article,
                        Pu: x.Pu,
                        Pu2: x.Pu2,
                    })));
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

        if (!date) {
            _errors['date'] = defaultErrorMsg;
        }

        if (!ref) {
            _errors['ref'] = defaultErrorMsg;
        }

        if (filteredData.length < 1) {
            _errors['table'] = 'Ajouter des articles.';
        }

        filteredData.forEach((_row, _index) => {
            if (!_row.Article
                || !_row.Pu2
                || !_row.Pu
                || Number(_row.Pu) < 0
                || Number(_row.Pu2) <= 0
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
        const expand = [DOCUMENT_ITEMS];
        const Id = isEditMode ? TarifId : uuidv4();
        const preparedData = {
            Id: Id,
            Ref: ref,
            TarifItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdTarif: Id,
                Pu: d.Pu,
                Pu2: d.Pu2,
                IdArticle: d.Article.Id,
            })),
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
            history.replace('/Tarif')
        } else {
            showSnackBar({
                error: true
            });
        }
    }

    const resetData = () => {
        setDate(new Date());
        setRef('');
        setSelectedCategory(null)
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
                    onClick={() => history.push('/liste-tarif')}
                >
                    Liste des tarifs
                </Button>
            </Box>

            <Paper>
                <Box display="flex" justifyContent="end" flexWrap="wrap">
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
                    />
                </Box>
                <Box mt={2} display="flex" justifyContent="space-between">
                    <Box mr={2} width={240}>
                        <TextField
                            value={ref}
                            onChange={({ target: { value } }) => setRef(value)}
                            variant="outlined"
                            fullWidth
                            size="small"
                            label="Référence/Titre"
                        />
                    </Box>
                </Box>
                <Box mt={2} width={240}>
                    <ArticleCategoriesAutocomplete
                        value={selectedCategory}
                        withArticles
                        onChange={(_, selectedValue) => {
                            setSelectedCategory(selectedValue || null)
                            setRef(selectedValue?.Name || "")
                            if (selectedValue)
                                setData([...selectedValue.Articles.map(x => ({
                                    Article: x,
                                    Pu: x.PVD,
                                    Pu2: '',
                                })), emptyLine])
                            else
                                setData([emptyLine])
                        }}
                    />
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
                        updateMyData={updateMyData}
                        deleteRow={deleteRow}
                        skipPageReset={skipPageReset}
                        addNewRow={addNewRow}
                    />
                    {errors.table && <Error>
                        {errors.table}
                    </Error>}
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

export default Tarif;