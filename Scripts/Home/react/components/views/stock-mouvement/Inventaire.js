import { Button, TextField, makeStyles, IconButton, FormControl, Select, MenuItem } from '@material-ui/core'
import Box from '@material-ui/core/Box'
import React from 'react'
import AddButton from '../../elements/button/AddButton'
import Error from '../../elements/misc/Error'
import Paper from '../../elements/misc/Paper'
import Table from '../../elements/table/Table'
import { useTitle } from '../../providers/TitleProvider'
import PrintIcon from '@material-ui/icons/Print';
import { inputTypes } from '../../../types/input'
import CloseIcon from '@material-ui/icons/Close';
import ArticleAutocomplete from '../../elements/article-autocomplete/ArticleAutocomplete'
import TitleIcon from '../../elements/misc/TitleIcon'
import { useModal } from 'react-modal-hook'
import LocalMallOutlinedIcon from '@material-ui/icons/LocalMallOutlined'
import { useLocation } from 'react-router-dom'
import qs from 'qs'
import { useLoader } from '../../providers/LoaderProvider'
import { getSingleData, saveData } from '../../../queries/crudBuilder'
import { useSite } from '../../providers/SiteProvider'
import { getInventoryList } from '../../../queries/articleQueries'
import IframeDialog from '../../elements/dialogs/IframeDialog'
import { getPrintInventaireURL } from '../../../utils/urlBuilder'
import { useSnackBar } from '../../providers/SnackBarProvider'
import { v4 as uuidv4 } from 'uuid'

const emptyLine = {
    Article: null,
    QteStock: '',
}

const DOCUMENT = 'Inventaires'

const useStyles = makeStyles({

})

const Inventaire = () => {
    const classes = useStyles();
    const { siteId } = useSite();
    const { showSnackBar } = useSnackBar();
    const { setTitle } = useTitle();
    const { showLoader } = useLoader();
    const location = useLocation();
    const InventaireId = qs.parse(location.search, { ignoreQueryPrefix: true }).InventaireId;
    const [data, setData] = React.useState([emptyLine]);
    const [titre, setTitre] = React.useState('');
    const [limit, setLimit] = React.useState(20);
    const [errors, setErrors] = React.useState({});
    const [showPrintModal, hidePrintModal] = useModal(({ in: open, onExited }) => {
        return (
            <IframeDialog
                onExited={onExited}
                open={open}
                onClose={() => {
                    hidePrintModal(null);
                }}
                src={getPrintInventaireURL(data.filter(x => x.Article).map((x) => (`ids=${x.Article.Id}`)).join('&'), siteId, titre)}>
            </IframeDialog>
        )
    }, [data, siteId, titre]);

    const columns = React.useMemo(
        () => codeBarreListColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setTitle('Inventaire')
    }, [])

    React.useEffect(() => {
        loadData();
    }, [siteId, limit])

    const loadData = () => {
        showLoader(true)
        getInventoryList(siteId, limit)
            .then(data => {
                setData(data.map(x => ({
                    Article: x.Article,
                    QteStockReel: x.Article.QteStock,
                    QteStock: x.Article.QteStock,
                })))
            }).finally(() => {
                showLoader()
            });
    }

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

        if (filteredData.length < 1) {
            _errors['table'] = 'Ajouter des articles.';
        }

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const print = async () => {
        if (!areDataValid()) return;
        showPrintModal();
    }

    const save = async () => {
        if (!areDataValid()) return;

        const Id = uuidv4();
        const preparedData = {
            Id: Id,
            IdSite: siteId,
            Titre: titre,
            InventaireItems: data.filter(x => x.Article).map(d => ({
                Id: uuidv4(),
                IdInvetaire: Id,
                QteStock: d.QteStock,
                QteStockReel: d.QteStockReel,
                IdArticle: d.Article.Id,
            })),
        };

        showLoader(true);
        const response = await saveData(DOCUMENT, preparedData);
        showLoader();

        if (response?.Id) {
            showSnackBar();
            resetData();
            loadData();
        } else {
            showSnackBar({
                error: true,
                text: "Erreur!"
            });
        }
    }

    const resetData = () => {
        setTitre('');
        setTimeout(() => {
            setDate(new Date());
        }, 1000)
        addNewRow();
    }

    return (
        <>
            <Paper>
                <TitleIcon noBorder title="Inventaire" Icon={LocalMallOutlinedIcon} />
                <Box mt={4} display="flex">
                    <Box width={240}>
                        <TextField
                            value={titre}
                            fullWidth
                            onChange={({ target: { value } }) => setTitre(value)}
                            variant="outlined"
                            size="small"
                            label="Titre"
                        />
                    </Box>
                    <Box ml={2} width={140}>
                        <FormControl variant="outlined" size="small" fullWidth>
                            <Select
                                value={limit}
                                fullWidth
                                onChange={({ target: { value } }) => {
                                    setLimit(value);
                                }}
                            >
                                {
                                    [10, 20, 50, 100].map(x => (
                                        <MenuItem key={x} value={x}>{x}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
                    </Box>
                </Box>
                <Box mt={2}>
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
                        <Button style={{
                            marginRight: 12
                        }} startIcon={<PrintIcon />} variant="contained" color="primary" onClick={print}>
                            Imprimer
                         </Button>
                        <Button variant="contained" color="primary" onClick={save}>
                            Enregistrer
                         </Button>
                    </Box>
                </Box>
            </Paper>
        </>
    )
}


export const codeBarreListColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article',
        type: inputTypes.text.description,
        editable: true,
        width: '60%',
        Cell: ({
            value,
            row: { index },
            column: { id },
            updateMyData,
            addNewRow,
            data
        }) => {
            return (
                <ArticleAutocomplete
                    value={value}
                    inTable
                    placeholder="Entrer un article..."
                    onBlur={() => updateMyData(index, id, value)}
                    onChange={(_, selectedValue) => {
                        updateMyData(index, id, selectedValue);
                        updateMyData(index, 'QteStock', selectedValue?.QteStock)
                        if (data.filter(x => !x.Article).length === 1 || data.length === 1)
                            addNewRow();

                        const qteCell = document.querySelector(`#my-table #QteReel-${(index)} input`);
                        if (qteCell) {
                            setTimeout(() => {
                                qteCell.focus();
                            }, 200)
                        }
                    }}

                />
            )
        }
    },
    {
        Header: 'Qte. stock',
        accessor: 'QteStock',
        type: inputTypes.number.description,
        align: 'left',
        width: 40,
    },
    {
        Header: 'Qte. réel',
        accessor: 'QteStockReel',
        type: inputTypes.number.description,
        editable: true,
        align: 'left',
        width: 40,
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { index }, deleteRow }) => {
            return (
                <div style={{ textAlign: 'center' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(index)}>
                        <CloseIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
])

export default Inventaire;