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
import TotalText from '../../elements/texts/TotalText'
import Loader from '../../elements/loaders/Loader'
import { useTitle } from '../../providers/TitleProvider'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { depenseColumns } from '../../elements/table/columns/depenseColumns'

const DOCUMENT = 'Depenses'
const DOCUMENT_ITEMS = 'DepenseItems'
const emptyLine = {
    TypeDepense: null,
    Montant: '',
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

const Depense = () => {
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [titre, setTitre] = React.useState('');
    const [data, setData] = React.useState([emptyLine]);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const location = useLocation();
    const DepenseId = qs.parse(location.search, { ignoreQueryPrefix: true }).DepenseId;
    const isEditMode = Boolean(DepenseId);

    const columns = React.useMemo(
        () => depenseColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);
    const total = data.reduce((sum, curr) => (
        sum += Number(curr.Montant)
    ), 0);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])


    React.useEffect(() => {
        setTitle('Dépense')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, DepenseId, [DOCUMENT_ITEMS + '/' + 'TypeDepense'])
                .then(response => {
                    setDate(response.Date);
                    setData(response.DepenseItems?.map(x => ({
                        TypeDepense: x.TypeDepense,
                        Montant: x.Montant,
                        Name: x.Name,
                    })));
                    setTitre(response.Titre);
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
        const filteredData = data.filter(x => x.TypeDepense);
        
        if (!date) {
            _errors['date'] = defaultErrorMsg;
        }

        if (filteredData.length < 1) {
            _errors['table'] = 'Ajouter des dépenses.';
        }

        filteredData.forEach((_row, _index) => {
            if (!_row.TypeDepense
                || !_row.Montant
                || Number(_row.Montant) < 0
                || !_row.Name
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
        const Id = isEditMode ? DepenseId : uuidv4();
        const preparedData = {
            Id: Id,
            Titre: titre,
            DepenseItems: data.filter(x => x.TypeDepense).map(d => ({
                Id: uuidv4(),
                IdDepense: Id,
                Montant: d.Montant,
                Name: d.Name,
                IdTypeDepense: d.TypeDepense.Id,
            })),
            Date: date
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);

        if (response?.Id) {
            resetData();
            showSnackBar();
            history.replace('/depense')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur!'
            });
        }
    }

    const resetData = () => {
        setDate(new Date());
        setTitre('');
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
                    onClick={() => history.push('/liste-des-depenses')}
                >
                    Liste des dépenses
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                    <Box display="flex" flexWrap="wrap">
                        <Box mr={2}>
                            <Box mr={2} width={240}>
                                <TextField
                                    value={titre}
                                    onChange={({ target: { value } }) => setTitre(value)}
                                    variant="outlined"
                                    fullWidth
                                    size="small"
                                    label="Titre"
                                />
                            </Box>
                        </Box>
                    </Box>
                    <DatePicker
                        value={date}
                        onChange={(_date) => setDate(_date)}
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

export default Depense;