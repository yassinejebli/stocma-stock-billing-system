import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintFactureURL } from '../../../../utils/urlBuilder';

const PrintFakeFacture = ({ onClose, onExited, open }) => {
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintFactureURL({
                IdFacture: document.Id
            })}>
        </IframeDialog>
    )
}

export default PrintFakeFacture;