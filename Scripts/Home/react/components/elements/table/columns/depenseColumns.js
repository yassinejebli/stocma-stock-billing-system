import React from 'react'
import IconButton from '@material-ui/core/IconButton';
import { inputTypes } from '../../../../types/input';
import { formatMoney } from '../../../../utils/moneyUtils';
import CloseIcon from '@material-ui/icons/Close';
import DeleteForeverOutlinedIcon from '@material-ui/icons/DeleteForeverOutlined';
import EditOutlinedIcon from '@material-ui/icons/EditOutlined';
import { format } from 'date-fns';
import { Box } from '@material-ui/core';
import DepenseAutocomplete from '../../type-depense-autocomplete/TypeDepenseAutocomplete';

export const depenseColumns = () => ([
    {
        Header: 'Dépense',
        accessor: 'Name',
        editable: true,
    },
    {
        Header: 'Catégorie',
        accessor: 'TypeDepense',
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
                <DepenseAutocomplete
                    value={value}
                    inTable
                    placeholder="Entrer une catégorie..."
                    onBlur={() => updateMyData(index, id, value)}
                    onChange={(_, selectedValue) => {
                        updateMyData(index, id, selectedValue);
                        if (data.filter(x => !x.TypeDepense).length === 1 || data.length === 1)
                            addNewRow();
                    }}

                />
            )
        }
    },
    {
        Header: 'Montant',
        accessor: 'Montant',
        editable: true,
        type: inputTypes.number.description,
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


export const depenseListColumns = () => ([
    {
        Id: 'Date',
        Header: 'Date',
        type: inputTypes.text.description,
        accessor: props => {
            return format(new Date(props.Date), 'dd/MM/yyyy')
        },
        width: 60,
    },
    {
        Header: 'Titre',
        accessor: 'Titre',
        type: inputTypes.text.description,
        width: 100,
    },
    {
        Header: 'Dépenses',
        accessor: props=>{
            return props.DepenseItems.map(x=>(x.Name + " : " + formatMoney(x.Montant))).filter(x=>x).join("\n")
        },
        type: inputTypes.text.description,
        width: 200,
    },
    {
        id: 'Total',
        Header: 'Total',
        type: inputTypes.text.description,
        width: 30,
        accessor: (props) => {
            const total = props.DepenseItems.reduce((sum, curr) => (
                sum += curr.Montant
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
                    {/* <IconButton tabIndex={-1} size="small" onClick={() => print(original)}>
                        <PrintOutlinedIcon />
                    </IconButton> */}
                    <IconButton tabIndex={-1} size="small" onClick={() => updateRow(original.Id)}>
                        <EditOutlinedIcon />
                    </IconButton>
                    <IconButton tabIndex={-1} size="small" onClick={() => deleteRow(original.Id)}>
                        <DeleteForeverOutlinedIcon />
                    </IconButton>
                </Box>
            )
        },
        width: 24,
    },
])