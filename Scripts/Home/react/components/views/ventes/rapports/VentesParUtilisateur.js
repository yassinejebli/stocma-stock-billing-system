import React from 'react'
import ExpansionPanel from '../../../elements/expansion/ExpansionPanel'
import { makeStyles, Box, Button } from '@material-ui/core'
import DatePicker from '../../../elements/date-picker/DatePicker';

const useStyles = makeStyles(theme => ({
    root: {

    },
}));

const VentesParUtilisateurs = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);

    return (
        <ExpansionPanel title="Ventes / Opérateur">
            <DatePicker
                value={dateFrom}
                label="Date de début"
                onChange={(date) => {
                    date && date.setHours(0, 0, 0, 0);
                    setDateFrom(date)
                }}
            />
            <DatePicker
                style={{
                    marginLeft: 12
                }}
                value={dateTo}
                label="Date de fin"
                onChange={(date) => {
                    date && date.setHours(23, 59, 59, 999);
                    setDateTo(date)
                }}
            />
          
        </ExpansionPanel>
    )
}

export default VentesParUtilisateurs;
