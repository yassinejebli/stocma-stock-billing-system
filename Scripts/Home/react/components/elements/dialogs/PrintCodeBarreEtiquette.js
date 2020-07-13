import React from 'react';
import { getPrintCodeBarreEtiquetteURL, getPrintMultipleCodeBarreEtiquetteURL } from '../../../utils/urlBuilder';
import IframeDialog from './IframeDialog';

const PrintCodeBarreEtiquette = ({ids, barCode, designation, onClose, onExited, open }) => {
    console.log({ids})
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={ids ? getPrintMultipleCodeBarreEtiquetteURL(ids) : getPrintCodeBarreEtiquetteURL({
                BarCode: barCode,
                Designation: designation,
            })}>
        </IframeDialog>
    )
}

export default PrintCodeBarreEtiquette;