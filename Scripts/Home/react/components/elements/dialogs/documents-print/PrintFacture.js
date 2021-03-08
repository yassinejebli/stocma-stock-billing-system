import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintFactureURL } from '../../../../utils/urlBuilder';
import PaiementFactureClientForm from '../../forms/PaiementFactureClientForm';
import { useSite } from '../../../providers/SiteProvider';

const PrintFacture = ({document, onClose, onExited, open}) => {
    const [showForm, setShowForm] = React.useState(false);
    const {useVAT} = useSite();
    if(!document) return null;
    return (
        <IframeDialog
                onExited={onExited}
                open={open}
                onClose={onClose}
                src={getPrintFactureURL({
                    IdFacture: document.Id
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
                                    label="Paiement reçu"
                                />
                            </div>}
                            {showForm &&
                                <Box mt={2}>
                                    <PaiementFactureClientForm
                                        amount={[].concat(...document.BonLivraisons.map(x => x.BonLivraisonItems.map(y => ({
                                            Qte: y.Qte,
                                            Pu: y.Pu
                                        })))).reduce((sum, curr) => (
                                            sum += curr.Pu * curr.Qte
                                        ), 0)}
                                        document={document}
                                        onSuccess={() => {
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

export default PrintFacture;