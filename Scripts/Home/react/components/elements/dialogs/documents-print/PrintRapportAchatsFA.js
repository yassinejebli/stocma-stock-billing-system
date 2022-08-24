import React from "react";
import IframeDialog from "../IframeDialog";
import { getPrintRapportAchatsFAURL } from "../../../../utils/urlBuilder";

const PrintRapportAchatsFA = ({
  dateFrom,
  dateTo,
  onClose,
  onExited,
  open,
}) => {
  return (
    <IframeDialog
      onExited={onExited}
      open={open}
      onClose={onClose}
      src={getPrintRapportAchatsFAURL({
        dateFrom: dateFrom.toISOString(),
        dateTo: dateTo.toISOString(),
      })}
    ></IframeDialog>
  );
};

export default PrintRapportAchatsFA;
