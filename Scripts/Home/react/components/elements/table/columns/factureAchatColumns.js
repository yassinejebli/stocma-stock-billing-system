import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { format } from 'date-fns';
import { Box } from '@material-ui/core';

export const factureAchatColumns = () => ([
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
    {
        id: 'TotalHT',
        Header: 'Montant',
        accessor: (props) => {
            const total = props.Pu * props.Qte;
            return formatMoney(total);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
])

export const factureAchatListColumns = () => ([
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
            const total = [].concat(...props.BonReceptions.map(x => x.BonReceptionItems.map(y => y)))
                .reduce((sum, curr) => {
                    sum += curr.Pu * curr.Qte;
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