import React from 'react'
import ExpansionPanel from '../../../elements/expansion/ExpansionPanel'
import { makeStyles, Box, Button } from '@material-ui/core'
import DatePicker from '../../../elements/date-picker/DatePicker';
import PrintIcon from '@material-ui/icons/Print';
import { useModal } from 'react-modal-hook';
import PrintRapportVentesFA from '../../../elements/dialogs/documents-print/PrintRapportVentesFA';

const useStyles = makeStyles(theme => ({
    root: {

    },
}));

const VentesFA = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const [showPrintModal, hidePrintModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintRapportVentesFA
                onExited={onExited}
                open={open}
                dateFrom={dateFrom}
                dateTo={dateTo}
                onClose={hidePrintModal}
            />
        )
    }, [dateFrom, dateTo]);

    return (
        <ExpansionPanel title="Rapport des ventes (FA)">
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
            <Box ml={2}>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<PrintIcon />}
                    onClick={showPrintModal}
                >
                    Imprimer
                </Button>
            </Box>
        </ExpansionPanel>
    )
}

export default VentesFA;
