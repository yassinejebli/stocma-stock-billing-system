﻿import React from 'react'
import { useTable, usePagination, useAsyncDebounce } from 'react-table'
import { makeStyles } from '@material-ui/core/styles';
import Input from '../input/Input';
import { Box, IconButton } from '@material-ui/core';
import SkipPreviousIcon from '@material-ui/icons/SkipPrevious';
import SkipNextIcon from '@material-ui/icons/SkipNext';
import NavigateNextIcon from '@material-ui/icons/NavigateNext';
import NavigateBeforeIcon from '@material-ui/icons/NavigateBefore';
import { useSite } from '../../providers/SiteProvider';

const border = '1px solid #d8d8d8';

const useStyles = makeStyles(theme => ({
    root: {
        '& tbody, textarea':{
            fontSize: '13px !important'
        },
        '& thead th': {
            paddingBottom: 14,
            borderBottom: border,
            fontWeight: 500,
            textAlign: 'left'
        },
        '& tbody tr > td': {
            borderBottom: border
        },
    },

}));



// Be sure to pass our updateMyData and the skipPageReset option
function Table({
    columns,
    data,
    updateMyData,
    updateRow,
    deleteRow,
    skipPageReset,
    serverPagination,
    fetchData,
    pageCount: _pageCount,
    addNewRow,
    showImage,
    print,
    convert,
    owner,
    filters,
    totalItems }) {
    const classes = useStyles();
    const {siteId} = useSite();
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        prepareRow,
        rows,
        page,
        canPreviousPage,
        canNextPage,
        pageOptions,
        pageCount,
        gotoPage,
        nextPage,
        previousPage,
        setPageSize,
        state: { pageIndex, pageSize },
    } = useTable(
        {
            columns,
            data,
            defaultColumn,
            autoResetPage: !skipPageReset,
            updateMyData,
            addNewRow,
            deleteRow,
            showImage,
            updateRow,
            print,
            convert,
            owner,
            initialState: { pageIndex: 0 },
            manualPagination: Boolean(serverPagination),
            manualFilters: Boolean(serverPagination),
            pageCount: _pageCount,
            siteId
        },
        usePagination
    );
    const myData = serverPagination ? page : rows;
    const onFetchDataDebounced = useAsyncDebounce(fetchData, 100);

    React.useEffect(() => {
        if(serverPagination&&fetchData){
            onFetchDataDebounced({ pageIndex, pageSize, filters });
        }
    }, [onFetchDataDebounced, pageIndex, pageSize, filters])

    React.useEffect(()=>{
        if(pageIndex !== 0)
            gotoPage(0);
    },[filters])

    return (
        <>
            <table {...getTableProps()} className={classes.root} border="0" width="100%">
                <colgroup>
                    {
                        columns.map((column, index) => (
                            <col key={index} width={column.width || 100} />
                        ))
                    }
                </colgroup>
                <thead>
                    {headerGroups.map((headerGroup, index) => (
                        <tr key={index} {...headerGroup.getHeaderGroupProps()}>
                            {headerGroup.headers.map(({ align = 'left', ...column }) => (
                                <th key={column.id} style={{ textAlign: align }} {...column.getHeaderProps()}>{column.render('Header')}</th>
                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps()}>
                    {myData.map((row, i) => {
                        prepareRow(row);
                        const disabled = row?.original?.Disabled;
                        return (
                            <tr key={i} {...row.getRowProps()}>
                                {row.cells.map((cell, index) => {
                                    return <td key={index} style={{color: disabled ? 'rgb(172, 174, 176)':'#000'}} {...cell.getCellProps()}>{cell.render('Cell')}</td>
                                })}
                            </tr>
                        )
                    })}
                </tbody>
            </table>
            {serverPagination && <Box mt={data.length>0?0:60} display="flex" justifyContent="flex-end" alignItems="center">
                <IconButton disableRipple onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
                    <SkipPreviousIcon />
                </IconButton>
                <IconButton disableRipple onClick={() => previousPage()} disabled={!canPreviousPage}>
                    <NavigateBeforeIcon />
                </IconButton>
                <div>
                    {pageIndex + 1} - {pageOptions.length}
                </div>
                <IconButton disableRipple onClick={() => nextPage()} disabled={!canNextPage}>
                    <NavigateNextIcon />
                </IconButton>
                <IconButton disableRipple onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>
                    <SkipNextIcon />
                </IconButton>
                {/* Nombre d'elements: {totalItems} */}
            </Box>}
        </>
    )
}

// Create an editable cell renderer
const EditableCell = ({
    value: initialValue,
    row: { index, original },
    column: { id, editable, type = 'text', align },
    updateMyData, // This is a custom function that we supplied to our table instance
}) => {

    const disabled = original?.Disabled;
    // We need to keep and update the state of the cell normally
    const [value, setValue] = React.useState(initialValue)

    const onChange = e => {
        setValue(e.target.value)
    }

    // We'll only update the external data when the input is blurred
    const onBlur = () => {
        if (updateMyData)
            updateMyData(index, id, value)
    }

    // If the initialValue is changed external, sync it up with our state
    React.useEffect(() => {
        setValue(initialValue)
    }, [initialValue])

    return <Input
        inTable
        tabIndex={id?.toLowerCase().includes('total')?-1:null}
        readOnly={!editable}
        value={value || ''}
        align={align}
        onChange={onChange}
        onBlur={onBlur}
        type={type}
        style={{
            color: disabled ? 'rgb(172, 174, 176)' : '#000'
        }}
    />
}

// Set our editable cell renderer as the default Cell renderer
const defaultColumn = {
    Cell: EditableCell,
}


export default Table
