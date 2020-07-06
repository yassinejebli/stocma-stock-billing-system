import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import KeyboardReturnIcon from '@material-ui/icons/KeyboardReturn';
import { Box, Tooltip } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { format } from 'date-fns';

export const getPaiementFournisseurListColumns = ({isFiltered}) => ([
    (isFiltered && {
        Header: 'Fournisseur',
        accessor: 'Fournisseur.Name',
        editable: true,
        type: inputTypes.text.description,
        width: 120,
    }),
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
        width: 70,
    },
    {
        Header: 'BR N#',
        accessor: 'BonReception.NumBon',
        type: inputTypes.text.description
    },
    {
        Header: 'Type',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
        width: 100
    },
    {
        id: 'Debit',
        Header: 'Débit',
        accessor: 'Debit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Debit);
        },
    },
    {
        id: 'Credit',
        Header: 'Crédit',
        accessor: 'Credit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Credit);
        },
    },
    {
        Id: 'DateEcheance',
        Header: 'Échéance',
        type: inputTypes.text.description,
        accessor: props => {
            return props.DateEcheance ? format(new Date(props.DateEcheance), 'dd/MM/yyyy') : ''
        },
    },
    {
        Header: 'Commentaire',
        accessor: 'Comment',
        type: inputTypes.text.description
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, print, deleteRow, updateRow, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.IdBonReception && <IconButton tabIndex={-1} size="small" onClick={() => print(original.BonReception)}>
                        <PrintOutlinedIcon />
                    </IconButton>}
                    {!original.IdBonReception && !original.IdBonAvoir && <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>}
                    {original?.TypePaiement?.IsBankRelated&&<IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                        <Tooltip title="Chèque/Effet impayé">
                            <KeyboardReturnIcon />
                        </Tooltip>
                    </IconButton>}
                    {!original.IdBonReception && !original.IdBonAvoir && <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>}
                </Box>
            )
        },
        width: 24
    },
]).filter(x=>x)