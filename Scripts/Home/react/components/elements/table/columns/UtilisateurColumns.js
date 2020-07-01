import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';

export const utilisateurColumns = () => ([
    {
        Header: 'Utilisateur',
        accessor: 'UserName',
        type: inputTypes.text.description,
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, updateRow, deleteRow }) => {
            const isAdmin = original?.Id === '00000000-0000-0000-0000-000000000000';
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    {!isAdmin&&<IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>}
                </Box>
            )
        },
        width: 24
    },
])