import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintFakeFactureURL } from '../../../../utils/urlBuilder';

const PrintFakeFacture = ({ document, onClose, onExited, open }) => {
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintFakeFactureURL({
                IdFakeFacture: document.Id
            })}>
        </IframeDialog>
    )
}

export default PrintFakeFacture;