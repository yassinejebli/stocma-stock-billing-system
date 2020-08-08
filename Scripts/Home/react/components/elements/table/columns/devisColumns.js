import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { getLastPriceSale } from '../../../../queries/articleQueries';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import ArticleAutocomplete from '../../article-autocomplete/ArticleAutocomplete';
import SyncIcon from '@material-ui/icons/Sync';
import { format } from 'date-fns';
import { Box, Tooltip } from '@material-ui/core';

export const devisColumns = ({ devisDiscount }) => ([
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
                            getLastPriceSale(selectedValue.Id, owner.Id).then(lastPriceSale => {
                                updateMyData(index, 'Pu', lastPriceSale);
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
    ((devisDiscount?.Enabled) && {
        Header: 'Remise',
        accessor: 'Discount',
        editable: true,
        type: inputTypes.text.description,
        align: 'right'
    }),
    {
        id: 'TotalHT',
        Header: 'Montant',
        accessor: (props) => {
            let discount = 0;
            const total = props.Pu * props.Qte;
            if (props.Discount) {
                if (!isNaN(props.Discount))
                    discount = props.Discount
                else if (/^\d+(\.\d+)?%$/.test(props.Discount)) {
                    discount = total * parseFloat(props.Discount) / 100;
                }
            }
            return formatMoney(total - discount);
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
].filter(x => x))



export const devisListColumns = () => ([
    {
        Header: 'Client',
        accessor: 'Client.Name',
        type: inputTypes.text.description,
        width: 100
    },
    {
        Header: 'Nom du client sur le devis',
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
        width: 60
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 30,
        accessor: (props) => {
            const discount = props.DevisItems.reduce((sum, curr) => {
                const total = curr.Pu * curr.Qte;
                if (curr.Discount) {
                    if (!curr.PercentageDiscount)
                        sum += Number(curr.Discount)
                    else
                        sum += total * parseFloat(curr.Discount) / 100;
                }
                return sum;
            }, 0);

            const total = props.DevisItems.reduce((sum, curr) => (
                sum += curr.Pu * curr.Qte
            ), 0);

            return formatMoney(total - discount);
        },
        align: 'right'
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, print, convert }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                    <Tooltip title="Convertir en bon de livraison">
                        <IconButton tabIndex={-1} size="small" onClick={() => convert(original.Id)}>
                            <SyncIcon />
                        </IconButton>
                    </Tooltip>
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