import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintFakeFactureAchatURL } from '../../../../utils/urlBuilder';

const PrintFakeFactureAchat = ({ document, onClose, onExited, open }) => {
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintFakeFactureAchatURL({
                IdFakeFacture: document.Id
            })}>
        </IframeDialog>
    )
}

export default PrintFakeFactureAchat;