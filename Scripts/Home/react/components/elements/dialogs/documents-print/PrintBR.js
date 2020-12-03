import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintBonReceptionURL } from '../../../../utils/urlBuilder';
import PaiementFournisseurForm from '../../forms/PaiementFournisseurForm';
import { useSite } from '../../../providers/SiteProvider';

const DOCUMENT_ITEMS = 'BonReceptionItems'

const PrintBR = ({document, onClose, onExited, open}) => {
    const {useVAT} = useSite();
    const [showForm, setShowForm] = React.useState(false);

    if(!document) return null;
    return (
        <IframeDialog
                onExited={onExited}
                open={open}
                onClose={onClose}
                src={getPrintBonReceptionURL({
                    IdBonReception: document.Id
                })}>
                <Box p={1}>
                    {document &&
                        <>
                            {!useVAT&&<div>
                                <FormControlLabel
                                    control={<Switch
                                        checked={showForm}
                                        onChange={(_, checked) => setShowForm(checked)}
                                    />}
                                    label="Paiement effectué"
                                />
                            </div>}
                            {showForm && !useVAT &&
                                <Box mt={2}>
                                    <PaiementFournisseurForm
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

export default PrintBR;