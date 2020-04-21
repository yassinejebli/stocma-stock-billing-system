import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import BottomNavigation from '@material-ui/core/BottomNavigation';
import BottomNavigationAction from '@material-ui/core/BottomNavigationAction';
import PostAddIcon from '@material-ui/icons/PostAdd';
import { useHistory } from 'react-router-dom';

const useStyles = makeStyles({
    root: {
        position: 'fixed',
        bottom: 0,
        left: 0,
        right: 0,
    },
});

//TODO: should be dynamic
const menuItems = [
    {
        label: 'Bon Livraison',
        value: 0,
        icon: <PostAddIcon style={{ color: 'rgb(14, 230, 158)' }} />,
        route: '/BonLivraison'
    },
    {
        label: 'Bon Reception',
        value: 1,
        icon: <PostAddIcon style={{ color: 'rgb(14, 140, 230)' }} />,
        route: '/BonReception'
    },
    {
        label: 'Facture',
        value: 2,
        icon: <PostAddIcon style={{ color: 'rgb(111, 14, 230)' }} />,
        route: '/Facture'
    }
]; 
function Table({ columns, data, updateMyData, skipPageReset }) {
  // For this example, we're using pagination to illustrate how to stop
  // the current page from resetting when our data changes
  // Otherwise, nothing is different here.
  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    prepareRow,
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
      // use the skipPageReset option to disable page resetting temporarily
      autoResetPage: !skipPageReset,
      // updateMyData isn't part of the API, but
      // anything we put into these options will
      // automatically be available on the instance.
      // That way we can call this function from our
      // cell renderer!
      updateMyData,
    },
    usePagination
  )

  // Render the UI for your table
  return (
    <>
      <table {...getTableProps()}>
        <thead>
          {headerGroups.map(headerGroup => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map(column => (
                <th {...column.getHeaderProps()}>{column.render('Header')}</th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {page.map((row, i) => {
            prepareRow(row)
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map(cell => {
                  return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                })}
              </tr>
            )
          })}
        </tbody>
      </table>
      <div className="pagination">
        <button onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
          {'<<'}
        </button>{' '}
        <button onClick={() => previousPage()} disabled={!canPreviousPage}>
          {'<'}
        </button>{' '}
        <button onClick={() => nextPage()} disabled={!canNextPage}>
          {'>'}
        </button>{' '}
        <button onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>
          {'>>'}
        </button>{' '}
        <span>
          Page{' '}
          <strong>
            {pageIndex + 1} of {pageOptions.length}
          </strong>{' '}
        </span>
        <span>
          | Go to page:{' '}
          <input
            type="number"
            defaultValue={pageIndex + 1}
            onChange={e => {
              const page = e.target.value ? Number(e.target.value) - 1 : 0
              gotoPage(page)
            }}
            style={{ width: '100px' }}
          />
        </span>{' '}
        <select
          value={pageSize}
          onChange={e => {
            setPageSize(Number(e.target.value))
          }}
        >
          {[10, 20, 30, 40, 50].map(pageSize => (
            <option key={pageSize} value={pageSize}>
              Show {pageSize}
            </option>
          ))}
        </select>
      </div>
    </>
  )
}
const NavigationMenu = () => {
    const classes = useStyles();
    const [selectedMenuItem, setSelectedMenuItem] = React.useState(0);
    const history = useHistory();
    return (
        <BottomNavigation className={classes.root} value={selectedMenuItem} onChange={(_, value) => setSelectedMenuItem(value)}>
            {
                menuItems.map(mi => (
                    <BottomNavigationAction
                        key={mi.value}
                        value={mi.value} label={mi.label}
                        showLabel icon={mi.icon}
                        onClick={() => history.replace(mi.route)}
                    />
                ))
            }
        </BottomNavigation>
    )
}

export default NavigationMenu;
