import React from 'react';
import IframeDialog from '../IframeDialog';
import { Box, FormControlLabel, Switch } from '@material-ui/core';
import { getPrintBonAvoirVenteURL } from '../../../../utils/urlBuilder';
import PaiementClientForm from '../../forms/PaiementClientForm';
import { useSite } from '../../../providers/SiteProvider';
import { useAuth } from '../../../providers/AuthProvider';

const DOCUMENT_ITEMS = 'BonAvoirCItems'

const PrintBonAvoirVente = ({ document, onClose, onExited, open }) => {
    const { canManagePaiementsClients } = useAuth();
    const isClientDivers = document?.Client?.IsClientDivers;
    const { useVAT } = useSite();
    const [showForm, setShowForm] = React.useState(false);
    const [showBalance, setShowBalance] = React.useState(false);
    const [showStamp, setShowStamp] = React.useState(false);

    if (!document) return null;
    return (
        <IframeDialog
            onExited={onExited}
            open={open}
            onClose={onClose}
            src={getPrintBonAvoirVenteURL({
                id: document.Id,
                showBalance,
                showStamp: showStamp
            })}>
            <Box p={1}>
                {document &&
                    <>
                        <Box display="flex" justifyContent="space-between" flexWrap="wrap">
                            {/* {!useVAT && <FormControlLabel
                                control={<Switch
                                    checked={showBalance}
                                    onChange={(_, checked) => setShowBalance(checked)} />}
                                label="Afficher le solde"
                            />
                            } */}
                            <FormControlLabel
                                control={<Switch
                                    checked={showStamp}
                                    onChange={(_, checked) => setShowStamp(checked)} />}
                                label="Afficher le cachet"
                            />
                        </Box>
                        {!isClientDivers&&canManagePaiementsClients&&<div>
                            <FormControlLabel
                                control={<Switch
                                    checked={showForm}
                                    onChange={(_, checked) => setShowForm(checked)}
                                />}
                                label="Remboursement de l'argent"
                            />
                        </div>}
                        {showForm && !isClientDivers &&
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

export default PrintBonAvoirVente;