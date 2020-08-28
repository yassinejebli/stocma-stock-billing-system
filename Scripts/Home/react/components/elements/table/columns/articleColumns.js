import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import ImageOutlinedIcon from '@material-ui/icons/ImageOutlined';

export const articleColumns = ({articleImageModule}) => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
        width: 140
    },
    {
        Header: 'Famille',
        accessor: 'Article.Categorie.Name',
        type: inputTypes.text.description,
        width: 80
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
        Header: 'Unité',
        accessor: 'Article.Unite',
        type: inputTypes.text.description,
        align: 'center',
        width: 26
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
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow, showImage }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.Article?.Image && articleImageModule?.Enabled && <IconButton tabIndex={-1} size="small" onClick={() => showImage(original.Article.Image)}>
                        <ImageOutlinedIcon />
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
].filter(x=>x))



export const fakeArticleColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Designation',
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
        Header: 'Unité',
        accessor: 'Unite',
        type: inputTypes.text.description,
        align: 'center',
        width: 26
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
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
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
        Header: 'Qte vendue',
        accessor: (props) => {
            return formatMoney(props.QteSold);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        Id: 'Turnover',
        Header: 'Total',
        accessor: (props) => {
            return formatMoney(props.Turnover);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        Id: 'Margin',
        Header: 'Marge',
        accessor: (props) => {
            return formatMoney(props.Margin);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
])


export const articlesNotSellingColumns = () => ([
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
        id: 'PVD',
        Header: 'Prix de vente',
        accessor: (props) => {
            return formatMoney(props.PVD);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        Id: 'Total',
        Header: 'Total',
        accessor: (props) => {
            return formatMoney(props.Total);
        },
        type: inputTypes.text.description,
        align: 'right'
    },
])