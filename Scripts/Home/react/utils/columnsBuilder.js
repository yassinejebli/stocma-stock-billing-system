import React from 'react'
import { inputTypes } from '../types/input'
import ArticleAutocomplete from "../components/elements/article-autocomplete/ArticleAutocomplete";
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { format } from 'date-fns';
import { formatMoney } from './moneyUtils';
import { getLastPriceSale } from '../queries/articleQueries';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { Box } from '@material-ui/core';


export const getBonLivraisonColumns = () => ([
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
                            updateMyData(index, 'Pu', selectedValue.PVD);

                        if (data.filter(x => !x.Article).length === 1 || data.length === 1)
                            addNewRow();
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
        Header: 'Pu.',
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


export const getClientColumns = () => ([
    {
        Header: 'Client',
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
    // {
    //     Header: 'Adresse',
    //     accessor: 'Adresse',
    //     type: inputTypes.text.description,
    //     width: 120
    // },
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
            return formatMoney(props.Solde || 0)
        },
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        Header: 'Plafond',
        accessor: 'Plafond',
        type: inputTypes.text.description,
        align: 'right'
    },
    {
        id: 'remove',
        Header: '',
        Cell: ({ row: { original: { Id } }, deleteRow }) => {
            return (
                <div style={{ textAlign: 'right' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
    {
        id: 'update',
        Header: '',
        Cell: ({ row: { original }, updateRow }) => {
            return (
                <div style={{ textAlign: 'center' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
])


export const getFournisseurColumns = () => ([
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
        id: 'remove',
        Header: '',
        Cell: ({ row: { original: { Id } }, deleteRow }) => {
            return (
                <div style={{ textAlign: 'right' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
    {
        id: 'update',
        Header: '',
        Cell: ({ row: { original }, updateRow }) => {
            return (
                <div style={{ textAlign: 'center' }}>
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                </div>
            )
        },
        width: 24,
        align: 'right'
    },
])


export const getBonLivraisonListColumns = () => ([
    {
        Header: 'Client',
        accessor: 'Client.Name',
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
        Header: 'Type de règ.',
        accessor: 'TypeReglement',
        type: inputTypes.text.description,
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 30,
        accessor: (props) => {
            const total = props.BonLivraisonItems.reduce((sum, curr) => (
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