import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { getLastPricePurchase } from '../../../../queries/articleQueries';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import { BarcodeScan } from 'mdi-material-ui'
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import ArticleAutocomplete from '../../article-autocomplete/ArticleAutocomplete';
import { format } from 'date-fns';
import { Box, Tooltip } from '@material-ui/core';
import HistoryIcon from '@material-ui/icons/History';

export const bonReceptionColumns = ({suiviModule}) => ([
    {
        Header: 'Article',
        accessor: 'Article',
        type: inputTypes.text.description,
        editable: true,
        width: '50%',
        Cell: ({
            value,
            row: { index },
            column: { id },
            updateMyData,
            addNewRow,
            owner,
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
                        if (selectedValue && owner)
                            getLastPricePurchase(selectedValue.Id, owner.Id).then(lastPricePurchase => {
                                updateMyData(index, 'Pu', lastPricePurchase);
                            });
                        else if (selectedValue)
                            updateMyData(index, 'Pu', selectedValue.PA);

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
        align: 'right'
    },
    {
        Header: 'P.U.',
        accessor: 'Pu',
        editable: true,
        type: inputTypes.number.description,
        align: 'right'
    },
    {
        id: 'TotalHT',
        Header: 'Montant',
        accessor: (props) => {
            return formatMoney(props.Pu * props.Qte);
        },
        type: inputTypes.text.description,
        editable: false,
        align: 'right'
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { index, original }, deleteRow, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {suiviModule?.Enabled && original.Article && <Tooltip title="Historiques des achats">
                        <IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                            <HistoryIcon />
                        </IconButton>
                    </Tooltip>}
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(index)}>
                        <CloseIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24,
        align: 'right'
    },
])



export const bonReceptionListColumns = ({barcodeModule}) => ([
    {
        Header: 'Fournisseur',
        accessor: 'Fournisseur.Name',
        type: inputTypes.text.description,
        width: 100
    },
    {
        Header: 'N#',
        accessor: 'NumBon',
        type: inputTypes.text.description,
        width: 60
    },
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
        width: 60
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 50,
        accessor: (props) => {
            const total = props.BonReceptionItems.reduce((sum, curr) => (
                sum += curr.Pu * curr.Qte
            ), 0);
            return formatMoney(total);
        },
        align: 'right'
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, customAction, print }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                    {barcodeModule?.Enabled&&<Tooltip title="Imprimer les codes-barres">
                        <IconButton tabIndex={-1} size="small" onClick={() => customAction(original.Id)}>
                            <BarcodeScan />
                        </IconButton>
                    </Tooltip>}
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original.Id)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box >
            )
        },
        width: 24
    },
])