import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import ArticleAutocomplete from '../../article-autocomplete/ArticleAutocomplete';
import { format } from 'date-fns';
import { Box } from '@material-ui/core';

export const stockMouvementColumns = () => ([
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
                <ArticleAutocomplete
                    value={value}
                    inTable
                    placeholder="Entrer un article..."
                    onBlur={() => updateMyData(index, id, value)}
                    onChange={(_, selectedValue) => {
                        updateMyData(index, id, selectedValue);
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



export const stockMouvementListColumns = () => ([
    {
        Header: 'Magasin d\'émission',
        accessor: 'SiteFrom.Name',
        type: inputTypes.text.description,
        width: 120
    },
    {
        Header: 'Magasin de réception',
        accessor: 'SiteTo.Name',
        type: inputTypes.text.description,
        width: 120
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
        Header: 'Commentaire',
        accessor: 'Comment',
        type: inputTypes.text.description,
        width: 60
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