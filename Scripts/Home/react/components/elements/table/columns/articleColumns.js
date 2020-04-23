import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';

export const articleColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Designation',
        type: inputTypes.text.description,
        width: 140
    },
    // {
    //     Header: 'Qte Total',
    //     accessor: 'QteStockSum',
    //     type: inputTypes.text.description,
    //     align: 'right'
    // },
    {
        id: 'QteStock',
        Header: 'Qte en stock',
        Cell: ({ row: { original }, siteId }) => {
            const articleSite = original.ArticleSites?.find(x => x.IdSite = siteId);
            return (<div style={{textAlign: 'right'}}>
                {formatMoney(articleSite?.QteStock)}
            </div>)
        },
        type: inputTypes.number.description,
        align: 'right'
    },
    {
        id: 'PA',
        Header: 'Prix d\'achat',
        accessor: (props) => {
            return formatMoney(props.PA);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'PVD',
        Header: 'Prix de vente',
        accessor: (props) => {
            return formatMoney(props.PVD);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'TVA',
        Header: 'T.V.A',
        accessor: (props) => {
            return props.TVA + '%';
        },
        type: inputTypes.text.description,
        align: 'right',
        width: 26
    },
    {
        Header: 'Unité',
        accessor: 'Unite',
        type: inputTypes.text.description,
        align: 'center',
        width: 26
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow }) => {
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