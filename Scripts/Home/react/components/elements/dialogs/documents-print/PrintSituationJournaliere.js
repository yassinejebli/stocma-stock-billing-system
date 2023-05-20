import React from "react";
import IframeDialog from "../IframeDialog";
import { getPrintSituationJournaliereURL } from "../../../../utils/urlBuilder";

const PrintSituationJournaliere = ({ date, onClose, onExited, open }) => {
  return (
    <IframeDialog
      onExited={onExited}
      open={open}
      onClose={onClose}
      src={getPrintSituationJournaliereURL({
        dt: date.toISOString(),
      })}
    ></IframeDialog>
  );
};

export default PrintSituationJournaliere;
