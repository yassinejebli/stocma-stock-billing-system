import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import ImageOutlinedIcon from '@material-ui/icons/ImageOutlined';

export const articleColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
        width: 140
    },
    {
        id: 'QteStock',
        Header: 'Qte en stock',
        accessor: (props) => {
            return formatMoney(props.QteStock);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'PA',
        Header: 'Prix d\'achat',
        accessor: (props) => {
            return formatMoney(props.Article.PA);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'PVD',
        Header: 'Prix de vente',
        accessor: (props) => {
            return formatMoney(props.Article.PVD);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'TVA',
        Header: 'T.V.A',
        accessor: (props) => {
            return props.Article.TVA + '%';
        },
        type: inputTypes.text.description,
        align: 'right',
        width: 26
    },
    {
        Header: 'Unité',
        accessor: 'Article.Unite',
        type: inputTypes.text.description,
        align: 'center',
        width: 26
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, showImage }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.Article.Image && <IconButton tabIndex={-1} size="small" onClick={() => showImage(original.Article.Image)}>
                        <ImageOutlinedIcon />
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    {/* <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton> */}
                </Box>
            )
        },
        width: 24
    },
])


export const articlesMarginColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article',
        type: inputTypes.text.description,
        width: 140
    },
    {
        id: 'QteStock',
        Header: 'Qte en stock',
        accessor: (props) => {
            return formatMoney(props.QteStock);
        },
        type: inputTypes.text.description,
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
        id: 'QteSold',
        Header: 'Qte vendues',
        accessor: (props) => {
            return formatMoney(props.QteSold);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        Id: 'Marge',
        Header: 'Marge',
        accessor: (props) => {
            return formatMoney(props.Marge);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
])