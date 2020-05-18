import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';

export const fournisseurColumns = ({useVAT}) => ([
    {
        Header: 'Fournisseur',
        accessor: 'Name',
        type: inputTypes.text.description,
        width: '30%'
    },
    {
        Header: 'I.C.E',
        accessor: 'ICE',
        type: inputTypes.text.description,
        width: 140
    },
    {
        Header: 'Adresse',
        accessor: 'Adresse',
        type: inputTypes.text.description,
        width: 120
    },
    {
        Header: 'Tel',
        accessor: 'Tel',
        type: inputTypes.text.description,
        width: 80
    },
    {
        id: 'Solde',
        Header: 'Solde',
        accessor: (props) => {
            const solde = useVAT ? props.SoldeFacture : props.Solde;
            console.log('props.SoldeFacture', useVAT)
            return formatMoney(solde || 0)
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24,
        align: 'right'
    },
])