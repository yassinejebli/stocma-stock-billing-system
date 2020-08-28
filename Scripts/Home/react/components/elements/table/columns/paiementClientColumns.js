import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import MonetizationOnIcon from '@material-ui/icons/MonetizationOn';
import LaunchIcon from '@material-ui/icons/Launch';
import PrintIcon from '@material-ui/icons/Print';
import PrintDisabledIcon from '@material-ui/icons/PrintDisabled';
import KeyboardReturnIcon from '@material-ui/icons/KeyboardReturn';
import {
    CartArrowRight, CartArrowUp, Cash, Checkbook, Sale, CashRefund,
    CreditCard,
} from 'mdi-material-ui'
import { Box, Tooltip } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { format } from 'date-fns';

export const getPaiementClientListColumns = ({ isFiltered, canUpdateBonLivraisons }) => ([
    (isFiltered && {
        Header: 'Client',
        accessor: 'Client.Name',
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
        Header: 'BL N#',
        accessor: 'BonLivraison.NumBon',
        type: inputTypes.text.description
    },
    {
        Header: 'Type',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
        width: 120,
        Cell: ({ row: { original } }) => {
            const typePaiement = original.TypePaiement;
            let icon = null;
            if (typePaiement.IsEspece)
                icon = <Cash style={{ color: '#eb9123' }} />;
            else if (typePaiement.IsVente)
                icon = <CartArrowRight style={{ color: '#3478c9' }} />;
            else if (typePaiement.IsBankRelated)
                icon = <Checkbook style={{ color: 'grey' }} />;
            else if (typePaiement.IsRemise)
                icon = <Sale style={{ color: '#39c217' }} />;
            else if (typePaiement.IsRemboursement)
                icon = <CashRefund style={{ color: '#7e3ee6' }} />;
            else if (typePaiement.IsAvoir)
                icon = <CartArrowUp style={{ color: '#e07575' }} />;

            return (
                <Box display="flex" alignItems="center">
                    {icon}
                    <Box ml={0.4}>
                        {typePaiement.Name}
                    </Box>
                </Box>
            )
        },
    },
    {
        id: 'Debit',
        Header: 'Débit',
        accessor: 'Debit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Debit);
        },
        width: 60,
    },
    {
        id: 'Credit',
        Header: 'Crédit',
        accessor: 'Credit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Credit);
        },
        width: 60,
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
        Cell: ({ row: { original }, print, deleteRow, updateRow, disableRow, customAction, updateRow2 }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.IdBonLivraison && <IconButton tabIndex={-1} size="small" onClick={() => print(original.BonLivraison)}>
                        <PrintIcon />
                    </IconButton>}
                    {original.EnCaisse && <IconButton tabIndex={-1} size="small" disableRipple>
                        <Tooltip title="Encaissé">
                            <MonetizationOnIcon style={{
                                color: 'green'
                            }} />
                        </Tooltip>
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => disableRow(original)}>
                        <Tooltip title="Supprimer cette ligne lors de l'impression">
                            <PrintDisabledIcon />
                        </Tooltip>
                    </IconButton>
                    {original.IdBonLivraison && canUpdateBonLivraisons && <IconButton tabIndex={-1} size="small" onClick={() => updateRow2(original.IdBonLivraison)}>
                        <Tooltip title="Modifier ce document">
                            <LaunchIcon />
                        </Tooltip>
                    </IconButton>}
                    {!original.IdBonLivraison && !original.IdBonAvoirC && <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>}
                    {original?.TypePaiement?.IsBankRelated && !original?.TypePaiement?.IsImpaye && <IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
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
]).filter(x => x)

export const getBankPaiementsClientListColumns = ({ isFiltered }) => ([
    (isFiltered && {
        Header: 'Client',
        accessor: 'Client.Name',
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
        Header: 'Type',
        accessor: 'TypePaiement.Name',
        type: inputTypes.text.description,
        width: 60,
    },
    {
        id: 'Debit',
        Header: 'Débit',
        accessor: 'Debit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Debit);
        },
        width: 60,
    },
    {
        id: 'Credit',
        Header: 'Crédit',
        accessor: 'Credit',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Credit);
        },
        width: 60,
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
        Cell: ({ row: { original }, print, deleteRow, updateRow, disableRow, customAction, updateRow2 }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    {original.EnCaisse && <IconButton tabIndex={-1} size="small" disableRipple>
                        <Tooltip title="Encaissé">
                            <MonetizationOnIcon style={{
                                color: 'green'
                            }} />
                        </Tooltip>
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    {!original?.TypePaiement?.IsImpaye && <IconButton tabIndex={-1} size="small" onClick={() => customAction(original)}>
                        <Tooltip title="Chèque/Effet impayé">
                            <KeyboardReturnIcon />
                        </Tooltip>
                    </IconButton>}
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
]).filter(x => x)