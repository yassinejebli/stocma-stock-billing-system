import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { format } from 'date-fns';

export const getSuiviAchatsColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
        width: 140
    },
    {
        Header: 'Fournisseur',
        accessor: 'BonReception.Fournisseur.Name',
        type: inputTypes.text.description,
        width: 160
    },
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.BonReception.Date), 'dd/MM/yyyy')
        },
    },
    {
        Header: 'BR N#',
        accessor: 'BonReception.NumBon',
        type: inputTypes.text.description
    },
    {
        Header: 'Qte.',
        accessor: 'Qte',
        align: 'right'
    },
    {
        id: 'Pu',
        Header: 'P.U.',
        accessor: 'Pu',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Pu);
        },
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, print }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original.BonReception)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
])