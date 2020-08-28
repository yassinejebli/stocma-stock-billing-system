import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { format } from 'date-fns';
import CloseIcon from '@material-ui/icons/Close';
import { Box, Tooltip } from '@material-ui/core';
import FakeArticleAutocomplete from '../../article-autocomplete/FakeArticleAutocomplete';
import Input from '../../input/Input';

export const fakeFactureColumns = () => ([
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
            data
        }) => {
            return (
                <>
                    <FakeArticleAutocomplete
                        value={value}
                        inTable
                        placeholder="Entrer un article..."
                        onBlur={() => updateMyData(index, id, value)}
                        onChange={(_, selectedValue) => {
                            updateMyData(index, id, selectedValue);
                            if (selectedValue)
                                updateMyData(index, 'Pu', selectedValue.PVD);
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
                    {value && <Box mb={1} style={{
                        color: value.MinStock > value.QteStock ? 'red' : 'green'
                    }}>
                        <Tooltip title={"CODE:" + Math.floor(1000 + Math.random() * 9000) +
                            "-" + formatMoney(value.PA) +
                            "-" + Math.floor(1000 + Math.random() * 9000)}>
                            <span>
                                {value.QteStock} en stock
                            </span>
                        </Tooltip>
                    </Box>}
                </>
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
        align: 'right',
        Cell: ({
            value,
            row: { original, index },
            column: { id },
            updateMyData,
        }) => {
            return (
                <>

                    <Input
                        value={value || ''}
                        inTable
                        align="right"
                        type="number"
                        onFocus={(event) => event.target.select()}
                        onChange={({ target: { value: _value } }) => updateMyData(index, id, _value)}
                        onBlur={() => updateMyData(index, id, value)}
                    />
                    {console.log({original})}
                    {Boolean(value && original?.Article?.PA >= value) && <Box mb={1}>
                        <div style={{
                            marginLeft: 'auto',
                            color: 'red',
                            width: 'fit-content',
                        }}>
                            Vérifier le prix!
                        </div>
                    </Box>}
                </>
            )
        }
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



export const fakeFactureListColumns = () => ([
    {
        Header: 'Client',
        accessor: 'Client.Name',
        type: inputTypes.text.description,
        width: 100
    },
    {
        Header: 'Société',
        accessor: 'ClientName',
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
        width: 50
    },
    {
        Header: 'Mode de paiement',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
        width: 80,
    },
    {
        Header: 'Note (numéro de chèque...)',
        accessor: 'Comment',
        type: inputTypes.text.description,
    },
    {
        Id: 'DateEcheance',
        Header: 'Date d\'échéance',
        type: inputTypes.text.description,
        accessor: props => {
            return props.DateEcheance ? format(new Date(props.DateEcheance), 'dd/MM/yyyy') : ''
        },
        width: 50
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 45,
        accessor: (props) => {
            const total = props.FakeFactureItems.reduce((sum, curr) => (
                sum += curr.Pu * curr.Qte
            ), 0);
            return formatMoney(total);
        },
        align: 'right'
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, print }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original.Id)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
])