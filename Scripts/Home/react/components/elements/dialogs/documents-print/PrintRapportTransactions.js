import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintRapportTransactionURL } from '../../../../utils/urlBuilder';

const PrintRapportTransaction = ({ dateFrom, dateTo, onClose, onExited, open }) => {

    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintRapportTransactionURL({
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString()
            })}>
        </IframeDialog>
    )
}

export default PrintRapportTransaction;