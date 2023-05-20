import React from "react";
import ExpansionPanel from "../../../elements/expansion/ExpansionPanel";
import { Box, Button } from "@material-ui/core";
import DatePicker from "../../../elements/date-picker/DatePicker";
import PrintIcon from "@material-ui/icons/Print";
import { useModal } from "react-modal-hook";
import PrintSituationJournaliere from "../../../elements/dialogs/documents-print/PrintSituationJournaliere";

const SitutationJournaliere = () => {
  const today = new Date();
  const [date, setDate] = React.useState(today);
  const [showPrintModal, hidePrintModal] = useModal(
    ({ in: open, onExited }) => {
      return (
        <PrintSituationJournaliere
          onExited={onExited}
          open={open}
          date={date}
          onClose={hidePrintModal}
        />
      );
    },
    [date]
  );

  return (
    <ExpansionPanel title="Transactions du jour">
      <DatePicker
        value={date}
        label="Date"
        onChange={(date) => {
          date && date.setHours(0, 0, 0, 0);
          setDate(date);
        }}
      />
      <Box ml={2}>
        <Button
          variant="contained"
          color="primary"
          startIcon={<PrintIcon />}
          onClick={() => {
            if (date) showPrintModal();
          }}
        >
          Imprimer
        </Button>
      </Box>
    </ExpansionPanel>
  );
};

export default SitutationJournaliere;
