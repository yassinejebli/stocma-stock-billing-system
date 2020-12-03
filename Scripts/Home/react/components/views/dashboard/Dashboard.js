import React from 'react';
import { useTitle } from '../../providers/TitleProvider';
import ProfitAndExpenses from './ProfitAndExpenses';
import MonthlyProfitAndExpenses from './MonthlyProfitAndExpenses';
import { Box, FormControl, Select, withStyles, InputBase, MenuItem } from '@material-ui/core';
import MonthlyProfitAndCash from './MonthlyProfitAndCash';

const currentYear = new Date().getFullYear();
const years = [ currentYear, currentYear - 1, currentYear - 2 ]
const Dashboard = () => {
    const { setTitle } = useTitle();
    const [selectedYear, setSelectedYear] = React.useState(years[0]);
    React.useEffect(() => {
        setTitle('Tableau du bord')
    }, []);

    return (
        <div>
            <Box display="flex" justifyContent="flex-end">
                <FormControl>
                    <Select
                        value={selectedYear}
                        onChange={({target: {value}})=> setSelectedYear(value)}
                        input={<BootstrapInput size="small" />}
                        size="small"
                        // className={classes.select}
                    >
                        {
                            years.map(x => (
                                <MenuItem key={x} value={x}>{x}</MenuItem>
                            ))
                        }
                    </Select>
                </FormControl>
            </Box>
            <div>
                <ProfitAndExpenses year={selectedYear} />
                <MonthlyProfitAndExpenses year={selectedYear} />
                {/* <MonthlyProfitAndCash year={selectedYear} /> */}
            </div>
        </div>
    )
}

const BootstrapInput = withStyles((theme) => ({
    input: {
      minWidth: 40,
      borderRadius: 4,
      position: 'relative',
      backgroundColor: 'transparent',
      border: '1px solid #ced4da',
      fontSize: 16,
      padding: '6px 26px 6px 12px',
      transition: theme.transitions.create(['border-color', 'box-shadow']),
      // Use the system font instead of the default Roboto font.
      '&:focus': {
        borderRadius: 4
      },
    },
  }))(InputBase);

export default Dashboard;
