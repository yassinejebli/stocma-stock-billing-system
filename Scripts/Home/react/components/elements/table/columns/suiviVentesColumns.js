import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';

export const getSuiviVentesColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
        width: 140
    },
    {
        Header: 'Client',
        accessor: 'BonLivraison.Client.Name',
        type: inputTypes.text.description,
        width: 160
    },
    {
        Header: 'BL N#',
        accessor: 'BonLivraison.NumBon',
        type: inputTypes.text.description
    },
    {
        Header: 'Qte.',
        accessor: 'Qte',
        align: 'right'
    },
    {
        id: 'Pu',
        Header: 'P.U.',
        accessor: 'Pu',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Pu);
        },
    },
    // {
    //     id: 'actions',
    //     Header: '',
    //     Cell: ({ row: { original }, updateRow, deleteRow }) => {
    //         return (
    //             <Box display="flex" justifyContent="flex-end">
    //                 <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
    //                     <EditOutlinedIcon />
    //                 </IconButton>
    //                 <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
    //                     <DeleteForeverOutlinedIcon />
    //                 </IconButton>
    //             </Box>
    //         )
    //     },
    //     width: 24
    // },
])