import React from 'react';
import IframeDialog from '../IframeDialog';
import { getPrintRapportVentesFAURL } from '../../../../utils/urlBuilder';

const PrintRapportVentesFA = ({ dateFrom, dateTo, onClose, onExited, open }) => {

    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintRapportVentesFAURL({
                dateFrom: dateFrom.toISOString(),
                dateTo: dateTo.toISOString()
            })}>
        </IframeDialog>
    )
}

export default PrintRapportVentesFA;