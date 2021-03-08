import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintRapportVentesBLByClientURL } from '../../../../utils/urlBuilder';

const PrintRapportVentesBLByClient = ({ id, dateFrom, dateTo, onClose, onExited, open }) => {

    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintRapportVentesBLByClientURL({
                id,
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString()
            })}>
        </IframeDialog>
    )
}

export default PrintRapportVentesBLByClient;