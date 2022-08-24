import React from "react";
import IframeDialog from "../IframeDialog";
import { getPrintFakeFactureURL } from "../../../../utils/urlBuilder";

const PrintFakeFacture = ({ document, onClose, onExited, open }) => {
  return (
    <IframeDialog
      onExited={onExited}
      open={open}
      onClose={onClose}
      src={getPrintFakeFactureURL({
        IdFakeFacture: document.Id,
      })}
    ></IframeDialog>
  );
};

export const PrintNewFakeFacture = ({
  document,
  onClose,
  onExited,
  open,
  isEASAndNewFacture,
}) => {
  return (
    <IframeDialog
      onExited={onExited}
      open={open}
      onClose={onClose}
      src={getPrintFakeFactureURL({
        IdFakeFacture: document.Id,
        isEASAndNewFacture,
      })}
    ></IframeDialog>
  );
};

export default PrintFakeFacture;
