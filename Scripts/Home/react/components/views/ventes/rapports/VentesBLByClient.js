import React from 'react'
import ExpansionPanel from '../../../elements/expansion/ExpansionPanel'
import { makeStyles, Box, Button } from '@material-ui/core'
import DatePicker from '../../../elements/date-picker/DatePicker';
import PrintIcon from '@material-ui/icons/Print';
import { useModal } from 'react-modal-hook';
import ClientAutocomplete from '../../../elements/client-autocomplete/ClientAutocomplete';
import PrintRapportVentesBLByClient from '../../../elements/dialogs/documents-print/PrintRapportVentesBLByClient';

const useStyles = makeStyles(theme => ({
    root: {

    },
}));

const VentesBLByClient = () => {
    const today = new Date();
    const firstDayCurrentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    const lastDayCurrentMonth = new Date();
    firstDayCurrentMonth.setHours(0, 0, 0, 0);
    lastDayCurrentMonth.setHours(23, 59, 59, 999);
    const [client, setClient] = React.useState(null);
    const [dateFrom, setDateFrom] = React.useState(firstDayCurrentMonth);
    const [dateTo, setDateTo] = React.useState(lastDayCurrentMonth);
    const [showPrintModal, hidePrintModal] = useModal(({ in: open, onExited }) => {
        return (
            <PrintRapportVentesBLByClient
                onExited={onExited}
                open={open}
                id={client?.Id}
                dateFrom={dateFrom}
                dateTo={dateTo}
                onClose={hidePrintModal}
            />
        )
    }, [dateFrom, dateTo, client]);



    return (
        <ExpansionPanel title="Rapport des ventes par client (BL)">
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
                <ClientAutocomplete
                    label="Client"
                    value={client}
                    onChange={(_, value) => setClient(value)}
                />
            </Box>
            <Box ml={2}>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<PrintIcon />}
                    onClick={() => {
                        if (client)
                            showPrintModal()
                    }}
                >
                    Imprimer
                </Button>
            </Box>
        </ExpansionPanel>
    )
}

export default VentesBLByClient;
