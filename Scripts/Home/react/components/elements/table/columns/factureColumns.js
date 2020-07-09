import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { format } from 'date-fns';
import { Box } from '@material-ui/core';

export const factureColumns = ({ factureDiscount }) => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
        width: '50%',
    },
    {
        Header: 'BL N#',
        accessor: 'Description',
        type: inputTypes.text.description,
        width: 180,
    },
    {
        Header: 'Qte.',
        accessor: 'Qte',
        type: inputTypes.number.description,
        align: 'right'
    },
    {
        Header: 'P.U.',
        accessor: 'Pu',
        type: inputTypes.number.description,
        align: 'right'
    },
    ((factureDiscount?.Enabled) && {
        Header: 'Remise',
        accessor: 'Discount',
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
        align: 'right'
    },
].filter(x => x))



export const factureListColumns = () => ([
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
        width: 60
    },
    {
        Header: 'Mode de paiement',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 30,
        accessor: (props) => {
            const total = [].concat(...props.BonLivraisons.map(x => x.BonLivraisonItems.map(y => y)))
                .reduce((sum, curr) => {
                    sum += curr.Pu * curr.Qte;
                    if (curr.Discount) {
                        if (!curr.PercentageDiscount)
                            sum -= Number(curr.Discount)
                        else
                            sum -= sum * parseFloat(curr.Discount) / 100;
                    }
                    return sum;
                }, 0);

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