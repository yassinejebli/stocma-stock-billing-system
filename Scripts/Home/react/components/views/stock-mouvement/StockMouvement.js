import { Button, TextField, makeStyles } from '@material-ui/core'
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
import { useSnackBar } from '../../providers/SnackBarProvider'
import { useLocation, useHistory } from 'react-router-dom'
import qs from 'qs'
import { stockMouvementColumns } from '../../elements/table/columns/stockMouvementColumns'
import { useSite } from '../../providers/SiteProvider'
import SiteAutocomplete from '../../elements/site-autocomplete/SiteAutocomplete'
import UndoIcon from '@material-ui/icons/Undo';

const DOCUMENT = 'StockMouvements'
const DOCUMENT_ITEMS = 'StockMouvementItems'
const emptyLine = {
    Article: null,
    Qte: 1,
    Pu: ''
}
const defaultErrorMsg = 'Ce champs est obligatoire.'

const useStyles = makeStyles({
    icon: {
        width: 28,
        height: 28,
        transform: 'scaleX(-1)'
    }
})

const StockMouvement = () => {
    const classes = useStyles();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const history = useHistory();
    const [comment, setComment] = React.useState('');
    const [siteFrom, setSiteFrom] = React.useState(null);
    const [siteTo, setSiteTo] = React.useState(null);
    const [data, setData] = React.useState([emptyLine]);
    const [date, setDate] = React.useState(new Date());
    const [errors, setErrors] = React.useState({});
    const [loading, setLoading] = React.useState(false);
    const [savedDocument, setSavedDocument] = React.useState(null);
    const location = useLocation();
    const StockMouvementId = qs.parse(location.search, { ignoreQueryPrefix: true }).StockMouvementId;
    const isEditMode = Boolean(StockMouvementId);

    const columns = React.useMemo(
        () => stockMouvementColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setSiteFrom(null)
        setSiteTo(null)
        setData([emptyLine])
    }, [siteId])


    React.useEffect(() => {
        setTitle('Mouvement de stock')
        if (isEditMode) {
            setLoading(true);
            getSingleData(DOCUMENT, StockMouvementId, ['SiteFrom','SiteTo',DOCUMENT_ITEMS + '/' + 'Article'])
                .then(response => {
                    setDate(response.Date);
                    setSiteFrom(response.SiteFrom)
                    setSiteTo(response.SiteTo)
                    setData(response.StockMouvementItems?.map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                        Pu: x.Pu,
                    })));
                    setComment(response.Comment);
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

        if (!siteFrom) {
            _errors['siteFrom'] = defaultErrorMsg;
        }

        if (!siteTo) {
            _errors['siteTo'] = defaultErrorMsg;
        }

        if(siteTo?.Id === siteFrom?.Id){
            _errors['siteTo'] = 'Choisir un autre magasin!';
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
        const expand = [DOCUMENT_ITEMS];
        const Id = isEditMode ? StockMouvementId : uuidv4();
        const preparedData = {
            Id: Id,
            IdSiteFrom: siteFrom.Id,
            IdSiteTo: siteTo.Id,
            Comment: comment,
            Date: date,
            [DOCUMENT_ITEMS]: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdStockMouvement: Id,
                Qte: d.Qte,
                IdArticle: d.Article.Id,
            })),
        };

        setLoading(true);
        const response = isEditMode ? await (await updateData(DOCUMENT, preparedData, Id, expand)).json()
            : await saveData(DOCUMENT, preparedData, expand);
        setLoading(false);

        if (response?.Id) {
            setSavedDocument(response)
            resetData();
            showSnackBar();
            history.replace('/mouvement-stock')
        } else {
            showSnackBar({
                error: true,
                text: 'Erreur!'
            });
        }
    }

    const resetData = () => {
        setComment('');
        setDate(new Date());
        setData([]);
        setSiteFrom(null);
        setSiteTo(null);
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
                    onClick={() => history.push('/liste-mouvement-stock')}
                >
                    Liste des mouvements
                </Button>
            </Box>
            <Paper>
                <Box display="flex" justifyContent="space-between" alignItems="center" flexWrap="wrap">
                    <SiteAutocomplete
                        value={siteFrom}
                        onChange={(_, value) => {
                            if(siteId !== value?.Id)
                                return showSnackBar({
                                    error: true,
                                    text: "Vous devez choisir le même magasin actuel dans le menu déroulant!"
                                })
                            setSiteFrom(value)
                        }}
                        errorText={errors.siteFrom}
                    />
                    <UndoIcon className={classes.icon} color="primary" />
                    <SiteAutocomplete
                        value={siteTo}
                        onChange={(_, value) => setSiteTo(value)}
                        errorText={errors.siteTo}
                    />
                </Box>
                <Box mt={2} display="flex" justifyContent="space-between">
                    <Box width={240}>
                        <TextField
                            value={comment}
                            fullWidth
                            onChange={({ target: { value } }) => setComment(value)}
                            variant="outlined"
                            size="small"
                            label="Commentaire"
                            multiline
                            error={Boolean(errors.comment)}
                            helperText={errors.comment}
                        />
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

export default StockMouvement;