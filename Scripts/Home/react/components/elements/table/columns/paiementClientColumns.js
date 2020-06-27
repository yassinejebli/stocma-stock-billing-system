import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import PrintDisabledIcon from '@material-ui/icons/PrintDisabled';
import KeyboardReturnIcon from '@material-ui/icons/KeyboardReturn';
import { Box, Tooltip } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { format } from 'date-fns';

export const getPaiementClientListColumns = ({isFiltered}) => ([
    (isFiltered && {
        Header: 'Client',
        accessor: 'Client.Name',
        editable: true,
        type: inputTypes.text.description,
    }),
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
    },
    {
        Header: 'BL N#',
        accessor: 'BonLivraison.NumBon',
        type: inputTypes.text.description
    },
    {
        Header: 'Type',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
        width: 60
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
        Cell: ({ row: { original }, print, deleteRow, updateRow, disableRow, customAction }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.IdBonLivraison && <IconButton tabIndex={-1} size="small" onClick={() => print(original.BonLivraison)}>
                        <PrintOutlinedIcon />
                    </IconButton>}
                    {!original.IdBonLivraison && !original.IdBonAvoirC && <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => disableRow(original)}>
                        <Tooltip title="Supprimer cette ligne lors de l'impression">
                            <PrintDisabledIcon />
                        </Tooltip>
                    </IconButton>
                    {original?.TypePaiement?.IsBankRelated&&<IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                        <Tooltip title="Chèque/Effet impayé">
                            <KeyboardReturnIcon />
                        </Tooltip>
                    </IconButton>}
                    {!original.IdBonLivraison && !original.IdBonAvoirC && <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>}
                </Box>
            )
        },
        width: 24
    },
]).filter(x=>x)