import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintTarifURL } from '../../../../utils/urlBuilder';
import { FormControlLabel, Switch, Box } from '@material-ui/core';

const PrinTarif = ({ document, onClose, onExited, open }) => {
    if (!document) return null;
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintTarifURL({
                id: document.Id,
            })}>
        </IframeDialog>
    )
}

export default PrinTarif;