import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintRapportVentesBLURL } from '../../../../utils/urlBuilder';

const PrintRapportVentesBL = ({ dateFrom, dateTo, onClose, onExited, open }) => {

    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintRapportVentesBLURL({
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString()
            })}>
        </IframeDialog>
    )
}

export default PrintRapportVentesBL;