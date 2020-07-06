import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import PrintOutlinedIcon from '@material-ui/icons/PrintOutlined';
import { Box } from '@material-ui/core';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import { format } from 'date-fns';

export const getSuiviVentesColumns = () => ([
    {
        Header: 'Article',
        accessor: 'Article.Designation',
        type: inputTypes.text.description,
    },
    {
        Header: 'Client',
        accessor: 'BonLivraison.Client.Name',
        type: inputTypes.text.description,
    },
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.BonLivraison.Date), 'dd/MM/yyyy')
        },
        width: 40,
    },
    {
        Header: 'BL N#',
        accessor: 'BonLivraison.NumBon',
        type: inputTypes.text.description,
        width: 60,
    },
    {
        Header: 'Qte.',
        accessor: 'Qte',
        align: 'right',
        width: 30,
    },
    {
        id: 'Pu',
        Header: 'P.U.',
        accessor: 'Pu',
        align: 'right',
        accessor: (props) => {
            return formatMoney(props.Pu);
        },
        width: 40,
    },
    {
        id: 'actions',
        Header: '',
        Cell: ({ row: { original }, print }) => {
            return (
                <Box display="flex" justifyContent="flex-end">
                    <IconButton tabIndex={-1} size="small" onClick={() => print(original.BonLivraison)}>
                        <PrintOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24
    },
])