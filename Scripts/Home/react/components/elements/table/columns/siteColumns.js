import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';

export const siteColumns = () => ([
    {
        Header: 'Code',
        accessor: 'Code',
        type: inputTypes.text.description,
        width: 60,
    },
    {
        Header: 'Dépôt/Magasin',
        accessor: 'Name',
        type: inputTypes.text.description,
        width: 140,
    },
    {
        Header: 'Adresse',
        accessor: 'Address',
        type: inputTypes.text.description
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
                    {original.Id!=1&&<IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>}
                </Box>
            )
        },
        width: 24
    },
])