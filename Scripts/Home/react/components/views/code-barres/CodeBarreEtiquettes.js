import { Button, TextField, makeStyles, IconButton } from '@material-ui/core'
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
import { BarcodeScan } from 'mdi-material-ui'
import PrintCodeBarreEtiquette from '../../elements/dialogs/PrintCodeBarreEtiquette'
import { useModal } from 'react-modal-hook'
import { useLocation } from 'react-router-dom'
import qs from 'qs'
import { useLoader } from '../../providers/LoaderProvider'
import { getSingleData } from '../../../queries/crudBuilder'
import ArticleCategoriesAutocomplete from '../../elements/article-categories-autocomplete/ArticleCategoriesAutocomplete'

const emptyLine = {
    Article: null,
    BarCode: '',
    Qte: 1,
}

const useStyles = makeStyles({

})

const CodeBarreEtiquettes = () => {
    const classes = useStyles();
    const { setTitle } = useTitle();
    const { showLoader } = useLoader();
    const location = useLocation();
    const BonReceptionId = qs.parse(location.search, { ignoreQueryPrefix: true }).BonReceptionId;
    const [data, setData] = React.useState([emptyLine]);
    const [errors, setErrors] = React.useState({});
    const [selectedCategory, setSelectedCategory] = React.useState(null);
    const [showPrintBarcodeLabelModal, hidePrintBarcodeLabelModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintCodeBarreEtiquette
                onExited={onExited}
                open={open}
                ids={data.filter(x => x.Article).map((x, i) => (`ids[${i}].key=` + x.Article.Id + `&ids[${i}].value=${x.Qte}`)).join('&')}
                onClose={() => {
                    hidePrintBarcodeLabelModal(null);
                }}
            />
        )
    }, [data]);

    const columns = React.useMemo(
        () => codeBarreListColumns(),
        []
    );
    const [skipPageReset, setSkipPageReset] = React.useState(false);

    React.useEffect(() => {
        setSkipPageReset(false)
    }, [data])

    React.useEffect(() => {
        setTitle('Impression des codes à barres')

        if (BonReceptionId) {
            showLoader(true);
            getSingleData('BonReceptions', BonReceptionId, ['BonReceptionItems' + '/' + 'Article'])
                .then(response => {
                    setData(response.BonReceptionItems?.filter(x=>x.Article.IsBarCodePrintable).map(x => ({
                        Article: x.Article,
                        Qte: x.Qte,
                    })));
                }).catch(err => console.error(err))
                .finally(() => showLoader(false));
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

        if (filteredData.length < 1) {
            _errors['table'] = 'Ajouter des articles.';
        }

        setErrors(_errors);
        return Object.keys(_errors).length === 0;
    }

    const print = async () => {
        if (!areDataValid()) return;
        showPrintBarcodeLabelModal();
    }

    console.log('test', { selectedCategory })
    return (
        <>
            <Paper>
                <TitleIcon noBorder title="Impression des codes à barres" Icon={BarcodeScan} />
                <Box mt={4} width={300}>
                    <ArticleCategoriesAutocomplete
                        value={selectedCategory}
                        withArticles
                        onChange={(_, selectedValue) => {
                            setSelectedCategory(selectedValue || null)
                            if (selectedValue)
                                setData([...selectedValue.Articles.map(x => ({
                                    Article: x,
                                    BarCode: x.BarCode,
                                    Qte: 1,
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
                    <Box mt={4} display="flex" justifyContent="flex-end">
                        <Button startIcon={<PrintIcon />} variant="contained" color="primary" onClick={print}>
                            Imprimer
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
                        updateMyData(index, 'BarCode', value?.BarCode);
                        if (data.filter(x => !x.Article).length === 1 || data.length === 1)
                            addNewRow();

                        const qteCell = document.querySelector(`#my-table #Qte-${(index)} input`);
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
        Header: 'Qte.',
        accessor: 'Qte',
        type: inputTypes.number.description,
        editable: true,
        align: 'left',
        width: 40,
    },
    {
        Header: 'Code à barres',
        id: 'BarCode',
        accessor: 'Article.BarCode',
        Cell: ({
            value,
        }) => {
            return (
                value ? <div style={{
                    fontFamily: "'Libre Barcode 39'",
                    fontSize: 30,
                    height: 30
                }}>
                    *{value}*
                </div>
                    : null
            )
        },
        type: inputTypes.text.description,
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

export default CodeBarreEtiquettes;