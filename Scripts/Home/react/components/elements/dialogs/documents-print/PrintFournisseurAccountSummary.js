import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintFournisseurAccountSummaryURL } from '../../../../utils/urlBuilder';

const PrintFournisseurAccountSummary = ({ fournisseurId, dateFrom, dateTo, onClose, onExited, open }) => {
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintFournisseurAccountSummaryURL({
                id: fournisseurId,
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString()
            })}>
        </IframeDialog>
    )
}

export default PrintFournisseurAccountSummary;