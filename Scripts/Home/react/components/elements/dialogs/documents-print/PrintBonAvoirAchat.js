import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintBonAvoirVenteURL } from '../../../../utils/urlBuilder';
import PaiementClientForm from '../../forms/PaiementClientForm';

const DOCUMENT_ITEMS = 'BonAvoirItems'

const PrintBonAvoirAchat = ({ document, onClose, onExited, open }) => {
    const [showForm, setShowForm] = React.useState(false);
    const [showStamp, setShowStamp] = React.useState(false);

    if (!document) return null;
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintBonAvoirVenteURL({
                id: document.Id,
                showStamp: showStamp
            })}>
            <Box p={1}>
                {document &&
                    <>
                        <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                            <FormControlLabel
                                control={<Switch
                                    checked={showStamp}
                                    onChange={(_, checked) => setShowStamp(checked)} />}
                                label="Afficher le cachet"
                            />
                        </Box>
                        <div>
                            <FormControlLabel
                                control={<Switch
                                    checked={showForm}
                                    onChange={(_, checked) => setShowForm(checked)}
                                />}
                                label="Remboursement de l'argent"
                            />
                        </div>
                        {showForm &&
                            <Box mt={2}>
                                <PaiementClientForm
                                    isAvoir
                                    amount={document[DOCUMENT_ITEMS]?.reduce((sum, curr) => (
                                        sum += curr.Pu * curr.Qte
                                    ), 0)}
                                    document={document}
                                    onSuccess={() => {
                                        //a workaround to refresh document
                                        const iframe = window.document.getElementById('iframe-dialog');
                                        iframe.src = iframe.src;
                                        setShowForm(false);
                                    }}
                                />
                            </Box>}
                    </>
                }
            </Box>
        </IframeDialog>
    )
}

export default PrintBonAvoirAchat;